using Risk.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risk.Command
{
    // Undo/Redo command for removing Lines.
    public class RemoveShapeCommand : IUndoRedoCommand
    {
        #region Fields

        private ObservableCollection<Shape> shapes;
        private List<Shape> shapeToRemove;
        #endregion

        #region Constructor

        public RemoveShapeCommand(ObservableCollection<Shape> _shapes, List<Shape> _shapesToRemove)
        {
            shapes = _shapes;
            shapeToRemove = _shapesToRemove;
        }

        #endregion

        #region Methods

        // For doing and redoing the command.
        public void Execute()
        {
            shapeToRemove.ForEach(x => shapes.Remove(x));
        }

        // For undoing the command.
        public void UnExecute()
        {
            shapeToRemove.ForEach(x => shapes.Add(x));
        }

        #endregion
    }
}
