using Risk.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risk.Command
{
    public class AddShapeCommand : IUndoRedoCommand
    {
        
        private ObservableCollection<Shape> shapes;
        
        private Shape shape;
        
        public AddShapeCommand(ObservableCollection<Shape> _shapes, Shape _shape)
        {
            shapes = _shapes;
            shape = _shape;
        }

        public void Execute()
        {
            shapes.Add(shape);
        }
        public void UnExecute()
        {
            shapes.Remove(shape);
        }

    }
}
