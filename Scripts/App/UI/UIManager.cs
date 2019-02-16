using UnityEngine;

public class UIManager : MonoBehaviour
{
	[Header("UI References")]
	/// <summary>
	/// Reference to the bottom popup canvas that contains play mode selection UI
	/// </summary>
	[SerializeField] private BottomSelectionViewManager playModeBottomCanvas;

	/// <summary>
	/// Reference to the bottom popup canvas that contains edit mode selection UI
	/// </summary>
	[SerializeField] private BottomSelectionViewManager editModeBottomCanvas;

	private void OnEnable()
	{
		GameManager.OnSimulationModeChanged += GameManager_SimulationModeChanged;
	}

	private void OnDisable()
	{
		GameManager.OnSimulationModeChanged -= GameManager_SimulationModeChanged;
	}

	/// <summary>
	/// Called when the game changes current mode
	/// </summary>
	/// <param name="mode">Mode that was changed to</param>
	private void GameManager_SimulationModeChanged(SimulationMode mode)
	{
		if(mode == SimulationMode.Play)
		{
			TogglePlayModeUI(true);
			ToggleEditModeUI(false);
		}

		else if(mode == SimulationMode.Edit)
		{
			TogglePlayModeUI(false);
			ToggleEditModeUI(true);
		}
	}

	/// <summary>
	/// Turns on or off the play mode UI
	/// </summary>
	/// <param name="val">Turn on if true, off if false</param>
	private void TogglePlayModeUI(bool val)
	{
		playModeBottomCanvas.gameObject.SetActive(val);
		if(val == true)
		{
			// Immediately open the view if the edit mode view is open/opening
			if(editModeBottomCanvas.bIsOpen || editModeBottomCanvas.bIsOpening == true)
			{
				playModeBottomCanvas.ToggleShowing(true, 0f);
			}

			// Immediately close the view if the edit view is closed
			else
			{
				playModeBottomCanvas.ToggleShowing(false, 0f);
			}
		}
	}

	/// <summary>
	/// Turns on or off the edit mode UI
	/// </summary>
	/// <param name="val">Turn on if true, off if false</param>
	private void ToggleEditModeUI(bool val)
	{
		editModeBottomCanvas.gameObject.SetActive(val);
		if(val == true)
		{
			// Immediately open the view if the play mode view is open/opening
			if(playModeBottomCanvas.bIsOpen || playModeBottomCanvas.bIsOpening == true)
			{
				editModeBottomCanvas.ToggleShowing(true, 0f);
			}

			// Immediately close the view if the play view is closed
			else
			{
				editModeBottomCanvas.ToggleShowing(false, 0f);
			}
		}
	}
}
