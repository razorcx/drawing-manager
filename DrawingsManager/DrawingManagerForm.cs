using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MoreLinq;
using Tekla.Structures;
using Tekla.Structures.Drawing;
using Tekla.Structures.Model;
using Color = System.Drawing.Color;
using ModelObject = Tekla.Structures.Model.ModelObject;
using Part = Tekla.Structures.Model.Part;

namespace DrawingsManager
{
	public partial class DrawingManagerForm : Form
	{
		private readonly Model _model = new Model();
		private readonly DrawingHandler _drawingHandler = new DrawingHandler();
		private List<Drawing> _drawings;
		private readonly Tekla.Structures.Model.Events _events = new Tekla.Structures.Model.Events();
		private object _selectionEventHandlerLock = new object();
		private object _changedObjectHandlerLock = new object();


		public DrawingManagerForm()
		{
			InitializeComponent();
			RegisterEventHandler();
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
				var revisions = new List<dynamic>(0);

				var drawingItems = _drawings.AsParallel().Select(d =>
				{
					var part = GetPart(d);
					var assy_pos = string.Empty;
					var part_pos = string.Empty;
					part?.GetReportProperty("ASSEMBLY_POS", ref assy_pos);
					part?.GetReportProperty("PART_POS", ref part_pos);

					var revisionMark = string.Empty;
					part?.GetReportProperty("DRAWING.REVISION.MARK", ref revisionMark);
					var revisionDescription = string.Empty;
					part?.GetReportProperty("DRAWING.REVISION.DESCRIPTION", ref revisionDescription);

					var revisionlastMark = string.Empty;
					part?.GetReportProperty("DRAWING.REVISION.LAST_MARK", ref revisionlastMark);

					var revisionItem = new 
					{
						Mark = revisionMark,
						Description = revisionDescription,
					};

					revisions.Add(revisionItem);

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
						Rev = revisionMark,
						RevDescription = revisionDescription,
						CreationDate = d.CreationDate,
						ModificationDate = d.ModificationDate,
						Title1 = d.Title1,
						Title2 = d.Title2,
						Title3 = d.Title3,
						Id = part?.Identifier.ID,
					};
					return drawingItem;
				}).ToList();

				dataGridView1.DataSource = 
					drawingItems
					.OrderBy(d => d.Type)
					.ThenBy(d => d.DrawingMark)
					.ThenBy(d => d.PartMark)
					.ThenBy(d => d.AssyMark)
					.ToList();

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

		public ModelObject GetPart(Drawing drawing)
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
				var id = (int)r.Id;
				var modelObject = _model.SelectModelObject(new Identifier(id));
				members.Add(modelObject);

			});

			new Tekla.Structures.Model.UI.ModelObjectSelector().Select(members);

			CreateBom(rows);
			CreateRevisionItem(rows);
		}

		private void CreateRevisionItem(List<DataGridViewRow> rows)
		{
			var revisionDetails = new List<string>();

			rows
				.Where(r => ((dynamic) r.DataBoundItem).Id != null)
				.OrderBy(r => ((dynamic) r.DataBoundItem).AssyMark)
				.ForEach(row =>
				{
					dynamic r = row.DataBoundItem;
					var id = (int) r.Id;
					var part = _model.SelectModelObject(new Identifier(id));

					var revisionMark = string.Empty;
					part?.GetReportProperty("DRAWING.REVISION.MARK", ref revisionMark);
					var revisionDescription = string.Empty;
					part?.GetReportProperty("DRAWING.REVISION.DESCRIPTION", ref revisionDescription);

					var revisionlastMark = string.Empty;
					part?.GetReportProperty("DRAWING.REVISION.LAST_MARK", ref revisionlastMark);

					var assy_pos = string.Empty;
					var part_pos = string.Empty;
					part?.GetReportProperty("ASSEMBLY_POS", ref assy_pos);
					part?.GetReportProperty("PART_POS", ref part_pos);

					revisionDetails.Add("Assembly Mark: " + assy_pos);
					if(!string.IsNullOrEmpty(part_pos))
						revisionDetails.Add("Part Mark: " + part_pos);
					revisionDetails.Add("Rev: " + revisionlastMark);
					revisionDetails.Add("Description: " + revisionDescription);
					revisionDetails.Add("-----------------------------------------------------------------");
				});

			listBox1.BeginInvoke((Action)(() =>
			{
				listBox1.DataSource = revisionDetails;
			}));
		}

		private void CreateBom(List<DataGridViewRow> rows)
		{
			if(!rows.Any()) return;

			var allParts = new List<Part>();

			var parts = rows
				.Where(r => ((dynamic)r.DataBoundItem).Id != null)
				.OrderBy(r => ((dynamic)r.DataBoundItem).AssyMark)
				.SelectMany(row =>
				{
					dynamic r = row.DataBoundItem;
					var id = (int)r.Id;
					var modelObject = _model.SelectModelObject(new Identifier(id));

					var assyPos = string.Empty;
					var partPos = string.Empty;
					var name = string.Empty;
					var profile = string.Empty;
					var material = string.Empty;
					var topLevel = string.Empty;
					var finish = string.Empty;

					var secondaries = new List<Part>();
					if (modelObject is Assembly)
					{
						var part = ((Assembly)modelObject).GetMainPart() as Tekla.Structures.Model.Part;
						((Assembly)modelObject).GetReportProperty("ASSEMBLY_POS", ref assyPos);
						part?.GetReportProperty("PART_POS", ref partPos);
						part?.GetReportProperty("NAME", ref name);
						part?.GetReportProperty("PROFILE", ref profile);
						part?.GetReportProperty("MATERIAL", ref material);
						part?.GetReportProperty("TOP_LEVEL", ref topLevel);
						part?.GetReportProperty("FINISH", ref finish);

						secondaries = ((Assembly)modelObject).GetSecondaries().OfType<Tekla.Structures.Model.Part>().ToList();

						allParts.Add(part);
					}
					else if (modelObject is Part)
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

			dataGridView1.BeginInvoke((Action) (() =>
			{
				dataGridView1.ClearSelection();
				rows.ForEach(r =>
				{
					r.Selected = true;
				});
			}));

			dataGridView2.BeginInvoke((Action)(() =>
			{
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
			}));

			richTextBox1.BeginInvoke((Action) (() =>
			{
				richTextBox1.Text = allParts.ToJson();
			}));
		}

		private void buttonRefresh_Click(object sender, EventArgs e)
		{
			_drawings = _drawingHandler.GetDrawings().ToAList<Drawing>();
			AddDrawingsToDataGridView();
		}

		private void buttonOpenDrawing_Click(object sender, EventArgs e)
		{
			OpenDrawing();
		}

		private void dataGridView1_DoubleClick(object sender, EventArgs e)
		{
			OpenDrawing();
		}

		private void OpenDrawing()
		{
			var row = dataGridView1.CurrentRow?.DataBoundItem;
			dynamic item = row;
			if (item == null) return;

			Drawing drawing;
			if (item.PartMark.ToString() != string.Empty)
			{
				var sdm = new SingleDrawingManager();
				drawing = sdm.GetSinglePartDrawing(item.PartMark.ToString());
				sdm.Handler.CloseActiveDrawing();
				sdm.SetActiveDrawing(drawing);
				return;
			}

			if (item.PartMark.ToString() != string.Empty || item.AssyMark.ToString() == string.Empty) return;

			var adm = new AssemblyDrawingManager();
			drawing = adm.GetAssemblyDrawing(item.AssyMark.ToString());
			adm.Handler.CloseActiveDrawing();
			adm.SetActiveDrawing(drawing);
		}

		public void RegisterEventHandler()
		{
			_events.SelectionChange += Events_SelectionChangeEvent;
			_events.ModelObjectChanged += Events_ModelObjectChangedEvent;
			_events.Register();
		}

		public void UnRegisterEventHandler()
		{
			_events.UnRegister();
		}

		void Events_SelectionChangeEvent()
		{
			/* Make sure that the inner code block is running synchronously */
			lock (_selectionEventHandlerLock)
			{
				System.Console.WriteLine("Selection changed event received.");

				var selection = new Tekla.Structures.Model.UI.ModelObjectSelector()
					.GetSelectedObjects()
					.ToAList<Part>();
				if (!selection.Any()) return;
				var rawRows = dataGridView1.Rows.OfType<DataGridViewRow>().ToList();

				var assemblies = selection.Select(p => p.GetAssembly());

				var rows = rawRows.AsParallel().Where(r =>
				{
					dynamic item = r.DataBoundItem;
					if (item.Id == null) return false;
					var id = (int)item.Id;
					return assemblies.FirstOrDefault(p => p.Identifier.ID == id) != null;
				}).ToList();

				CreateBom(rows);
			}
		}

		void Events_ModelObjectChangedEvent(List<ChangeData> changes)
		{
			/* Make sure that the inner code block is running synchronously */
			lock (_changedObjectHandlerLock)
			{
				foreach (ChangeData data in changes)
					System.Console.WriteLine("Changed event received " + ":" + data.Object.ToString() + ":" + " Type" + ":" + data.Type.ToString() + " guid: " + data.Object.Identifier.GUID.ToString());
				System.Console.WriteLine("Changed event received for " + changes.Count.ToString() + " objects");
			}
		}
	}
}
