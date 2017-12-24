using System.Collections.Generic;
using System.Linq;
using Tekla.Structures.Drawing;
using Tekla.Structures.Model;

namespace DrawingsManager
{
	public class SingleDrawingManager : DrawingManagerBase
	{
		private readonly Model _model = new Model();

		public dynamic GetSingleDrawings(List<SinglePartDrawing> singlePartDrawings)
		{
			var singleDrawings = singlePartDrawings.AsParallel().Select(a =>
			{
				var mark = string.Empty;
				var part = _model.SelectModelObject(a.PartIdentifier) as Tekla.Structures.Model.Part;
				part?.GetReportProperty("PART_POS", ref mark);

				return new
				{
					SingleDrawing = a,
					Part = _model.SelectModelObject(a.PartIdentifier),
					DrawingMark = a.Mark,
					PartMark = mark,
				};
			}).ToList();

			return singleDrawings;
		}

		public List<SinglePartDrawing> GetSinglePartDrawings(List<Drawing> drawings)
		{
			return drawings.OfType<SinglePartDrawing>().ToList();
		}

		public string GetDrawingUsableMark(Drawing drawing)
		{
			var mark = string.Empty;
			if (drawing is SinglePartDrawing)
			{
				var part =
					new Model().SelectModelObject(((SinglePartDrawing)drawing).PartIdentifier);
				part?.GetReportProperty("PART_POS", ref mark);
				return mark;
			}
			return string.Empty;
		}

		public SinglePartDrawing GetSinglePartDrawing(string partMark)
		{
			var drawings = GetDrawings();
			var singlePartDrawings = GetSinglePartDrawings(drawings);

			return singlePartDrawings.AsParallel()
				.FirstOrDefault(s => GetDrawingUsableMark(s) == partMark);
		}

		public override bool SetActiveDrawing(Drawing drawing)
		{
			return Handler.SetActiveDrawing(drawing);
		}
	}
}