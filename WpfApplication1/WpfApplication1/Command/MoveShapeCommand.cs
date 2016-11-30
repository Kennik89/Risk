using Risk.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risk.Command
{
    public class MoveShapeCommand : IUndoRedoCommand
    {

        private Shape shape;

        private double offsetX;
        private double offsetY;

        // For changing the current state of the diagram.
        public MoveShapeCommand(Shape _shape, double _offsetX, double _offsetY)
        {
            shape = _shape;
            offsetX = _offsetX;
            offsetY = _offsetY;
        }

        public void Execute()
        {
            shape.CanvasCenterX += offsetX;
            shape.CanvasCenterY += offsetY;
        }

        public void UnExecute()
        {
            shape.CanvasCenterX -= offsetX;
            shape.CanvasCenterY -= offsetY;
        }
    }
}
