using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Risk.Command
{
    class EditShapeCommand : IUndoRedoCommand
    {
        private Shape _shape;
        private double _dx;
        private double _dy;
        private double _dwidth;
        private double _dheight;

        public EditShapeCommand(Shape shape, double dx, double dy, double dwidth, double dheight)
        {
            _shape = shape;
            _dx = dx;
            _dy = dy;
            _dwidth = dwidth;
            _dheight = dheight;
        }

        public void Execute()
        {
            _shape.X += _dx;
            _shape.Y += _dy;
            _shape.Width += _dwidth;
            _shape.Height += _dheight;
        }

        public void UnExecute()
        {
            _shape.X -= _dx;
            _shape.Y -= _dy;
            _shape.Width -= _dwidth;
            _shape.Height -= _dheight;
        }
    }
}
