using System.Collections.Generic;
using UnityEngine;

public class CommandManager
{
    private static CommandManager _instance;
    public static CommandManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new CommandManager();
            }
            return _instance;
        }
    }

    private Stack<ICommand> undoStack = new Stack<ICommand>();
    private Stack<ICommand> redoStack = new Stack<ICommand>();

    public void ExecuteCommand(ICommand command)
    {
        Debug.Log("Executing command...");
        command.Execute();
        undoStack.Push(command);
        redoStack.Clear();
        Debug.Log("Command executed. Undo stack count: " + undoStack.Count);
    }

    public void UndoAction()
    {
        if (undoStack.Count > 0)
        {
            ICommand command = undoStack.Pop();
            command.Undo();
            redoStack.Push(command);
        }
        Debug.Log("UndoAction called. Stack count: " + undoStack.Count);
    }

    public void RedoAction()
    {
        Debug.Log("RedoAction called. Stack count: " + redoStack.Count);
        if (redoStack.Count > 0)
        {
            ICommand command = redoStack.Pop();
            command.Execute();
            undoStack.Push(command);
        }
    }
}
