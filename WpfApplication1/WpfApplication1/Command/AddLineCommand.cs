using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Risk.Command
{
    public class AddLineCommand : IUndoRedoCommand
    {
        #region Fields

        private System.Collections.ObjectModel.ObservableCollection<Line> _lines;
        private Line _line;

        #endregion

        #region Constructor

        public AddLineCommand(System.Collections.ObjectModel.ObservableCollection<Line> lines, Line line)
        {
            _lines = lines;
            _line = line;
        }
        #endregion

        #region Methods

        // For Undo.
        public void Execute()
        {
            _lines.Add(_line);
        }

        // For Redo.
        public void UnExecute()
        {
            _lines.Remove(_line);
        }

        #endregion
    }
}
