using UnityEngine;
using VectorStudios.Dice.Math;

/// <summary>
/// Created By: Jonathon Wigley - 11/14/2018
/// Last Edited By: Jonathon Wigley - 11/14/2018
/// </summary>

/// <summary>
/// Throws the object in a direction with vertical lift
/// </summary>
public class LobBehaviour : IInteractBehaviour
{
    /// <summary>
    /// Minimum multiplier for random angular force generation
    /// </summary>
    private float minAngularForceMultiplier = .075f;

    /// <summary>
    /// Maximum multiplier for random anuglar force generation
    /// </summary>
    private float maxAngularForceMultiplier = .3f;

    /// <summary>
    /// Horizontal force to apply to the lob
    /// </summary>
    private float horizontalLobForce;

    /// <summary>
    /// Vertical force to apply to the lob
    /// </summary>
    private float verticalLobForce;

    /// <summary>
    /// Multiplier to apply to the spin angular force
    /// </summary>
    private float angularForceMultiplier;

    /// <summary>
    /// Lob the die in the input direction
    /// </summary>
    /// <param name="obj">Gameobject being interacted with</param>
    /// <param name="args">Data about the input causing this interaction</param>
    /// <returns></returns>
    public bool Interact(GameObject obj, InputData args)
    {
        // Find the rigidbody on the interactable
        Rigidbody body = obj.GetComponentInChildren<Rigidbody>();
        if(body == null)
        {
            Debug.LogError("Trying to call roll behaviour on object without rigidbody");
            return false;
        }

        // Get settings from the game manager
        GameSettings gameSettings = GameManager.Instance.GameSettings;
        horizontalLobForce = gameSettings.HorizontalFlickForce;
        verticalLobForce = gameSettings.VerticalFlickForce;
        angularForceMultiplier = gameSettings.AngularThrowForceMultiplier;

        // Get the object's current horizontal velocity
        Vector3 horizontalVelocity = DiceMath.GetHorizontalVelocity(body);

        // Zero out the body's velocity so we have full control over it
        body.velocity = Vector3.zero;

        // Get the target velocity based on the direction direction
        Vector2 lobVector = args.Direction.normalized;
        Vector3 targetHorizontalVelocity = new Vector3(lobVector.x, 0f, lobVector.y) * horizontalLobForce;
        Vector3 targetVelocity = targetHorizontalVelocity + new Vector3(0f, verticalLobForce, 0f);

        // Add the target velocity to the object
        body.AddForce(targetVelocity, ForceMode.Impulse);

        Vector3 torque = DiceMath.AngularDirectionalForce(obj.transform, targetHorizontalVelocity * angularForceMultiplier);
        // Debug.Log("Torque: " + torque);
        // Add spin that correlates to the direction the object is being lobbed
        body.AddTorque(DiceMath.AngularDirectionalForce(obj.transform, targetHorizontalVelocity * angularForceMultiplier), ForceMode.Impulse);

        return true;
    }
}