using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Created By: Jonathon Wigley - 10/21/2018
/// Last Edited By: Jonathon Wigley - 10/21/2018
/// </summary>

/// <summary>
/// Manager for a collapsable view that has sub-collapsable views that
/// are changed by tabs
/// </summary>
public class TabbedCollapsableViewManager : CollapsableView
{
	[Header("Tabbed View References")]
	/// <summary>
	/// List of all the views that are controlled by tabs
	/// </summary>
	[SerializeField] protected List<CollapsableView> tabbedViews = new List<CollapsableView>();

	[Header("Tabbed View Settings")]
	/// <summary>
	/// Sorting order that the current view's canvas is set to
	/// </summary>
	[SerializeField] protected int currentViewSortingOrder = 10;

	/// <summary>
	/// Sorting order that views are set to when they aren't the current view
	/// </summary>
	[SerializeField] protected int defaultSortingOrder = 0;

	[SerializeField]
	/// <summary>
	/// Transition duration for the tab views
	/// </summary>
	protected float tabTransitionDuration = 0;

	/// <summary>
	/// Reference to the last view that was open
	/// </summary>
	protected CollapsableView previousView;

	/// <summary>
	/// Reference to the current open or opening view
	/// </summary>
	protected CollapsableView currentView;

	/// <summary>
	/// Opens a collapsable view
	/// </summary>
	/// <param name="view">View to open</param>
	public virtual void BTN_OpenView(CollapsableView view)
	{
		// If the tabbed view is closed or closing, open it
		if(bIsClosed || bIsClosing)
		{
			// Open the selected view immediately if opening the whole tabbed view
			OpenView(view, 0f);
			ToggleShowing(true, transitionDuration);
		}

		// Close the tabbed view if we click the current view's tab
		else if(view == currentView)
		{
			ToggleShowing(false, transitionDuration);
		}

		// Open the selected view
		else
		{
			OpenView(view, tabTransitionDuration);
		}
	}

	/// <summary>
	/// Closes a collapsable view
	/// </summary>
	/// <param name="view">View to close</param>
	public virtual void BTN_CloseView(CollapsableView view)
	{
		CloseView(view, tabTransitionDuration);
	}

	/// <summary>
	/// Opens or closes a specific collapsable view
	/// </summary>
	/// <param name="view"></param>
	public virtual void BTN_ToggleView(CollapsableView view)
	{
		ToggleView(view, transitionDuration);
	}

	/// <summary>
	/// Opens a view
	/// </summary>
	/// <param name="view">View to open</param>
	/// <param name="duration">Amount of time it takes to open the view</param>
	public virtual void OpenView(CollapsableView view, float duration = .5f)
	{
		// Don't do anything if the view is null or this isn't the manager for the view
		if(view == null || tabbedViews.Contains(view) == false)
			return;

		// If the current view is still opening and we want to switch to the previous view
		// just close the current view
		if(view == previousView && currentView.bIsOpening)
		{
			CloseView(currentView, duration, onCompleteCallback: ()=>
			{
				// Update sorting orders at end of transition so the correct view is on top
				previousView.SetSortingOrder(defaultSortingOrder);
				currentView.SetSortingOrder(currentViewSortingOrder);
			});

			// Update the previous and current views
			previousView = currentView;
			currentView = view;
		}

		// Normal transition
		else
		{
			// Update the previous and current views
			previousView = currentView;
			currentView = view;

			// Update sorting orders so the correct view is on top
			previousView?.SetSortingOrder(defaultSortingOrder);
			currentView.SetSortingOrder(currentViewSortingOrder);

			view.ToggleShowing(true, duration, onCompleteCallback: ()=>
			{
				if(previousView != currentView)
					CloseView(previousView, 0f);
			});
		}
	}

	/// <summary>
	/// Closes a view
	/// </summary>
	/// <param name="view">View to close</param>
	/// <param name="duration">Amount of time it takes to close the view</param>
	public virtual void CloseView(CollapsableView view, float duration = .5f, transitionDelegate onCompleteCallback = null)
	{
		if(view == null)
			return;

		view.ToggleShowing(false, duration);
	}

	/// <summary>
	/// Opens a closed view or closes and open view
	/// </summary>
	/// <param name="view">View to toggle</param>
	/// <param name="duration">Amount of time it takes to open or close a view</param>
	public virtual void ToggleView(CollapsableView view, float duration = .5f)
	{
		if(view == null)
			return;

		if(view.bIsOpen || view.bIsOpening)
			CloseView(view, duration);
		else
			OpenView(view, duration);
	}
}