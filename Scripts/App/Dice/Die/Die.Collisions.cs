using UnityEngine;

/// <summary>
/// Created By: Jonathon Wigley - 09/08/2018
/// Last Edited By: Jonathon Wigley - 10/12/2018
///
/// Purpose:
/// Handles functionality of die collisions
/// <summary>

public partial class Die : PooledObject
{
    /// <summary>
    /// Ground collision layer index
    /// </summary>
    private int GroundLayer;

    /// <summary>
    /// Dice collision layer index
    /// </summary>
    private int DiceLayer;

    /// <summary>
    /// Dispatched when the die touches the ground
    /// </summary>
    /// <param name="collidedDie">Die that hit the ground</param>
    public static event CollideWithGroundDelegate OnCollideWithGround;
    public delegate void CollideWithGroundDelegate(Die collidedDie, Vector3 hitPoint);

    /// <summary>
    /// Dispatched when the die collides with another die
    /// </summary>
    /// <param name="die">Die dispatching the event</param>
    /// <param name="otherDie">Die the dispatching die hit</param>
    public static event CollideWithOtherDieDelegate OnCollideWithOtherDie;
    public delegate void CollideWithOtherDieDelegate(Die die, Die otherDie);

    /// <summary>
    /// Dispatched when the die hits any object
    /// </summary>
    /// <param name="collidedDie">Die dispatching the event</param>
    /// <param name="hitObject">Object the dispatching die hit</param>
    public static event HitObjectDelegate OnHitObject;
    public delegate void HitObjectDelegate(Die collidedDie, GameObject hitObject);

    /// <summary>
    /// Initialize settings dealing with collision detection
    /// </summary>
    private void InitCollisionSettings()
    {
        GroundLayer = LayerMask.NameToLayer("Ground");
        DiceLayer = LayerMask.NameToLayer("Dice");
    }

    /// <summary>
    /// Unity message called when the die collides with something
    /// </summary>
    /// <param name="other">Collision information</param>
    private void OnCollisionEnter(Collision other)
    {
        // Die hit anything
        HandleCollision(other);

        // Die hit the ground
        if(other.gameObject.layer == GroundLayer)
			HandleGroundLayerCollision(other);

        // Die hit another die
        else if(other.gameObject.layer == DiceLayer)
            HandleDiceLayerCollision(other);
    }

    /// <summary>
    /// Handles a generic collision with another gameobject
    /// </summary>
    /// <param name="other">Collision data</param>
    private void HandleCollision(Collision other)
    {
		OnHitObject?.Invoke(this, other.gameObject);
    }

    /// <summary>
    /// Handles a collision with the ground layer
    /// </summary>
    /// <param name="other">Collision data</param>
    private void HandleGroundLayerCollision(Collision other)
    {
        if(bNeedsToBounce == true)
        {
            BounceRoll();
            bNeedsToBounce = false;
        }

        OnCollideWithGround?.Invoke(this, other.contacts[0].point);
    }

    /// <summary>
    /// Handles collision with other dice
    /// </summary>
    /// <param name="other">Collision data</param>
    private void HandleDiceLayerCollision(Collision other)
    {
        Die otherDie = other.gameObject.GetComponent<Die>();
        if(otherDie == null)
        {
            Debug.LogError("Dice layer object does not have a Die script attached", otherDie.gameObject);
            return;
        }

        // Don't bounce if dice collide while rolling
        if(bNeedsToBounce == true && otherDie.bRolling == false)
        {
            BounceRoll();
            bNeedsToBounce = false;
        }

        OnCollideWithOtherDie?.Invoke(this, otherDie);
    }
}
