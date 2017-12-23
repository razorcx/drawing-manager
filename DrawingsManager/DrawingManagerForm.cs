using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using Tekla.Structures;
using Tekla.Structures.Drawing;
using Tekla.Structures.Model;
using Color = System.Drawing.Color;
using Part = Tekla.Structures.Model.Part;

namespace DrawingsManager
{
	public partial class DrawingManager : Form
	{
		private readonly Model _model = new Model();
		private readonly DrawingHandler _drawingHandler = new DrawingHandler();
		private List<Drawing> _drawings;

		public DrawingManager()
		{
			InitializeComponent();
		}

		private void DrawingManager_Load(object sender, EventArgs e)
		{
			_drawings = _drawingHandler.GetDrawings().ToAList<Drawing>();

			AddDrawingsToDataGridView();
		}

		private void AddDrawingsToDataGridView()
		{
			if (_drawingHandler.GetConnectionStatus())
			{
				var drawingItems = _drawings.AsParallel().Select(d =>
				{
					var part = GetPart(d);
					var assy_pos = string.Empty;
					var part_pos = string.Empty;
					part?.GetReportProperty("ASSEMBLY_POS", ref assy_pos);
					part?.GetReportProperty("PART_POS", ref part_pos);

					var size = Math.Round(d.Layout.SheetSize.Width / 25.4, 1) 
						+ "x" + Math.Round(d.Layout.SheetSize.Height / 25.4, 1);

					var drawingItem = new
					{
						Size = size,
						Status = d.UpToDateStatus.ToString(),
						Type = GetDrawingTypeCharacter(d),
						DrawingMark = d.Mark,
						AssyMark = assy_pos,
						PartMark = part_pos,
						Name = d.Name,
						CreationDate = d.CreationDate,
						ModificationDate = d.ModificationDate,
						Title1 = d.Title1,
						Title2 = d.Title2,
						Title3 = d.Title3,
						Id = part?.Identifier.ID,
					};
					return drawingItem;
				}).ToList();

				dataGridView1.DataSource = drawingItems.OrderBy(d => d.DrawingMark).ToList();

				var rows = dataGridView1.Rows.OfType<DataGridViewRow>().ToList();
				rows.ForEach(r =>
				{
					dynamic item = r.DataBoundItem;
					var status = item.Status;
					if (status == "DrawingIsUpToDate")
					{
						r.DefaultCellStyle.BackColor = Color.LightGreen;
						r.DefaultCellStyle.ForeColor = Color.Black;
					}
					else if (status == "PartsWereModified")
					{
						r.DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
						r.DefaultCellStyle.ForeColor = Color.Crimson;
					}
					else if (status == "DrawingWasCloned")
					{
						r.DefaultCellStyle.BackColor = Color.LightSkyBlue;
					}
					else if (status == "DrawingIsUpToDateButMayNeedChecking")
					{
						r.DefaultCellStyle.BackColor = Color.Coral;
					}
					else if (status == "NumberOfPartsInNumberingSeriesIncreased")
					{
						r.DefaultCellStyle.BackColor = Color.DarkGoldenrod;
					}
					else if (status == "AllPartsDeleted")
					{
						r.DefaultCellStyle.BackColor = Color.DarkGray;
					}
					else if (status == "OriginalPartDeleted")
					{
						r.DefaultCellStyle.BackColor = Color.CadetBlue;
					}
				});

			}
		}

		private string GetDrawingTypeCharacter(Drawing drawingInstance)
		{
			string result = "U"; // Unknown drawing

			if (drawingInstance is GADrawing)
				result = "G";
			else if (drawingInstance is AssemblyDrawing)
				result = "A";
			else if (drawingInstance is CastUnitDrawing)
				result = "C";
			else if (drawingInstance is MultiDrawing)
				result = "M";
			else if (drawingInstance is SinglePartDrawing)
				result = "W";
			return result;
		}

		public Tekla.Structures.Model.ModelObject GetPart(Drawing drawing)
		{
			if (drawing is AssemblyDrawing)
			{
				var part =
					new Model().SelectModelObject(((AssemblyDrawing) drawing).AssemblyIdentifier);
				return part;
			}
			if (drawing is SinglePartDrawing)
			{
				var part =
					new Model().SelectModelObject(((SinglePartDrawing) drawing).PartIdentifier);
				return part;
			}
			return null;
		}

		private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			var rows = dataGridView1.SelectedRows.OfType<DataGridViewRow>().ToList();

			var members = new ArrayList();
			rows.ForEach(row =>
			{
				dynamic r = row.DataBoundItem;
				if (r.Id == null) return;
				var id = (int) r.Id;
				var modelObject = _model.SelectModelObject(new Identifier(id));
				members.Add(modelObject);

			});

			new Tekla.Structures.Model.UI.ModelObjectSelector().Select(members);

			var allParts = new List<Part>();

			var parts = rows
				.Where(r => ((dynamic) r.DataBoundItem).Id != null)
				.OrderBy(r => ((dynamic)r.DataBoundItem).AssyMark)
				.SelectMany(row =>
				{
					dynamic r = row.DataBoundItem;
					var id = (int) r.Id;
					var modelObject = _model.SelectModelObject(new Identifier(id));
					members.Add(modelObject);

					var assyPos = string.Empty;
					var partPos = string.Empty;
					var name = string.Empty;
					var profile = string.Empty;
					var material = string.Empty;
					var topLevel = string.Empty;
					var finish = string.Empty;

					var secondaries = new List<Tekla.Structures.Model.Part>();
					if (modelObject is Assembly)
					{
						var part = ((Assembly) modelObject).GetMainPart() as Tekla.Structures.Model.Part;
						((Assembly) modelObject).GetReportProperty("ASSEMBLY_POS", ref assyPos);
						part?.GetReportProperty("PART_POS", ref partPos);
						part?.GetReportProperty("NAME", ref name);
						part?.GetReportProperty("PROFILE", ref profile);
						part?.GetReportProperty("MATERIAL", ref material);
						part?.GetReportProperty("TOP_LEVEL", ref topLevel);
						part?.GetReportProperty("FINISH", ref finish);

						secondaries = ((Assembly) modelObject).GetSecondaries().OfType<Tekla.Structures.Model.Part>().ToList();

						allParts.Add(part);
					}
					else if (modelObject is Tekla.Structures.Model.Part)
					{
						var part = modelObject as Tekla.Structures.Model.Part;
						part?.GetReportProperty("ASSEMBLY_POS", ref assyPos);
						part?.GetReportProperty("PART_POS", ref partPos);
						part?.GetReportProperty("NAME", ref name);
						part?.GetReportProperty("PROFILE", ref profile);
						part?.GetReportProperty("MATERIAL", ref material);
						part?.GetReportProperty("TOP_LEVEL", ref topLevel);
						part?.GetReportProperty("FINISH", ref finish);

						allParts.Add(part);
					}

					var views = new List<dynamic>();
					var view = new
					{
						AssyMark = assyPos,
						PartMark = partPos,
						Name = name,
						Profile = profile,
						Material = material,
						Finish = finish,
						TopLevel = topLevel,
					};

					var secViews = secondaries.Select(s =>
					{
						s?.GetReportProperty("ASSEMBLY_POS", ref assyPos);
						s?.GetReportProperty("PART_POS", ref partPos);
						s?.GetReportProperty("NAME", ref name);
						s?.GetReportProperty("PROFILE", ref profile);
						s?.GetReportProperty("MATERIAL", ref material);
						s?.GetReportProperty("TOP_LEVEL", ref topLevel);
						s?.GetReportProperty("FINISH", ref finish);

						allParts.Add(s);

						return new
						{
							AssyMark = string.Empty,
							PartMark = partPos,
							Name = name,
							Profile = profile,
							Material = material,
							Finish = finish,
							TopLevel = topLevel,
						};
					})
					.OrderBy(s => s.PartMark)
					.ToList();

					views.Add(view);
					views.AddRange(secViews);

					return views;

				}).ToList();

			dataGridView2.DataSource = parts.ToList();

			var gridRows = dataGridView2.Rows.OfType<DataGridViewRow>().ToList();
			gridRows.ForEach(r =>
			{
				dynamic item = r.DataBoundItem;
				if (item.AssyMark == item.PartMark)
				{
					r.DefaultCellStyle.Font = new Font(FontFamily.GenericSansSerif, 9, FontStyle.Bold);
					r.DefaultCellStyle.BackColor = Color.LightCyan;
					r.DefaultCellStyle.ForeColor = Color.Black;
				}
			});

			richTextBox1.Text = allParts.ToJson();
		}

		private void buttonRefresh_Click(object sender, EventArgs e)
		{
			_drawings = _drawingHandler.GetDrawings().ToAList<Drawing>();
			AddDrawingsToDataGridView();
		}
	}

	public static class ExtensionMethods
	{
		public static List<T> ToAList<T>(this IEnumerator enumerator)
		{
			var list = new List<T>();
			while (enumerator.MoveNext())
			{
				var loop = (T)enumerator.Current;
				if (loop != null)
					list.Add(loop);
			}
			return list;
		}

		public static string ToJson(this object obj, bool formatting = true)
		{
			return
				formatting
					? JsonConvert.SerializeObject(obj, Formatting.Indented)
					: JsonConvert.SerializeObject(obj, Formatting.None);
		}

		public static T FromJson<T>(this string json)
		{
			return JsonConvert.DeserializeObject<T>(json);
		}
	}

}
