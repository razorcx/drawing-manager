using System.Collections.Generic;
using System.Linq;
using Tekla.Structures.Drawing;
using Tekla.Structures.Model;

namespace DrawingsManager
{
	public class AssemblyDrawingManager : DrawingManagerBase
	{
		private readonly Model _model = new Model();

		public dynamic GetAssemblyDrawings(List<AssemblyDrawing> assemblyDrawings)
		{
			var assyDrawings = assemblyDrawings.AsParallel().Select(a =>
			{
				var mark = string.Empty;
				var part = _model.SelectModelObject(a.AssemblyIdentifier) as Assembly;
				part?.GetReportProperty("ASSEMBLY_POS", ref mark);

				return new
				{
					AssemblyDrawing = a,
					Assembly = _model.SelectModelObject(a.AssemblyIdentifier),
					DrawingMark = a.Mark,
					PartMark = mark,
				};
			}).ToList();

			return assyDrawings;
		}

		public List<AssemblyDrawing> GetAssemblyDrawings(List<Drawing> drawings)
		{
			return drawings.OfType<AssemblyDrawing>().ToList();
		}

		public string GetDrawingUsableMark(Drawing drawing)
		{
			var mark = string.Empty;
			if (drawing is AssemblyDrawing)
			{
				var part =
					new Model().SelectModelObject(((AssemblyDrawing)drawing).AssemblyIdentifier);
				part?.GetReportProperty("ASSEMBLY_POS", ref mark);
				return mark;
			}
			return string.Empty;
		}

		public AssemblyDrawing GetAssemblyDrawing(string partMark)
		{
			var drawings = GetDrawings();
			var assemblyDrawings = GetAssemblyDrawings(drawings);

			return assemblyDrawings.AsParallel()
				.FirstOrDefault(s => GetDrawingUsableMark(s) == partMark);
		}

		public override bool SetActiveDrawing(Drawing drawing)
		{
			return Handler.SetActiveDrawing(drawing);
		}
	}
}