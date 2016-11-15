using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risk.Command
{
    public interface IUndoRedoController
    {
        // Regions can be used to make code foldable (minus/plus sign to the left).
        #region Methods

        // For doing and redoing the command.
        void Execute();
        // For undoing the command.
        void UnExecute();

        #endregion
    }
}
