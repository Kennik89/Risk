using System.Collections.Generic;
using System.Collections.ObjectModel;
using Model;

namespace Risk.Command
{
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

        public void Execute()
        {
            _linesToRemove.ForEach(x => _lines.Remove(x));
        }

        public void UnExecute()
        {
            _linesToRemove.ForEach(x => _lines.Add(x));
        }

        #endregion
    }
}
