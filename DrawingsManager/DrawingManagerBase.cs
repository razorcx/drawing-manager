using System.Collections.Generic;
using Tekla.Structures.Drawing;

namespace DrawingsManager
{
	public abstract class DrawingManagerBase
	{
		public DrawingHandler Handler => new DrawingHandler();

		public List<Drawing> GetDrawings()
		{
			var drawingCollection = Handler.GetDrawings();
			if (drawingCollection.GetSize() < 1)
				return null;

			drawingCollection.SelectInstances = false;

			var drawings = new List<Drawing>();
			foreach (Drawing drawing in drawingCollection)
				drawings.Add(drawing);

			return drawings;
		}

		public abstract bool SetActiveDrawing(Drawing drawing);
	}
}