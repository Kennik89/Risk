using System.Collections.ObjectModel;
using Model;

namespace Risk.Command
{
    public class AddShapeCommand : IUndoRedoCommand
    {
        
        private ObservableCollection<Shape> _shapes;
        
        private Shape _shape;
        
        public AddShapeCommand(ObservableCollection<Shape> shapes, Shape shape)
        {
            _shapes = shapes;
            _shape = shape;
        }

        // For Undo
        public void Execute()
        {
            _shapes.Add(_shape);
        }
        // For Redo
        public void UnExecute()
        {
            _shapes.Remove(_shape);
        }

    }
}
