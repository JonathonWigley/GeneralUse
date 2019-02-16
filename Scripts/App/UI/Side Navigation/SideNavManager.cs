using System;
using System.Collections.Generic;
using UnityEngine;
using TouchScript;
using TouchScript.Pointers;
using TouchScript.Gestures;

/// <summary>
/// Created By: Jonathon Wigley - 10/20/2018
/// Last Edited By: Jonathon Wigley - 10/21/2018
/// </summary>

/// <summary>
/// Collapsable view with sub-collapsable views that act as the navigation sidebar
/// </summary>
public class SideNavManager : CollapsableView
{
	[Header("Side Nav References")]
	/// <summary>
	/// Reference to the main side nav menu that will be opened first
	/// </summary>
	[SerializeField] private CollapsableView mainMenuView;

	/// <summary>
	/// List of all the collapsable views that are part of the side nav
	/// </summary>
	[SerializeField] private List<CollapsableView> sideNavViews = new List<CollapsableView>();

	[SerializeField] protected FlickGesture flickGesture;
	[SerializeField] protected float minNormalizedFlickDistance = .2f;

	/// <summary>
	/// Reference to the TouchManager instance
	/// </summary>
	protected ITouchManager touchManager;

	/// <summary>
	/// Reference to the side nav's rect transform
	/// </summary>
	protected RectTransform sideNavRectTransform;

	/// <summary>
	/// Reference to the last collapsable view that was opened
	/// </summary>
	protected CollapsableView currentView;

	/// <summary>
	/// Reference to the last opened view
	/// </summary>
	protected CollapsableView previousView;

	/// <summary>
	/// True if the current view is in the process of opening
	/// </summary>
	private bool bViewIsOpening;

	protected override void Awake()
	{
		base.Awake();

		sideNavRectTransform = GetComponent<RectTransform>();
		touchManager = TouchManager.Instance;
	}

	private void OnEnable()
	{
		touchManager.PointersReleased += TouchManager_PointersReleased;
		ScreenFlickManager.OnFlickRight += ScreenFlickManager_FlickRight;
		ScreenFlickManager.OnFlickLeft += ScreenFlickManager_FlickLeft;
		AndroidManager.OnAndroidBackButton += AndroidManager_AndroidBackButton;
		RollTotalDisplay.OnPressed += RollTotalDisplay_Pressed;
	}

	private void OnDisable()
	{
		touchManager.PointersReleased -= TouchManager_PointersReleased;
		ScreenFlickManager.OnFlickRight -= ScreenFlickManager_FlickRight;
		ScreenFlickManager.OnFlickLeft -= ScreenFlickManager_FlickLeft;
		AndroidManager.OnAndroidBackButton -= AndroidManager_AndroidBackButton;
		RollTotalDisplay.OnPressed -= RollTotalDisplay_Pressed;
	}

	/// <summary>
	/// Called when a pointer is released
	/// </summary>
	/// <param name="sender">Touch manager that sent the event</param>
	/// <param name="args">Data about the pointers that were released</param>
	private void TouchManager_PointersReleased(object sender, PointerEventArgs args)
	{
		if(bIsClosed == true || bIsClosing == true)
			return;

		// Return if the open tween just started
		if(collapseTween != null && collapseTween.fullPosition < .1f)
			return;

		IList<Pointer> activePointers = touchManager.PressedPointers;
		IList<Pointer> releasedPointers = args.Pointers;

		// Check to see if any pointers are inside the rect
		bool allPointersOutOfRect = true;
		for (int i = 0; i < releasedPointers.Count; i++)
		{
			if(RectTransformUtility.RectangleContainsScreenPoint(sideNavRectTransform , releasedPointers[i].Position))
			{
				allPointersOutOfRect = false;
				break;
			}
		}

		// If all the pointers were released outside of the rect, check to see if any are still in the rect
		if(allPointersOutOfRect == true)
		{
			for (int i = 0; i < activePointers.Count; i++)
			{
				if(RectTransformUtility.RectangleContainsScreenPoint(sideNavRectTransform , activePointers[i].Position))
				{
					allPointersOutOfRect = false;
					break;
				}
			}
		}

		// Close the side nav if no pointers are inside the nav
		if(allPointersOutOfRect)
		{
			// Close the side nav and then close all the subviews
			ToggleShowing(false, transitionDuration, HideAllViewsImmediate);
		}
	}

	/// <summary>
	/// Called when a right flick is detected
	/// </summary>
	protected void ScreenFlickManager_FlickRight()
	{
		ToggleShowing(true, transitionDuration);
	}

	/// <summary>
	/// Called when a left flick is detected
	/// </summary>
	protected void ScreenFlickManager_FlickLeft()
	{
		ToggleShowing(false, transitionDuration);
	}

	/// <summary>
	/// Called when the back button is pressed on android
	/// </summary>
	protected void AndroidManager_AndroidBackButton()
	{
		if(currentToggleState == ToggleState.Open || currentToggleState == ToggleState.Opening)
			ToggleShowing(false, transitionDuration);
		else
			AndroidManager.SuspendApp();
	}

	/// <summary>
	/// Called when the roll total display is pressed
	/// </summary>
	protected void RollTotalDisplay_Pressed()
	{
		OpenHistory();
	}

	/// <summary>
	/// Opens a collapsable view
	/// </summary>
	/// <param name="view">View to open</param>
	public void BTN_OpenView(CollapsableView view)
	{
		OpenView(view, transitionDuration);
	}

	/// <summary>
	/// Closes a collapsable view
	/// </summary>
	/// <param name="view">View to close</param>
	public void BTN_CloseView(CollapsableView view)
	{
		CloseView(view, transitionDuration);
	}

	/// <summary>
	/// Opens or closes a specific collapsable view
	/// </summary>
	/// <param name="view"></param>
	public void BTN_ToggleView(CollapsableView view)
	{
		ToggleView(view, transitionDuration);
	}

	/// <summary>
	/// Opens a specific view
	/// </summary>
	/// <param name="view">View to open</param>
	private void OpenView(CollapsableView view, float duration = .5f)
	{
		if(view == null || bViewIsOpening == true)
			return;

		// Cache the previous view
		previousView = currentView;

		// Set the new view to the current view
		currentView = view;

		// Only say the new view is opening if it is closed
		if(view.bIsOpen == false)
		{
			bViewIsOpening = true;

			// Set any newly opened views to layer 40
			if(view != mainMenuView)
				view.SetSortingOrder(40);
		}

		view.ToggleShowing(true, duration, onCompleteCallback: ()=>
		{
			// Close the previous view if the new view is different
			if(previousView != currentView)
				CloseView(previousView, 0f);

			bViewIsOpening = false;
		});
	}

	/// <summary>
	/// Closes a specific view
	/// </summary>
	/// <param name="view">View to close</param>
	private void CloseView(CollapsableView view, float duration = .5f)
	{
		if(view == null)
			return;

		// Uncheck that the current view is opening if it is closed before it finishes opening
		if(bViewIsOpening == true && view == currentView)
		{
			bViewIsOpening = false;
		}

		// Open the main menu to go back to when closing a view
		if(view != mainMenuView)
		{
			OpenView(mainMenuView, 0f);

			// Set the sorting layer of a closing view back to 30
			view.SetSortingOrder(30);
		}

		// Close the target view
		view.ToggleShowing(false, duration);
	}

	/// <summary>
	/// Opens a closed view or closes an open view
	/// </summary>
	/// <param name="view">View to toggle</param>
	private void ToggleView(CollapsableView view, float duration = .5f)
	{
		if(view == null)
			return;

		// Close view
		if(view.bIsOpen)
		{
			CloseView(view, duration);
		}

		// Open view
		else
		{
			OpenView(view, duration);
		}
	}

	/// <summary>
	/// Opens or closes the side nav
	/// </summary>
	/// <param name="val">Opens if true, closes if false</param>
	/// <param name="duration">Amount of time it takes to open or close in seconds. Default .5</param>
	public override void ToggleShowing(bool val, float duration = .5f, transitionDelegate onCompleteCallback = null)
	{
		base.ToggleShowing(val, duration, onCompleteCallback);

		// Open side nav
		if(val == true)
		{
			// Make sure the main menu view is open all the way when we open the side nav
			OpenView(mainMenuView, 0f);
		}
	}

	/// <summary>
	/// Hides all the views that are open immediately without transition
	/// </summary>
	private void HideAllViewsImmediate()
	{
		for (int i = 0; i < sideNavViews.Count; i++)
		{
			if(sideNavViews[i].bIsOpen)
				sideNavViews[i].ToggleShowing(false, 0f);
		}
	}

#region Open Specific Views
	public void OpenHistory()
	{
		ToggleShowing(true, transitionDuration);
		OpenView(sideNavViews.Find(x => x.GetType() == typeof(HistoryCollapsableView)));
	}

#endregion
}
