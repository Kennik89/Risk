using Risk.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risk.Command
{
    // Undo/Redo command for removing Shapes.
    public class RemoveShapesCommand : IUndoRedoCommand
    {
        #region Fields

        private ObservableCollection<Shape> _shapes;
        private ObservableCollection<Line> _lines;
        private List<Shape> _shapesToRemove;
        private List<Line> _linesToRemove;
        #endregion

        #region Constructor

        public RemoveShapesCommand(ObservableCollection<Shape> shapes, ObservableCollection<Line> lines, List<Shape> shapesToRemove)
        {
            _shapes = shapes;
            _lines = lines;
            _shapesToRemove = shapesToRemove;

            _linesToRemove = _lines.Where(x => _shapesToRemove.Any(y => y.UID == x.From.UID || y.UID == x.To.UID)).ToList();
        }

        public RemoveShapesCommand(ObservableCollection<Shape> shapes, ObservableCollection<Line> lines, Shape shapeToRemove)
        {
            _shapes = shapes;
            _lines = lines;
            _shapesToRemove = new List<Shape>(1) {shapeToRemove};

            _linesToRemove = _lines.Where(x => _shapesToRemove.Any(y => y.UID == x.From.UID || y.UID == x.To.UID)).ToList();
        }

        #endregion

        #region Methods

        // For doing and redoing the command.
        public void Execute()
        {
            _linesToRemove.ForEach(x => _lines.Remove(x));
            _shapesToRemove.ForEach(x => _shapes.Remove(x));
        }

        // For undoing the command.
        public void UnExecute()
        {
            _shapesToRemove.ForEach(x => _shapes.Add(x));
            _linesToRemove.ForEach(x => _lines.Add(x));
        }

        #endregion
    }
}
