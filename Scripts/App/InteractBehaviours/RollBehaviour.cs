using UnityEngine;
using VectorStudios.Dice.Math;

/// <summary>
/// Rolls an object like a die
/// </summary>
public class RollBehaviour : IInteractBehaviour
{
    /// <summary>
    /// Minimum multiplier for random angular force generation
    /// </summary>
    private float minAngularForceMultiplier = .075f;

    /// <summary>
    /// Maximum multiplier for random anuglar force generation
    /// </summary>
    private float maxAngularForceMultiplier = .3f;

    public bool Interact(GameObject obj, InputData args)
    {
        GameSettings gameSettings = GameManager.Instance.GameSettings;

        Rigidbody body = obj.GetComponent<Rigidbody>();
        if(body == null)
        {
            Debug.LogError("Trying to call roll behaviour on object without rigidbody");
            return false;
        }

        gameSettings = GameManager.Instance.GameSettings;
        if(gameSettings == null)
        {
            Debug.LogError("Could not find game settings");
            return false;
        }

        body.AddForce(DiceMath.RandomLinearRollForce(RollType.Roll), ForceMode.Impulse);
        body.AddTorque(DiceMath.RandomAngularRollForce(RollType.Roll), ForceMode.Impulse);

        return true;
    }

}