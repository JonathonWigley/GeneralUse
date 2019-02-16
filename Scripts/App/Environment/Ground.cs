using UnityEngine;
using TouchScript.Pointers;

/// <summary>
/// Created By: Jonathon Wigley - 8/19/2018
/// Last Edited By: Jonathon Wigley - 09/21/2018
///
/// Purpose:
/// Manage different effects that occur when the ground is touched
/// <summary>

public class Ground : MonoBehaviour, ITouchable
{
	bool inputTouching;
	float touchStartTime;
	float tapTimeThreshold = .3f;

    /// <summary>
    /// Dispatched when the ground is touched
    /// </summary>
    /// <param name="touch"></param>
	public delegate void Pressed(Pointer input);
	public static event Pressed OnPressed;

    bool ITouchable.HandleInputPressed(Pointer input)
    {
        inputTouching = true;
        touchStartTime = Time.realtimeSinceStartup;
        return true;
    }

    bool ITouchable.HandleInputReleased(Pointer input)
    {
        if(inputTouching == false || input.Id != input.Id)
            return false;

        if(Time.realtimeSinceStartup - touchStartTime < tapTimeThreshold)
			OnPressed?.Invoke(input);

        inputTouching = false;

        return true;
    }

    bool ITouchable.HandleInputMoved(Pointer input) { return true; }
    bool ITouchable.HandleInputStationary(Pointer input) { return true; }
    bool ITouchable.HandleInputCancelled(Pointer input) { return true; }
}
