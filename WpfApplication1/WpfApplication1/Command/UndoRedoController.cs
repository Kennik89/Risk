using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risk.Command
{
    public class UndoRedoController
    {
        #region Fields

        private readonly Stack<IUndoRedoCommand> undoStack = new Stack<IUndoRedoCommand>();
        private readonly Stack<IUndoRedoCommand> redoStack = new Stack<IUndoRedoCommand>();

        #endregion

        #region Properties

        public static UndoRedoController Instance { get; } = new UndoRedoController();

        #endregion

        #region Constructor

        private UndoRedoController() { }

        #endregion

        #region Methods
        public void AddAndExecute(IUndoRedoCommand command)
        {
            undoStack.Push(command);
            redoStack.Clear();
            command.Execute();
        }

        public bool CanUndo() => undoStack.Any();

        public void Undo()
        {
            if (!undoStack.Any()) throw new InvalidOperationException();
            var command = undoStack.Pop();
            redoStack.Push(command);
            command.UnExecute();

        }

       public bool CanRedo() => redoStack.Any();


        public void Redo()
        {
            if (!redoStack.Any()) throw new InvalidOperationException();
            var command = redoStack.Pop();
            undoStack.Push(command);
            command.Execute();
        }

        public void Clear()
        {
            undoStack.Clear();
            redoStack.Clear();
        }
        #endregion

    }
}
