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

        private readonly Stack<IUndoRedoCommand> _undoStack = new Stack<IUndoRedoCommand>();
        private readonly Stack<IUndoRedoCommand> _redoStack = new Stack<IUndoRedoCommand>();

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
            _undoStack.Push(command);
            _redoStack.Clear();
            command.Execute();
        }

        public bool CanUndo() => _undoStack.Any();

        public void Undo()
        {
            if (!_undoStack.Any()) throw new InvalidOperationException();
            var command = _undoStack.Pop();
            _redoStack.Push(command);
            command.UnExecute();

        }

        public bool CanRedo() => _redoStack.Any();

        public void Redo()
        {
            if (!_redoStack.Any()) throw new InvalidOperationException();
            var command = _redoStack.Pop();
            _undoStack.Push(command);
            command.Execute();
        }

        public void Clear()
        {
            _undoStack.Clear();
            _redoStack.Clear();
        }
        #endregion
    }
}
