using UnityEngine;
using DG.Tweening;
using EasyButtons;
using System;

/// <summary>
/// Created By: Jonathon Wigley - 10/09/2018
/// Last Edited By: Jonathon Wigley - 10/21/2018
/// </summary>

/// <summary>
/// View that has content that gets collapsed into a mask
/// </summary>
public class CollapsableView : MonoBehaviour
{
	/// <summary>
	/// Action that a toggleable object can do
	/// </summary>
	public enum ToggleAction { Open, Close, Nothing }

	/// <summary>
	/// Action that the collapsable view can take when the game starts
	/// </summary>
	[SerializeField] private ToggleAction onStartAction = ToggleAction.Close;

	[Header("References")]
	/// <summary>
	/// Reference to the transform of the container object with a mask component
	/// that collapses to hide the content
	/// </summary>
	[SerializeField] protected RectTransform maskTransform;

	/// <summary>
	/// Reference to the content container that holds the content of the view
	/// </summary>
	[SerializeField] protected GameObject contentContainer;

	[Header("Transition Settings")]
	[SerializeField] protected Direction direction = Direction.Vertical;

	/// <summary>
	/// Duration of the collapse or open transition
	/// </summary>
	public float TransitionDuration { get { return transitionDuration; } }
	[SerializeField] protected float transitionDuration = .5f;

	/// <summary>
	/// Ease used by the tween to affect the curve of motion when opening or
	/// collapsing the view
	/// </summary>
	public Ease TransitionEase { get { return transitionEase; } }
	[SerializeField] protected Ease transitionEase = Ease.OutQuart;

	[Header("Size Settings")]
	/// <summary>
	/// If true, use the screen size for the width if horizontal, height if vertical
	/// If false, use the custom sizes defined
	/// </summary>
	[SerializeField] protected bool bUseScreenSize = true;

	/// <summary>
	/// Magnitude of the open size. Controls height if vertical, width if horizontal
	/// </summary>
	public float OpenSize { get { return openSize; } }
	[SerializeField] protected float openSize = 100f;

	/// <summary>
	/// Magnitude of the closed size. Controls height if vertical, width if horizontal
	/// </summary>
	public float ClosedSize { get { return closedSize; } }
	[SerializeField] protected float closedSize = 0f;

	/// <summary>
	/// Magnitude of the unchanging size delta. Width for vertical, height for horizontal
	/// </summary>
	[SerializeField] protected float staticSize = 100f;

	/// <summary>
	/// Reference to this view's canvas component
	/// </summary>
	protected Canvas canvas;

	/// <summary>
	/// Reference to the tweener used to open/close the view
	/// </summary>
	protected Tweener collapseTween;

#region States
	/// <summary>
	/// Current state of the collapsable view
	/// </summary>
	/// <value></value>
	protected ToggleState CurrentToggleState { get { return currentToggleState; } }
	[SerializeField] protected ToggleState currentToggleState;

	/// <summary>
	/// True if the view is closed
	/// </summary>
	public bool bIsClosed { get { return currentToggleState == ToggleState.Closed; } }

	/// <summary>
	/// True if the view is in the process of opening
	/// </summary>
	public bool bIsOpening { get { return currentToggleState == ToggleState.Opening; } }

	/// <summary>
	/// True if the view is open
	/// </summary>
	public bool bIsOpen { get { return currentToggleState == ToggleState.Open; } }

	/// <summary>
	/// True if the view is in the process of closing
	/// </summary>
	public bool bIsClosing { get { return currentToggleState == ToggleState.Closing; } }
#endregion

#region Events and Delegates
	/// <summary>
	/// Delegate used to store a function to call when the open or close transition completes
	/// </summary>
	public delegate void transitionDelegate();

	/// <summary>
	/// Dispatched when a view starts opening
	/// </summary>
	/// <param name="view">View that started opening</param>
	public delegate void StartOpeningDelegate(CollapsableView view);
	public event StartOpeningDelegate OnStartOpening;

	/// <summary>
	/// Dispatched when a view finishes opening
	/// </summary>
	/// <param name="view">View that finished opening</param>
	public delegate void FinishOpeningDelegate(CollapsableView view);
	public event FinishOpeningDelegate OnFinishOpening;

	/// <summary>
	/// Dispatched when a view starts closing
	/// </summary>
	/// <param name="view">View that started closing</param>
	public delegate void StartClosingDelegate(CollapsableView view);
	public event StartClosingDelegate OnStartClosing;

	/// <summary>
	/// Dispatched when a view finishes closing
	/// </summary>
	/// <param name="view">View that finished closing</param>
	public delegate void FinishClosingDelegate(CollapsableView view);
	public event FinishClosingDelegate OnFinishClosing;
#endregion

	protected virtual void Awake()
	{
		canvas = GetComponent<Canvas>();
	}

	protected virtual void Start()
	{
		InitSizing();
		InitOpening();
	}

	/// <summary>
	/// Initializes the view as open or closed based on the selected action
	/// </summary>
	protected void InitOpening()
	{
		if(onStartAction == ToggleAction.Close)
		{
			// Set the toggle state to open so we can close the view
			ChangeToggleState(ToggleState.Open);
			ToggleShowing(false, 0f);
		}

		else if (onStartAction == ToggleAction.Open)
		{
			// Set the toggle state to open so we can close the view
			ChangeToggleState(ToggleState.Closed);
			ToggleShowing(true, 0f);
		}
	}

	/// <summary>
	/// Handles setting the static size correctly based on if we are using screen sizing
	/// </summary>
	protected void InitSizing()
	{
		if(bUseScreenSize)
		{
			if(direction == Direction.Horizontal)
				staticSize = 0;
			else if(direction == Direction.Vertical)
				staticSize = 0;
		}
	}

	/// <summary>
	/// Called by an open button to open the view
	/// </summary>
	[Button("Open")]
	public virtual void BTN_HandleOpenButton()
	{
		ToggleShowing(true, transitionDuration);
	}

	/// <summary>
	/// Called by a close button to collapse the view
	/// </summary>
	[Button("Close")]
	public virtual void BTN_HandleCloseButton()
	{
		ToggleShowing(false, transitionDuration);
	}

	/// <summary>
	/// Called by a button to toggle the menu open or closed
	/// </summary>
	/// <param name="val">Open if true, hide if false</param>
	public virtual void BTN_Toggle(bool val)
	{
		ToggleShowing(val, transitionDuration);
	}

	/// <summary>
	/// Set the canvas sorting layer of the this view
	/// </summary>
	/// <param name="sortingOrder">Sorting layer to set the canvas to</param>
	public virtual void SetSortingOrder(int sortingOrder)
	{
		canvas.overrideSorting = true;
		canvas.sortingOrder = sortingOrder;
	}

	/// <summary>
	/// Opens or collapses the collapsable view
	/// </summary>
	/// <param name="val">Opens the view if true, closes it if false</param>
	/// <param name="duration">Time it takes to open or close a view</param>
	/// <param name="onCompleteCallback">Callback that is called when the transition is complete</param>
	public virtual void ToggleShowing(bool val, float duration = .5f, transitionDelegate onCompleteCallback = null)
	{
		// Return if we are already at the desired state or already targeting it
		if( (val == true && (bIsOpen == true || bIsOpening == true)) ||
			(val == false && (bIsClosed == true || bIsClosing == true)) )
			return;

		if(val == true) HandleStartOpening();
		else HandleStartClosing();

		// Kill any current tween
		if(collapseTween != null)
			collapseTween.Kill();

		// Activate the content at the beginning if we are opening the view
		if(val == true)
			contentContainer.SetActive(true);

		// Determine what the target size delta is
		Vector2 targetSizeDelta = GetTargetSizeDelta(val);

		// Start the tween
		collapseTween = maskTransform.DOSizeDelta(targetSizeDelta, duration).
		SetEase(transitionEase).
		OnComplete(()=>
		{
			// Call the completion callback if it was set
			if(onCompleteCallback != null)
				onCompleteCallback();

			// Deactivate the content at the end if we are closing the view and it isn't showing
			if(val == false && closedSize == 0)
				contentContainer.SetActive(false);

			if(val == true)
				HandleFinishOpening();
			else
				HandleFinishClosing();

			collapseTween = null;
		});
	}

	/// <summary>
	/// Change the current toggle state
	/// </summary>
	/// <param name="state">State to change the current state to</param>
	protected void ChangeToggleState(ToggleState state)
	{
		currentToggleState = state;
	}

	/// <summary>
	/// Gets the target size delta for opening or closing the view
	/// </summary>
	/// <param name="val">Gets open target if true, close target if false</param>
	protected Vector2 GetTargetSizeDelta(bool val)
	{
		// Determine what the target size delta is
		Vector2 targetSizeDelta = new Vector2();
		if(direction == Direction.Horizontal)
			targetSizeDelta = val ? new Vector2(openSize, staticSize) : new Vector2(closedSize, staticSize);
		else if(direction == Direction.Vertical)
			targetSizeDelta = val ? new Vector2(staticSize, openSize) : new Vector2(staticSize, closedSize);

		return targetSizeDelta;
	}

	/// <summary>
	/// Handles functionality for when the view starts opening
	/// </summary>
	protected void HandleStartOpening()
	{
		ChangeToggleState(ToggleState.Opening);
		OnStartOpening?.Invoke(this);
	}

	/// <summary>
	/// Handles functionality for when the view finishes opening
	/// </summary>
	protected void HandleFinishOpening()
	{
		ChangeToggleState(ToggleState.Open);
		OnFinishOpening?.Invoke(this);
	}

	/// <summary>
	/// Handles functionality for when the view starts closing
	/// </summary>
	protected void HandleStartClosing()
	{
		ChangeToggleState(ToggleState.Closing);
		OnStartClosing?.Invoke(this);
	}

	/// <summary>
	/// Handles functionality for when the view finishes closing
	/// </summary>
	protected void HandleFinishClosing()
	{
		ChangeToggleState(ToggleState.Closed);
		OnFinishClosing?.Invoke(this);
	}
}