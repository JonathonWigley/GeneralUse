using UnityEngine;
using System;

/// <summary>
/// Created By: Jonathon Wigley - 12/11/2018
/// Last Edited By: Jonathon Wigley - 12/11/2018
/// </summary>

/// <summary>
/// Component to attach to an object that should be carryable
/// </summary>
[RequireComponent(typeof(DragGesture))]
public class CarryHandler : MonoBehaviour
{
    /// <summary>
    /// Drag gesture attached to this object
    /// </summary>
    private DragGesture dragGesture;

    /// <summary>
    /// Rigidbody attached to this object
    /// </summary>
    private Rigidbody body;

    /// <summary>
    /// Prefab of the rigidbody dragger used for this object
    /// </summary>
    public RigidbodyDragger DraggerPrefab
    {
        get { return draggerPrefab; }
        set
        {
            if(value != null)
                draggerPrefab = value;
        }
    }
    [SerializeField] private RigidbodyDragger draggerPrefab;

    /// <summary>
    /// Current rigidbody dragger being used to drag this object
    /// </summary>
    private RigidbodyDragger currentDragger;

    /// <summary>
    /// Height above the touch plane that the object should be carried at
    /// </summary>
    public float CarryHeight
    {
        get { return carryHeight; }
        set
        {
            carryHeight = Mathf.Clamp(value, 0f, float.PositiveInfinity);
        }
    }
    [SerializeField] private float carryHeight = 2f;

    /// <summary>
    /// Dispatched when the object is picked up
    /// </summary>
    public Action OnPickUp;

    /// <summary>
    /// Dispatched every frame in Update that the object is being carried
    /// </summary>
    public Action OnUpdateCarry;

    /// <summary>
    /// Dispatched when the object is dropped
    /// </summary>
    public Action OnDrop;

    private void Awake()
    {
        dragGesture = GetComponent<DragGesture>();
        body = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        dragGesture.OnPressed += DragGesture_Pressed;
        dragGesture.OnDragged += DragGesture_Dragged;
        dragGesture.OnReleased += DragGesture_Released;
    }

    private void OnDisable()
    {
        dragGesture.OnPressed -= DragGesture_Pressed;
        dragGesture.OnDragged -= DragGesture_Dragged;
        dragGesture.OnReleased -= DragGesture_Released;
    }

    /// <summary>
    /// Called when the drag gesture is first pressed
    /// </summary>
    /// <param name="gesture"></param>
    private void DragGesture_Pressed(DragGesture gesture)
    {
        if(currentDragger == null)
        {
            currentDragger = DraggerPrefab.GetPooledInstance<RigidbodyDragger>().Init(body);
            OnPickUp?.Invoke();
        }

        Vector3 targetPosition = gesture.Position + new Vector3(0f, CarryHeight, 0f);
        currentDragger.DragWithWorldPoint(targetPosition);
    }

    /// <summary>
    /// Called when the drag gesture is being moved/dragged
    /// </summary>
    /// <param name="gesture"></param>
    private void DragGesture_Dragged(DragGesture gesture)
    {
        if(currentDragger == null)
        {
            currentDragger = DraggerPrefab.GetPooledInstance<RigidbodyDragger>().Init(body);
            OnPickUp?.Invoke();
        }

        Vector3 targetPosition = gesture.Position + new Vector3(0f, CarryHeight, 0f);
        currentDragger.DragWithWorldPoint(targetPosition);

        OnUpdateCarry?.Invoke();
    }

    /// <summary>
    /// Called when the drag gesture is released
    /// </summary>
    /// <param name="gesture"></param>
    private void DragGesture_Released(DragGesture gesture)
    {
        // Disconnect the dragger if it exists
        if(currentDragger)
        {
            currentDragger.Disconnect();
            currentDragger.ReturnToPool();
            currentDragger = null;

            OnDrop?.Invoke();
        }
    }
}