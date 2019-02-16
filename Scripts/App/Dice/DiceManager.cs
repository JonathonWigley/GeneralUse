using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Created By: Jonathon Wigley - 5/21/18
/// Last Edited By: Jonathon Wigley - 02/03/2019
/// </summary>

/// <summary>
/// All possible types of dice
/// </summary>
public enum DieType { d4, d6, d8, d10, d12, d20 }

/// <summary>
/// Manage functionality dealing with the dice objects rather than rolling
/// or individual funciton
/// </summary>
public class DiceManager : MonoBehaviour
{
	/// <summary>
	/// Singleton instance
	/// </summary>
	public static DiceManager Instance { get{ return instance; } }
	private static volatile DiceManager instance;

	[Header("References")]
	[SerializeField] private DiceSettings diceSettings;

	/// <summary>
	/// Reference to the current dice settings for the game
	/// </summary>
	public DiceSettings DiceSettings { get { return diceSettings; } }

	/// <summary>
	/// True if all dice are at rest
	/// </summary>
	public bool bAllDiceAtRest { get; private set; }

	/// <summary>
	/// All currently active dice
	/// </summary>
	private List<Die> dice = new List<Die>();
	public List<Die> Dice { get { return new List<Die>(dice); } }

	/// <summary>
	/// Queue of all dice that are currently rolling
	/// </summary>
	private Queue<Die> rollingDice = new Queue<Die>();

	/// <summary>
	/// Reference to the material that any spawned dice will be set to
	/// </summary>
	private DiceSkinData currentDiceSkin;

	/// <summary>
	/// Dispatched when a die is spawned
	/// </summary>
	public static event DieSpawned OnDieSpawned;
	public delegate void DieSpawned(DieData args);
	private static void DispatchDieSpawnedDelegate(DieData data)
	{ OnDieSpawned?.Invoke(data); }

	/// <summary>
	/// Dispatched when a die is destroyed
	/// </summary>
	public static event DieDestroyed OnDieDestroyed;
	public delegate void DieDestroyed(DieData args);
	private static void DispatchDieDestroyedDelegate(DieData data)
	{ OnDieDestroyed?.Invoke(data); }

	/// <summary>
	/// Dispatched when all dice come to rest
	/// </summary>
	public static event AllDiceAtRestDelegate OnAllDiceAtRest;
	public delegate void AllDiceAtRestDelegate(DiceData data);
	public void DispatchAllDiceAtRestDelegate(DiceData data)
	{ OnAllDiceAtRest?.Invoke(data); }

	private void Awake()
	{
		// Singleton initialization
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Debug.Log("Destroying singleton duplicate: " + gameObject);
			Destroy(this.gameObject);
			return;
		}
	}

	private void OnEnable()
	{
		Die.OnStartedRolling += Die_StartedRolling;
		Die.OnStoppedRolling += Die_StoppedRolling;
		SpawnDieButton.OnPressed += SpawnDieButton_Pressed;
		DiceSkinManager.OnDiceSkinChanged += DiceSkinManager_DiceSkinChanged;
		DiceSetManager.OnSetLoaded += DiceSetManager_SetLoaded;
		GameManager.OnSimulationModeChanged += GameManager_SimulationModeChanged;

		TrashCanUI.OnPressed += TrashCanUI_Pressed;
	}

	private void OnDisable()
	{
		Die.OnStartedRolling -= Die_StartedRolling;
		Die.OnStoppedRolling -= Die_StoppedRolling;
		SpawnDieButton.OnPressed -= SpawnDieButton_Pressed;
		DiceSkinManager.OnDiceSkinChanged -= DiceSkinManager_DiceSkinChanged;
		DiceSetManager.OnSetLoaded -= DiceSetManager_SetLoaded;
		GameManager.OnSimulationModeChanged -= GameManager_SimulationModeChanged;

		TrashCanUI.OnPressed -= TrashCanUI_Pressed;
	}

	/// <summary>
	/// Called when the simulation mode is changed between edit and play mode
	/// </summary>
	private void GameManager_SimulationModeChanged(SimulationMode mode)
	{
		ToggleEditModeForAllDice(mode == SimulationMode.Edit);
	}

	/// <summary>
	/// Called when a die starts rolling
	/// </summary>
	/// <param name="die"></param>
	private void Die_StartedRolling(DieData data)
	{
		if(rollingDice.Contains(data.Die))
			return;

		rollingDice.Enqueue(data.Die);
	}

	/// <summary>
	/// Called when any die stops rolling
	/// </summary>
	/// <param name="die">Die that stopped rolling</param>
	private void Die_StoppedRolling(DieData die)
	{
		// All dice have stopped rolling
		if(AreAllDiceAtRest())
		{
			bAllDiceAtRest = true;

			// Dispatch the at rest event with all dice that came to rest
			DispatchAllDiceAtRestDelegate(new DiceData(rollingDice.ToArray().ToList()));
			rollingDice.Clear();
		}
	}

	/// <summary>
	/// Called whenever the dice set manager loads a set of dice
	/// </summary>
	/// <param name="set">Dice set being loaded</param>
	private void DiceSetManager_SetLoaded(DiceSetData set)
	{
		DestroyAllDice();

		if(set.dieTypes == null)
			return;

		List<DieType> types = set.dieTypes;
		for (int i = 0; i < types.Count; i++)
		{
			SpawnDie(types[i], currentDiceSkin);
		}
	}

	/// <summary>
	/// Called when the dice skin should be changed
	/// </summary>
	/// <param name="skin">Skin to change to</param>
	private void DiceSkinManager_DiceSkinChanged(DiceSkinData skin)
	{
		ChangeAllDiceSkins(skin);
		currentDiceSkin = skin;
	}

	/// <summary>
	/// Called when a button is pressed to spawn a die
	/// </summary>
	private void SpawnDieButton_Pressed(SpawnDieButton button, DieType type)
	{
		SpawnDie(type, currentDiceSkin);
	}

	private void TrashCanUI_Pressed()
	{
		DestroyAllDice();
	}


	public bool IsAnyDieHeld()
	{
		for (int i = 0; i < dice.Count; i++)
		{
			if(dice[i].bHeld == true)
				return true;
		}

		return false;
	}

	/// <summary>
	/// Checks to see if all active dice are at rest
	/// </summary>
	/// <returns>True if all dice are at rest, false if not</returns>
	private bool AreAllDiceAtRest()
	{
		for (int i = 0; i < dice.Count; i++)
		{
			if(dice[i].bAtRest == false && dice[i].bHeld == false)
				return false;
		}

		return true;
	}

	/// <summary>
	/// Changes the material of all the dice to the given material
	/// </summary>
	/// <param name="mat"></param>
	public void ChangeAllDiceSkins(DiceSkinData skin)
	{
		for (int i = 0; i < dice.Count; i++)
		{
			dice[i].SetSkin(skin);
		}
	}

	/// <summary>
	/// Spawns a die of the given die type into the game
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="type"></param>
	public void SpawnDie(DieType type, DiceSkinData skin)
	{
		// Get a die from the object pool and add it to current dice
		Die spawnedDie = DicePrefabs.Instance.Get3DPrefab(type).GetPooledInstance<Die>();
		spawnedDie.transform.position = Vector3.up * 2f;
		spawnedDie.SetSkin(skin);
		spawnedDie.ToggleEditMode(GameManager.Instance.CurrentSimulationMode == SimulationMode.Edit);
		dice.Add(spawnedDie);

		// Dispatch the spawned die event when a die is successfully spawned
		DispatchDieSpawnedDelegate(new DieData(spawnedDie));
	}

	/// <summary>
	/// Removes the given die from the game
	/// </summary>
	/// <param name="die"></param>
	public void DestroyDie(object sender, Die die)
	{
		dice.Remove(die);
		die.ReturnToPool();

		// Dispatch the destroy die event
		DispatchDieDestroyedDelegate(new DieData(die));
	}

	/// <summary>
	/// Destroys all currently active dice
	/// </summary>
	public void DestroyAllDice()
	{
		for (int i = dice.Count - 1; i >= 0; i--)
		{
			DestroyDie(this, dice[i]);
		}
	}

	/// <summary>
	/// Change whether or not all of the dice are in edit mode or play mode
	/// </summary>
	/// <param name="val">Edit mode if true, play mode if false</param>
	public void ToggleEditModeForAllDice(bool val)
	{
		for (int i = 0; i < dice.Count; i++)
		{
			dice[i].ToggleEditMode(val);
		}
	}
}

/// <summary>
/// Data about an individual die
/// </summary>
public struct DieData
{
	/// <summary>
	/// Die associated with this data
	/// </summary>
	public Die Die;

	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="die"></param>
	public DieData(Die die)
	{
		Die = die;
	}
}

/// <summary>
/// Data about all active dice
/// </summary>
public struct DiceData
{
	/// <summary>
	/// List of all active dice associated with this data
	/// </summary>
	public List<Die> Dice;

	/// <summary>
	/// Total value of all the dice
	/// </summary>
	public int RollTotal;

	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="dice">List of dice</param>
	public DiceData(List<Die> dice)
	{
		Dice = dice;

		// Total up the rolled values of all the dice
		RollTotal = 0;
		for (int i = 0; i < dice.Count; i++)
		{
			RollTotal += dice[i].RolledValue;
		}
	}
}