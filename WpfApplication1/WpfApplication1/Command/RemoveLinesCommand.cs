using Risk.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//TODO MAKE THIS WORK
//TOOD
//TODO

namespace Risk.Command
{
    // Undo/Redo command for removing Lines.
    public class RemoveLinesCommand : IUndoRedoCommand
    {
        // Regions can be used to make code foldable (minus/plus sign to the left).
        #region Fields

        private ObservableCollection<Line> lines;

        // The 'linesToRemove' field holds a collection of existing lines, that are removed from the 'lines' collection
        private List<Line> linesToRemove;

        #endregion

        #region Constructor

        // For changing the current state of the diagram 
        //  (or at least the relevant parts).
        public RemoveLinesCommand(ObservableCollection<Line> _lines, List<Line> _linesToRemove)
        {
            lines = _lines;
            linesToRemove = _linesToRemove;
        }

        #endregion

        #region Methods

        // For doing and redoing the command.
        public void Execute()
        {
            // This is a lambda expression, that iterates the 'linesToRemove' collection, 
            //  and for each one removes it from the 'lines' collection.
            // Java:
            //  foreach (Line line in linesToRemove)
            //  { 
            //    lines.Remove(line);
            //  }
            linesToRemove.ForEach(x => lines.Remove(x));
        }

        // For undoing the command.
        public void UnExecute()
        {
            // This is a lambda expression, that iterates the 'linesToRemove' collection, 
            //  and for each one adds it to the 'lines' collection.
            // Java:
            //  foreach (Line line in linesToRemove)
            //  { 
            //    lines.Add(line);
            //  }
            linesToRemove.ForEach(x => lines.Add(x));
        }

        #endregion
    }
}
