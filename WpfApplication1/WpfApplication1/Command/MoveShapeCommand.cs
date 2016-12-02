using Risk.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risk.Command
{
    class MoveShapeCommand : IUndoRedoCommand
    {
        private Shape shape;
        private double dx;
        private double dy;

        public MoveShapeCommand(Shape _shape, double _dx, double _dy)
        {
            shape = _shape;
            dx = _dx;
            dy = _dy;
        }

        public void Execute()
        {
            shape.X += dx;
            shape.Y += dy;
        }

        public void UnExecute()
        {
            shape.X -= dx;
            shape.Y -= dy;
        }

    }
}
