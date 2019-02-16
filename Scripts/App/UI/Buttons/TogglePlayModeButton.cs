using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Created By: Jonathon Wigley - 10/24/2018
/// Last Edited By: Jonathon Wigley - 10/24/2018
/// </summary>

/// <summary>
/// Component attached to the button used to toggle play and edit mode
/// </summary>
public class TogglePlayModeButton : MonoBehaviour
{
	/// <summary>
	/// Sprite that is shown when in play mode
	/// </summary>
	[SerializeField] Sprite playModeSprite;

	/// <summary>
	/// Sprite that is shown during edit mode
	/// </summary>
	[SerializeField] Sprite editModeSprite;

	/// <summary>
	/// Reference to the image displaying the mode sprite
	/// </summary>
	Image toggleImage;

	/// <summary>
	/// Dispatched when the play mode button is toggled
	/// </summary>
	public static event PressedDelegate OnPressed;
	public delegate void PressedDelegate();

	private void Awake()
	{
		toggleImage = GetComponent<Image>();
	}

	private void OnEnable()
	{
		GameManager.OnSimulationModeChanged += GameManager_SimulationModeChanged;
	}

	private void OnDisable()
	{
		GameManager.OnSimulationModeChanged -= GameManager_SimulationModeChanged;
	}

	/// <summary>
	/// Called when this button is pressed
	/// </summary>
	public void BTN_HandleButtonPress()
	{
		OnPressed?.Invoke();
	}

	/// <summary>
	/// Called when the game mode is changed
	/// </summary>
	/// <param name="mode"></param>
	private void GameManager_SimulationModeChanged(SimulationMode mode)
	{
		ChangeState(mode);
	}

	/// <summary>
	/// Change the state of the button based on game mode
	/// </summary>
	/// <param name="mode">Game mode</param>
	private void ChangeState(SimulationMode mode)
	{
		switch(mode)
		{
			case SimulationMode.Play:
				toggleImage.sprite = playModeSprite;
			break;

			case SimulationMode.Edit:
				toggleImage.sprite = editModeSprite;
			break;
		}
	}
}