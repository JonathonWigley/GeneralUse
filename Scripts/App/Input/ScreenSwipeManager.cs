using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Created By: Jonathon Wigley - 12/17/2018
/// Last Edited By: Jonathon Wigley - 12/29/2018
/// </summary>

/// <summary>
/// Manages the swipe handlers for spawned dice
/// </summary>
public class ScreenSwipeManager : MonoBehaviour
{
	/// <summary>
	/// Drag gesture that is used for the swipe
	/// </summary>
	[SerializeField] private DragGesture screenDragGesture;

    /// <summary>
    /// Max amount of time the swipe can be on the screen before its not registered
    /// </summary>
    [SerializeField] private float maxTimeToRegister = 1.5f;

	/// <summary>
	/// Layermask containing the layers that can be swiped
	/// </summary>
	[SerializeField] LayerMask swipableLayers;

    /// <summary>
    /// True if the swipe gesture is currently active
    /// </summary>
    private bool bSwipeActive;

    /// <summary>
    /// List of dice that the swipe gesture passed over
    /// </summary>
    private List<ISwipable> swipedObjects = new List<ISwipable>();

    /// <summary>
    /// List of directions that correspond to the direction of each swiped object
    /// </summary>
    private List<Vector2> swipeDirections = new List<Vector2>();

    /// <summary>
    /// List that contains all the recorded position of the swipe
    /// </summary>
    private List<Vector2> swipeScreenPositions = new List<Vector2>();

	private void OnEnable()
	{
		RegisterGestureEvents();
	}

	private void OnDisable()
	{
		UnregisterGestureEvents();
	}

    /// <summary>
    /// Register event handlers to the drag gesture
    /// </summary>
    private void RegisterGestureEvents()
    {
        screenDragGesture.OnPressed += DragGesture_Pressed;
        screenDragGesture.OnDragged += DragGesture_Dragged;
        screenDragGesture.OnReleased += DragGesture_Released;
    }

    /// <summary>
    /// Unregister event handlers from the drag gesture
    /// </summary>
    private void UnregisterGestureEvents()
    {
        screenDragGesture.OnPressed -= DragGesture_Pressed;
        screenDragGesture.OnDragged -= DragGesture_Dragged;
        screenDragGesture.OnReleased -= DragGesture_Released;
    }

    /// <summary>
    /// Called when the drag gesture is first pressed
    /// </summary>
    /// <param name="gesture"></param>
    private void DragGesture_Pressed(DragGesture gesture)
    {
        bSwipeActive = true;
    }

    /// <summary>
    /// Called when the drag gesture is being moved/dragged
    /// </summary>
    /// <param name="gesture"></param>
    private void DragGesture_Dragged(DragGesture gesture)
    {
        // Return if the swipe gesture is not active
        if(bSwipeActive == false)
            return;

        // Add the recorded position to the list of positions
        swipeScreenPositions.Add(gesture.ScreenPosition);

        // Check to see if the time limit has passed
        if(gesture.Duration > maxTimeToRegister)
        {
            swipedObjects.Clear();
            bSwipeActive = false;
            return;
        }
		// Spherecast at each point and detect any collisions
		RaycastHit[] hits = RaycastUtility.GetWorldHitsSphere(gesture.ScreenPosition, swipableLayers, .75f);
		for (int j = 0; j < hits.Length; j++)
		{
			if(hits[j].transform == null)
				continue;

			// If the hit object is swipable, swipe it
			ISwipable swipableObject = hits[j].transform.GetComponent<ISwipable>();
			if(swipableObject != null)
            {
				if(swipedObjects.AddDistinct(swipableObject))
                {
                    Vector2 swipeDirection;
                    if(swipeScreenPositions.Count >= 2)
                    {
                        swipeDirection = swipeScreenPositions[swipeScreenPositions.Count - 2].
                                        DirectionTo(swipeScreenPositions[swipeScreenPositions.Count - 1]);
                    }

                    else
                    {
                        swipeDirection = screenDragGesture.StartPosition.DirectionTo(swipeScreenPositions[swipeScreenPositions.Count - 1]);
                    }

                    swipeDirections.Add(swipeDirection);
                }
            }
		}
    }

    /// <summary>
    /// Called when the drag gesture is released
    /// </summary>
    /// <param name="gesture"></param>
    private void DragGesture_Released(DragGesture gesture)
    {
        // If the swipe was released while its still active, handle objects that were swiped
        if(bSwipeActive == true)
        {
            bSwipeActive = false;

            CheckForMissedObjectsInSwipePath();
            HandleSwipedObjects();
        }

        swipedObjects.Clear();
        swipeDirections.Clear();
        swipeScreenPositions.Clear();
    }

    /// <summary>
    /// Set a new swipe gesture for the die
    /// </summary>
    public void SetDragGesture(DragGesture newGesture)
    {
        if(screenDragGesture != null)
            UnregisterGestureEvents();

        screenDragGesture = newGesture;
        RegisterGestureEvents();
    }

    /// <summary>
    /// Remove any points from list of recorded positions that don't matter
    /// </summary>
    private void RemoveUnecessaryVectorsFromCurve()
    {
        if(swipeScreenPositions.Count < 3)
            return;

        float acceptableNormalizedDistance = .03f;

        int index1 = 0;
        int index2 = 1;

        Vector2 normalizedScreenPoint1;
        Vector2 normalizedScreenPoint2;

		// Counter used as a failsafe to break out of the while loop (shouldn't need to be used)
		int failsafe = 0;

		// Run through points and remove in real time if 2 are too close together
        while(index2 < swipeScreenPositions.Count)
        {
			// Failsafe counter just in case
			failsafe++;
			if(failsafe > 100)
				break;

            normalizedScreenPoint1 = Camera.main.ScreenToViewportPoint(swipeScreenPositions[index1]);
            normalizedScreenPoint2 = Camera.main.ScreenToViewportPoint(swipeScreenPositions[index2]);
            bool nextPointIsTooClose = normalizedScreenPoint1.DistanceTo(normalizedScreenPoint2) < acceptableNormalizedDistance;

            if(nextPointIsTooClose)
            {
                swipeScreenPositions.RemoveAt(index2);
            }
            else
            {
                index1++;
                index2 = index1 + 1;
            }
        }
    }

	/// <summary>
	/// Add points in between points that are too far apart to make sure there are no empty spaces
	/// In the curve used for checking for objects
	/// </summary>
	private bool AddPointsInBetween()
	{
		float minNormalizedDistanceToSplit = .03f;
        Vector2 normalizedScreenPoint1;
        Vector2 normalizedScreenPoint2;

		List<int> indicesToAdd =  new List<int>(); // Indices to insert corresponding points at
		List<Vector2> pointsToAdd = new List<Vector2>(); // Point to add at the corresponding index

		for (int i = 0; i < swipeScreenPositions.Count - 1; i++)
		{
			normalizedScreenPoint1 = Camera.main.ScreenToViewportPoint(swipeScreenPositions[i]);
			normalizedScreenPoint2 = Camera.main.ScreenToViewportPoint(swipeScreenPositions[i + 1]);

			// If points are too far apart, add a point in between them
			if(normalizedScreenPoint1.DistanceTo(normalizedScreenPoint2) >= minNormalizedDistanceToSplit)
			{
				float distance = swipeScreenPositions[i].DistanceTo(swipeScreenPositions[i + 1]);
				Vector2 direction = swipeScreenPositions[i].DirectionTo(swipeScreenPositions[i + 1]);
				indicesToAdd.Add(i + 1 + indicesToAdd.Count);
				pointsToAdd.Add(swipeScreenPositions[i] + direction * (distance / 2f));
			}
		}

		// If no points were added return false
		if(indicesToAdd.Count == 0)
			return false;

		// Add new points to the curve and return true
		for (int i = 0; i < indicesToAdd.Count; i++)
			swipeScreenPositions.Insert(indicesToAdd[i], pointsToAdd[i]);

		return true;
	}

    /// <summary>
    /// Raycasts along a linear path from the start of the swipe to the end of the swipe
    /// in order to detect any missed objects
    /// </summary>
    private void CheckForMissedObjectsInSwipePath()
    {
		// Remove any vectors that are too close from the curve
        RemoveUnecessaryVectorsFromCurve();

		// Failsafe counter to break out of the while loop
		int failsafe = 0;
		while(AddPointsInBetween())
		{
			// Increment the failsafe counter
			failsafe++;
			if(failsafe >= 10)
				break;
		}

        // Raycast at all of the points set in the swipe curve
        for (int i = 0; i < swipeScreenPositions.Count; i++)
        {
			// Spherecast at each point and detect any collisions
            RaycastHit[] hits = RaycastUtility.GetWorldHitsSphere(swipeScreenPositions[i], swipableLayers, .5f);
			for (int j = 0; j < hits.Length; j++)
			{
				if(hits[j].transform == null)
					continue;

				// If the hit object is swipable, swipe it
				ISwipable swipableObject = hits[j].transform.GetComponent<ISwipable>();
				if(swipableObject != null)
                {
                    if(swipedObjects.AddDistinct(swipableObject))
                    {
                        Vector2 swipeDirection;
                        if(swipeScreenPositions.Count >= 2)
                        {
                            swipeDirection = swipeScreenPositions[swipeScreenPositions.Count - 2].
                                            DirectionTo(swipeScreenPositions[swipeScreenPositions.Count - 1]);
                        }

                        else
                        {
                            swipeDirection = screenDragGesture.StartPosition.DirectionTo(swipeScreenPositions[swipeScreenPositions.Count - 1]);
                        }

                        swipeDirections.Add(swipeDirection);
                    }
                }
			}
        }
    }

    /// <summary>
    /// Handle the action on the objects that were swiped
    /// </summary>
    /// <param name="swipeDirection">Direction of the swipe</param>
    private void HandleSwipedObjects()
    {
        for (int i = 0; i < swipedObjects.Count; i++)
        {
            swipedObjects[i].Swipe(swipeDirections[i]);
        }
    }
}
