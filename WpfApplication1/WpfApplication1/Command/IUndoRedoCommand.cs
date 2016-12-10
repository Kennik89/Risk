using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risk.Command
{
    public interface IUndoRedoCommand
    {
        void Execute();
        void UnExecute();
    }
}
