using UnityEngine;

/// <summary>
/// Created By: Jonathon Wigley - 11/11/2018
/// Last Edited By: Jonathon Wigley - 11/11/2018
/// </summary>

/// <summary>
/// Interface to be used by different interact behaviours that can be used
/// across objects for certain interaction types
/// </summary>
public interface IInteractBehaviour
{
    /// <summary>
    /// Call the interact behaviour's functionality
    /// </summary>
    bool Interact(GameObject interactable, InputData args);
}