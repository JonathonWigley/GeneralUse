using UnityEngine;

/// <summary>
/// Created By: Jonathon Wigley - 11/14/2018
/// Last Edited By: Jonathon Wigley - 11/14/2018
/// </summary>

/// <summary>
/// Interact behaviour that does nothing
/// </summary>
public class NullBehaviour : IInteractBehaviour
{
    /// <summary>
    /// Do nothing
    /// </summary>
    public bool Interact(GameObject interactable, InputData args)
    {
        // Do nothing
        return true;
    }
}