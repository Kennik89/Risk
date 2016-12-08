using Risk.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risk.Command
{
    public class PasteShapeCommand : IUndoRedoCommand
    {

        private ObservableCollection<Shape> _shapes;
        private ObservableCollection<Line> _lines;
        private List<Shape> _shapesToAdd;
        private List<Line> _linesToAdd;

        public PasteShapeCommand(ObservableCollection<Shape> shapes, List<Shape> shape)
        {
            _shapes = shapes;
            _shapesToAdd = shape;
        }

        public PasteShapeCommand(ObservableCollection<Shape> shapes, Shape shape)
        {
            _shapes = shapes;
            _shapesToAdd = new List<Shape> { shape };
        }

        private void collectLines(List<Shape> shapes)
        {
        }

        public void Execute()
        {
            foreach (Shape shape in _shapesToAdd)
            {

                _linesToAdd.ForEach(x => _lines.Remove(x));
                _shapes.Remove(shape);
            }
        }
        public void UnExecute()
        {
            foreach (Shape shape in _shapesToAdd)
            {
                _linesToAdd.ForEach(x => _lines.Add(x));
                _shapes.Add(shape);
            }
        }

    }
}
