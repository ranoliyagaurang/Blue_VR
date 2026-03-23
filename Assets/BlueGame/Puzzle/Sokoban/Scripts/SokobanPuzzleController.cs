using Commands;
using UnityEngine;
using UnityEngine.InputSystem;

public class SokobanPuzzleController : MonoBehaviour
{
    public void OnInputUndo(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        CommandHistoryHandler.Instance.Undo();
    }
    
    public void OnInputRedo(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        CommandHistoryHandler.Instance.Redo();
    }

    public void OnUndoClick()
    {
        CommandHistoryHandler.Instance.Undo();
    }

    public void OnRedoClick()
    {
        CommandHistoryHandler.Instance.Redo();
    }
}