using Risk.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risk.Command
{
    class EditShapeCommand
    {
        private Shape shape;
        private double dx;
        private double dy;
        private double dwidth;
        private double dheight;

        public EditShapeCommand(Shape _shape, double _dx, double _dy, double _dwidth, double _dheight)
        {
            shape = _shape;
            dx = _dx;
            dy = _dy;
            dwidth = _dwidth;
            dheight = _dheight;
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
