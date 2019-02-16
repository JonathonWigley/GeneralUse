using UnityEngine;

/// <summary>
/// Created By: Jonathon Wigley - 10/21/2018
/// Last Edited By: Jonathon Wigley - 10/21/2018
/// </summary>

/// <summary>
/// Tabbed collapsable view manager meant to control the bottom selection tab views
/// </summary>
public class BottomSelectionViewManager : TabbedCollapsableViewManager
{
	protected override void Start()
	{
		base.Start();

		currentView = tabbedViews[0];
	}

	private void OnEnable()
	{
		ScreenFlickManager.OnFlickUp += ScreenFlickManager_FlickUp;
		ScreenFlickManager.OnFlickDown += ScreenFlickManager_FlickDown;
	}

	private void OnDisable()
	{
		ScreenFlickManager.OnFlickUp -= ScreenFlickManager_FlickUp;
		ScreenFlickManager.OnFlickDown -= ScreenFlickManager_FlickDown;
	}

	/// <summary>
	/// Called when a flick up is registered
	/// </summary>
	private void ScreenFlickManager_FlickUp()
	{
		if(bIsOpen || bIsOpening)
			return;

		if(currentView == null)
			OpenView(tabbedViews[0]);

		ToggleShowing(true, transitionDuration);
	}

	/// <summary>
	/// Called when a flick down is registered
	/// </summary>
	private void ScreenFlickManager_FlickDown()
	{
		if(bIsClosed || bIsClosing)
			return;

		ToggleShowing(false, transitionDuration);
	}
}
