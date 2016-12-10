using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Risk.Command
{
    public class PasteShapeCommand : IUndoRedoCommand
    {

        private ObservableCollection<Shape> _shapes;
        private ObservableCollection<Line> _lines;
        private Shape _oldShape;
        private Shape _newShape;
        private List<Shape> _connectedShapes;
        private List<Line> _oldLines;
        private List<Line> _newLines;

        public PasteShapeCommand(ObservableCollection<Shape> shapes, ObservableCollection<Line> lines, Shape oldShape, Shape newShape)
        {
            _shapes = shapes;
            _lines = lines;
            _oldShape = oldShape;
            _newShape = newShape;
            CollectConnectedShapes();
            _newLines = new List<Line>();
        }

        private void CollectConnectedShapes()
        {
            _connectedShapes = new List<Shape>();
            _oldLines = new List<Line>();
            _oldLines = _lines.Where(x => _oldShape.Uid == x.From.Uid || _oldShape.Uid == x.To.Uid).ToList();

            foreach (var l in _oldLines)
            {
                if (l.To == _oldShape)
                {
                    _connectedShapes.Add(l.From);
                }
                else if (l.From == _oldShape)
                {
                    _connectedShapes.Add(l.To);
                }
            }
        }

        public void Execute()
        {
            _shapes.Add(_newShape);
            foreach (Shape s in _connectedShapes)
            {
                _newLines.Add(new Line() { From = _newShape, To = s });
            }
            _newLines.ForEach(x => _lines.Add(x));
        }
        public void UnExecute()
        {
            _newLines.ForEach(x => _lines.Remove(x));
            _shapes.Remove(_newShape);
        }

    }
}
