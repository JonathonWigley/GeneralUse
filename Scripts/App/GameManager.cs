using UnityEngine;

/// <summary>
/// Created By: Jonathon Wigley - 8/5/2018
/// Last Edited By: Jonathon Wigley - 10/24/2018
/// </summary>

/// <summary>
/// Modes that the game can be in
/// </summary>
public enum GameMode
{
	/// <summary>
	/// 3D Play mode where dice are able to be rolled
	/// </summary>
	Simulation,

	/// <summary>
	/// 2D Mode
	/// </summary>
	Flat
}

/// <summary>
/// Modes that the simulation mode can be in
/// </summary>
public enum SimulationMode
{
	/// <summary>
	/// Mode used for spawning and rolling dice
	/// </summary>
	Play,

	/// <summary>
	///
	/// </summary>
	Edit
}

/// <summary>
/// Manager to store core game settings and information to be pulled by other objects
/// </summary>
public class GameManager : MonoSingleton<GameManager>
{
	/// <summary>
	/// Settings for the game
	/// </summary>
	public GameSettings GameSettings { get { return settings.Clone(); } }
	[SerializeField] private GameSettings settings;

	/// <summary>
	/// Database containing all the dice skins that are available
	/// </summary>
	public DiceSkinDatabase Skins { get { return skins.Clone(); } }
	[SerializeField] private DiceSkinDatabase skins;

	/// <summary>
	/// Current mode the game is in
	/// </summary>
	public GameMode CurrentGameMode { get { return currentGameMode; } }

	/// <summary>
	/// Current game mode the game is in, use ChangeGameMode() to edit
	/// </summary>
	private GameMode currentGameMode;

	/// <summary>
	/// Current type of simulation mode the game is in if the game is in simulation mode
	/// </summary>
	public SimulationMode CurrentSimulationMode { get { return currentSimulationMode; } }

	/// <summary>
	/// Current simulation mode the game is in (if in simulation mode.
	/// Use ChangeSimulationMode() to edit
	/// </summary>
	private SimulationMode currentSimulationMode;

	/// <summary>
	/// Dispatched whenever the game mode changes
	/// </summary>
	/// <param name="mode">Mode that the game was changed to</param>
	public static event GameModeChangedDelegate OnGameModeChanged;
	public delegate void GameModeChangedDelegate(GameMode mode);

	/// <summary>
	/// Dispatched when the simulation mode is changed
	/// </summary>
	/// <param name="mode">Simulation mode that is active now</param>
	public static event SimulationModeChangedDelegate OnSimulationModeChanged;
	public delegate void SimulationModeChangedDelegate(SimulationMode mode);

	private void Start()
	{
		ChangeGameMode(GameMode.Simulation);
		ChangeSimulationMode(SimulationMode.Play);
	}

	private void OnEnable()
	{
		TogglePlayModeButton.OnPressed += TogglePlayModeButton_Pressed;
	}

	private void OnDisable()
	{
		TogglePlayModeButton.OnPressed -= TogglePlayModeButton_Pressed;
	}

	/// <summary>
	/// Called when the play mode toggle button is pressed
	/// </summary>
	private void TogglePlayModeButton_Pressed()
	{
		ToggleSimulationMode();
	}

	/// <summary>
	/// Change to flat mode if currently in simulation mode, or
	/// simulation mode if in flat mode
	/// </summary>
	private void ToggleGameMode()
	{
		ChangeGameMode(currentGameMode == GameMode.Simulation ?
						GameMode.Flat :
						GameMode.Simulation);
	}

	/// <summary>
	/// Change to play mode if currently in edit mode,
	/// or edit mode if currently in play mode
	/// </summary>
	private void ToggleSimulationMode()
	{
		ChangeSimulationMode(CurrentSimulationMode == SimulationMode.Play ?
						SimulationMode.Edit :
						SimulationMode.Play);
	}

	/// <summary>
	/// Change the game mode
	/// </summary>
	/// <param name="mode">Mode to change to</param>
	private void ChangeGameMode(GameMode mode)
	{
		if(CurrentGameMode == GameMode.Simulation && mode != GameMode.Simulation)
			ChangeSimulationMode(SimulationMode.Play);

		currentGameMode = mode;
		OnGameModeChanged?.Invoke(mode);
	}

	/// <summary>
	/// Change the mode of simulation while in simulation mode
	/// </summary>
	/// <param name="mode">Simulation mode to change to</param>
	private void ChangeSimulationMode(SimulationMode mode)
	{
		if(currentGameMode != GameMode.Simulation)
			return;

		currentSimulationMode = mode;
		OnSimulationModeChanged?.Invoke(mode);
	}
}