using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Risk.Command
{
    class MoveShapeCommand : IUndoRedoCommand
    {
        private Shape _shape;
        private double _dx;
        private double _dy;

        public MoveShapeCommand(Shape shape, double dx, double dy)
        {
            _shape = shape;
            _dx = dx;
            _dy = dy;
        }

        public void Execute()
        {
            _shape.X += _dx;
            _shape.Y += _dy;
        }

        public void UnExecute()
        {
            _shape.X -= _dx;
            _shape.Y -= _dy;
        }

    }
}
