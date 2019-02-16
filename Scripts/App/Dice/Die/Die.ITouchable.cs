using UnityEngine;
using TouchScript.Pointers;

/// <summary>
/// Created By: Jonathon Wigley - 10/12/2018
/// Last Edited By: Jonathon Wigley - 10/12/2018
/// </summary>

/// <summary>
/// Partial class of Die
/// Handles implementation of the Touchable interface
/// </summary>
public partial class Die : PooledObject, ITouchable
{
    /// <summary>
    /// Reference to the ITouchable implementation of this class
    /// </summary>
    ITouchable touchable;

    /// <summary>
    /// True if a touch is currently interacting with the die
    /// </summary>
    private bool bInputIsTouching;

    /// <summary>
    /// Touch pointer that is currently interacting with this die
    /// </summary>
    Pointer currentPointer;

    /// <summary>
    /// Time at which the currentPointer started touching this die
    /// </summary>
    float inputStartTime;

    /// <summary>
    /// Initialize references dealing with the touchable interface
    /// </summary>
    private void FindITouchableReference()
    {
        touchable = GetComponent<ITouchable>();
    }

    /// <summary>
    /// Called when this die is touched
    /// </summary>
    /// <param name="touch"></param>
    bool ITouchable.HandleInputPressed(Pointer input)
    {
        // Only one touch per die allowed
        if(bInputIsTouching == true)
            return false;
        bInputIsTouching = true;

        inputStartTime = Time.realtimeSinceStartup;
        currentPointer = input;

        bNeedsToBounce = false;

        return true;
    }

    /// <summary>
    /// Called when any touch has moved
    /// </summary>
    /// <param name="touch"></param>
    bool ITouchable.HandleInputMoved(Pointer input)
    {
        // Ignore input if this die isn't being held or the input isn't this die's pointer
        // if(bInputIsTouching == false || input.Id != currentPointer.Id)
        //     return false;

        // if(bHeld == false)
        // {
        //     // Raycast to see if our touch is still hitting this die
        //     RaycastHit hit = RaycastUtility.GetWorldHit(input.Position, LayerMask.GetMask("Dice"));

        //     bool isTouchingThisDie = hit.transform != null &&
        //                             (hit.transform.GetComponent<Die>() == this ||
        //                              hit.transform.GetComponentInParent<Die>() == this ||
        //                              hit.transform.GetComponentInChildren<Die>() == this);

        //     // If we move off of the die begin holding it
        //     if(isTouchingThisDie == false)
        //         ChangeState(DieState.Held);
        // }

        // // Do hold functionality if the die is being held
        // if(bHeld == true)
        // {
        //     UpdateHold(input);
        // }

        return true;
    }

    /// <summary>
    /// Called when any touch is pressed but hasn't moved
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    bool ITouchable.HandleInputStationary(Pointer input)
    {
        return touchable.HandleInputMoved(input);
    }

    /// <summary>
    /// Called when any touch has been released
    /// </summary>
    /// <param name="touch"></param>
    bool ITouchable.HandleInputReleased(Pointer input)
    {
        // Ignore the release if this die isn't being touched or if the input
        // doesn't match this die's pointerID. If the die was flicked, input touching
        // is set to false so this function returns.
        if(bInputIsTouching == false || (input.Id != currentPointer.Id && input.Id != -1))
            return false;

        // Set the die input states to false
        bInputIsTouching = false;

        // Wake up the rigid body in case it is still/asleep
        body.WakeUp();

        // Update our die state to Rolling
        ChangeState(DieState.Dropped);

        return true;
    }

    /// <summary>
    /// Called when any touch has been cancelled
    /// </summary>
    /// <param name="touch"></param>
    bool ITouchable.HandleInputCancelled(Pointer input)
    {
        return touchable.HandleInputReleased(input);
    }
}
