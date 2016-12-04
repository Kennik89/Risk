using Risk.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//TODO MAKE THIS WORK

namespace Risk.Command
{
    // Undo/Redo command for removing Lines.
    public class RemoveLinesCommand : IUndoRedoCommand
    {
        #region Fields

        private ObservableCollection<Line> lines;
        private List<Line> linesToRemove;
        #endregion

        #region Constructor

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
            linesToRemove.ForEach(x => lines.Remove(x));
        }

        // For undoing the command.
        public void UnExecute()
        {
            linesToRemove.ForEach(x => lines.Add(x));
        }

        #endregion
    }
}
