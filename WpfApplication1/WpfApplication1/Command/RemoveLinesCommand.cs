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

        private ObservableCollection<Line> _lines;
        private List<Line> _linesToRemove;
        #endregion

        #region Constructor

        public RemoveLinesCommand(ObservableCollection<Line> lines, List<Line> linesToRemove)
        {
            _lines = lines;
            _linesToRemove = linesToRemove;
        }

        public RemoveLinesCommand(ObservableCollection<Line> lines, Line lineToRemove)
        {
            _lines = lines;
            _linesToRemove = new List<Line>(1) {lineToRemove};
        }

        #endregion

        #region Methods

        // For doing and redoing the command.
        public void Execute()
        {
            _linesToRemove.ForEach(x => _lines.Remove(x));
        }

        // For undoing the command.
        public void UnExecute()
        {
            _linesToRemove.ForEach(x => _lines.Add(x));
        }

        #endregion
    }
}
