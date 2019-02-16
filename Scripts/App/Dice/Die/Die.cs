using System;
using System.Collections.Generic;
using UnityEngine;
using TouchScript.Pointers;
using VectorStudios.Dice.Math;

/// <summary>
/// Created By: Jonathon Wigley - 5/21/18
/// Last Edited By: Jonathon Wigley - 12/29/2018
/// </summary>

/// <summary>
/// States that a die can be in
/// </summary>
public enum DieState
{
    /// <summary>
    /// Die is currently rolling
    /// </summary>
    Rolling,

    /// <summary>
    /// Die is at rest and a side is on the ground
    /// </summary>
    Grounded,

    /// <summary>
    /// Die is at rest but no side is on the ground
    /// </summary>
    Skewed,

    /// <summary>
    /// Die is currently being held by the player
    /// </summary>
    Held,

    /// <summary>
    /// Die was just spawned and is waiting to come to rest
    /// </summary>
    Spawning,

    /// <summary>
    /// Die has been released without being rolled
    /// </summary>
    Dropped
}

/// <summary>
/// Types of rolls that a die can use
/// </summary>
public enum RollType { Roll, Bounce, Nudge }

/// <summary>
/// Handles logic of a die object
/// </summary>
public partial class Die : PooledObject
{
#region Reference Variables
    /// <summary>
    /// Reference to the current game settings object
    /// </summary>
    protected GameSettings gameSettings;

    /// <summary>
    /// Reference to the current dice settings object
    /// </summary>
    protected DiceSettings diceSettings;

    /// <summary>
    /// Reference to the rigidbody for the die
    /// </summary>
    protected Rigidbody body;

    /// <summary>
    /// Reference to the die's mesh collider
    /// </summary>
    protected new MeshCollider collider;

    /// <summary>
    /// Lock UI that is currently being used by this die
    /// </summary>
    protected DieLockUI currentLockUI;
#endregion

#region States
    /// <summary>
    /// Current state of the die
    /// </summary>
    public DieState State { get; protected set; }

    /// <summary>
    /// True if the die is currently rolling
    /// </summary>
    public bool bRolling { get { return State == DieState.Rolling; } }

    /// <summary>
    /// True if the die is at rest but no side is on the ground
    /// </summary>
    public bool bSkewed { get { return State == DieState.Skewed; } }

    /// <summary>
    /// True if the die is at rest and a side is on the ground
    /// </summary>
    public bool bGrounded { get { return State == DieState.Grounded; } }

    /// <summary>
    /// True if the die is at rest, whether its skewed or grounded
    /// </summary>
    public bool bAtRest { get { return State == DieState.Grounded || State == DieState.Skewed; } }

    /// <summary>
    /// True if the die is currently being held by the player
    /// </summary>
    public bool bHeld { get { return State == DieState.Held; } }

    /// <summary>
    /// True if the die is currently falling after being droppped
    /// </summary>
    public bool bDropped { get { return State == DieState.Dropped; } }

    /// <summary>
    /// True if the die is currently in edit mode
    /// </summary>
    public bool bIsEditing { get; protected set; }

    /// <summary>
    /// Marked true after certain rolling situations to tell the die that it needs
    /// to bounce when it hits the ground
    /// </summary>
    protected bool bNeedsToBounce;
#endregion

#region Interactions
    IInteractBehaviour tapBehaviour = new RollBehaviour();
    IInteractBehaviour longPressBehaviour = new NullBehaviour();
    IInteractBehaviour flickedOnGroundBehaviour = new LobBehaviour();
    IInteractBehaviour flickedWhileHeldBehaviour = new ThrowBehaviour();
    IInteractBehaviour swipedBehaviour = new LobBehaviour();
#endregion

    /// <summary>
    /// Type of die this is
    /// </summary>
    public DieType DieType { get; protected set; }

    /// <summary>
    /// Die's current rolled value, returns 0 if the die is rolling
    /// </summary>
    public int RolledValue { get; protected set; }

    /// <summary>
    /// Dispatched when the die starts rolling
    /// </summary>
    public static event StartedRollingDelegate OnStartedRolling;
    public delegate void StartedRollingDelegate(DieData data);

    /// <summary>
    /// Dispatched when the die stops rolling
    /// </summary>
    public static event StoppedRollingDelegate OnStoppedRolling;
    public delegate void StoppedRollingDelegate(DieData data);

    protected void Awake()
    {
        touchable = this;
        body = GetComponent<Rigidbody>();
        collider = GetComponentInChildren<MeshCollider>();
        if(gameObject.layer != LayerMask.NameToLayer("Dice"))
            Debug.LogError("Die's layer is not set to Dice layer", this);

        gameSettings = GameManager.Instance.GameSettings;
        diceSettings = DiceManager.Instance.DiceSettings;

        PopulateListOfSides();
        GetGestureReferences();
        GetRenderingReferences();
        FindITouchableReference();
        FindIKillableReference();
        FindISwipableReference();
        FindCarryHandlerReference();
    }

    protected void Start()
    {
        InitCollisionSettings();
        InitGestureSettings();
        ParseDieTypeFromSideCount();
    }

    protected void OnEnable()
    {
        ScreenTapManager.OnTapped += ScreenTapManager_Tapped;

        RegisterGestureEvents();
        RegisterCarryHandlerEvents();

        ChangeState(DieState.Spawning);
    }

    protected void OnDisable()
    {
        ScreenTapManager.OnTapped -= ScreenTapManager_Tapped;

        UnregisterGestureEvents();
        UnregisterCarryHandlerEvents();
    }

    protected void Update()
    {
        // Die stopped rolling this frame
        if(bAtRest == false && bHeld == false && body.IsSleeping())
        {
            HandleComeToRest();
        }
    }

    /// <summary>
    /// Applies forces to the die to make it roll
    /// </summary>
    /// <param name="sender"></param>
    public void Roll()
    {
        if(bHeld == true)
            return;

        HandleStartRolling();

        tapBehaviour.Interact(gameObject, new InputData());
    }

    /// <summary>
    /// Called when a die is thrown to give it an extra bounce
    /// </summary>
    protected void BounceRoll()
    {
        HandleStartRolling();

        // Add the forces to the die
        Vector3 velocity = body.velocity;
        body.velocity = new Vector3(velocity.x, 0f, velocity.z);
        body.angularVelocity = Vector3.zero;
        body.AddForce(DiceMath.RandomLinearRollForce(RollType.Bounce), ForceMode.Impulse);
        body.AddTorque(DiceMath.RandomAngularRollForce(RollType.Bounce), ForceMode.Impulse);
    }

    /// <summary>
    /// Applies a small nudge force to the die to dislodge it
    /// </summary>
    public void Nudge()
    {
        HandleStartRolling();

        // Add the forces to the die
        body.AddForce(DiceMath.RandomLinearRollForce(RollType.Nudge), ForceMode.Impulse);
        body.AddTorque(DiceMath.RandomAngularRollForce(RollType.Nudge), ForceMode.Impulse);
    }

    /// <summary>
    /// Called every frame when the die is being held
    /// </summary>
    /// <param name="touch"></param>
    protected void UpdateHold(Pointer input)
    {
        // Wake up a body that is held so it doesn't jump around
        if(body.IsSleeping())
            body.WakeUp();
    }

    /// <summary>
    /// Called when the ground has been touched by input
    /// </summary>
    /// <param name="sender">Touch that touched the ground</param>
    protected void ScreenTapManager_Tapped(Vector2 tapPosition)
    {
        Roll();
        bNeedsToBounce = true;
    }

    /// <summary>
    /// Returns true if the player has held the die for longer than the tap time
    /// </summary>
    /// <returns></returns>
    protected bool TouchTimeIsPastTapThreshold()
    {
        if(Time.realtimeSinceStartup - inputStartTime < diceSettings.TapTimeThreshold)
            return false;

        return true;
    }

    /// <summary>
    /// Handles functionality for when the die starts rolling
    /// </summary>
    protected void HandleStartRolling()
    {
        ChangeState(DieState.Rolling);

        OnStartedRolling?.Invoke(new DieData(this));
    }

    /// <summary>
    /// Handles functionality for when the die stops rolling and comes to rest
    /// </summary>
    protected void HandleComeToRest()
    {
        DieState previousState = State;

        // Grounded
        if(IsSideOnGround() == true)
        {
            ChangeState(DieState.Grounded);
            RolledValue = GetSideOnGround().RollValue;
        }

        // Skewed
        else
        {
            ChangeState(DieState.Skewed);
            RolledValue = GetSideClosestToTheGround(false).RollValue;
        }

        // Send out the stop rolling event if this die was rolling before
        if(previousState == DieState.Rolling)
        {
            OnStoppedRolling?.Invoke(new DieData(this));
        }
    }

    /// <summary>
    /// Change the die state
    /// </summary>
    /// <param name="newState">State to change to</param>
    protected void ChangeState(DieState newState)
    {
        if(State == newState)
            return;

        State = newState;
        switch (newState)
        {
            case DieState.Dropped:
            case DieState.Grounded:
            case DieState.Skewed:
            break;

            case DieState.Spawning:
            case DieState.Held:
            case DieState.Rolling:
                AndroidManager.VibrateDevice(10);
                break;
        }
    }

    /// <summary>
    /// Determine this die's type from its side count
    /// </summary>
    protected void ParseDieTypeFromSideCount()
    {
        switch(SideCount)
        {
            case 4:
                DieType = DieType.d4;
            break;

            case 6:
                DieType = DieType.d6;
            break;

            case 8:
                DieType = DieType.d8;
            break;

            case 10:
                DieType = DieType.d10;
            break;

            case 12:
                DieType = DieType.d12;
            break;

            case 20:
                DieType = DieType.d20;
            break;

            default:
                Debug.LogError("Die Type could not be determined for " + gameObject.name + " with " + SideCount + " sides", this);
                DieType = DieType.d4;
            break;
        }
    }

    /// <summary>
    /// Set whether the die is in edit or play mode
    /// </summary>
    /// <param name="val">Edit mode if true, play mode if false</param>
    public void ToggleEditMode(bool val)
    {
        ToggleLockUI(val);

        bIsEditing = val;
    }

    /// <summary>
    /// Show or hide the lock UI
    /// </summary>
    /// <param name="val">Show if true, hide if false</param>
    private void ToggleLockUI(bool val)
    {
        if(val == true)
        {
            currentLockUI = DicePrefabs.Instance.GetLockUIPrefab().GetPooledInstance<DieLockUI>();
            currentLockUI.SetTargetDie(this);
        }
        else if(val == false && currentLockUI != null)
        {
            currentLockUI.ReturnToPool();
            currentLockUI = null;
        }
    }
}