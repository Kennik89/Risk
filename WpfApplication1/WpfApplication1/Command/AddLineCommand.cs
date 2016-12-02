
using Risk.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risk.Command
{
    public class AddLineCommand : IUndoRedoCommand
    {
        #region Fields


        private System.Collections.ObjectModel.ObservableCollection<Line> lines;
        private Line line;

        #endregion

        #region Constructor

        // For changing the current state of the diagram.
        public AddLineCommand(System.Collections.ObjectModel.ObservableCollection<Line> _lines, Line _line)
        {
            lines = _lines;
            line = _line;
        }

        #endregion

        #region Methods

        // For doing and redoing the command.
        public void Execute()
        {
            lines.Add(line);
        }

        // For undoing the command.
        public void UnExecute()
        {
            lines.Remove(line);
        }

        #endregion
    }
}
