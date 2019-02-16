using UnityEngine;

/// <summary>
/// Created By: Jonathon Wigley - 12/29/2018
/// Last Edited By: Jonathon Wigley - 12/29/2018
/// </summary>

/// <summary>
/// Interface that should be implemented by any object that should be able to be swiped
/// </summary>
public interface ISwipable
{
    void Swipe(Vector3 swipeDirection);
}