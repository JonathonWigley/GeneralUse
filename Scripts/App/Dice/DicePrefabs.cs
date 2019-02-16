using UnityEngine;

/// <summary>
/// Created By: Jonathon Wigley - 8/1/2018
/// Last Edited By: Jonathon Wigley - 8/1/2018
///
/// Purpose:
/// Maintain a reference to the dice prefabs used throughout the project
/// <summary>

public class DicePrefabs : MonoBehaviour
{
	private static volatile DicePrefabs instance;
	public static DicePrefabs Instance { get{ return instance; } }

	// 3D mode prefabs
	[SerializeField] Die D4_3D;
	[SerializeField] Die D6_3D;
	[SerializeField] Die D8_3D;
	[SerializeField] Die D10_3D;
	[SerializeField] Die D12_3D;
	[SerializeField] Die D20_3D;

	// 2D mode prefabs
	[SerializeField] Die D4_2D;
	[SerializeField] Die D6_2D;
	[SerializeField] Die D8_2D;
	[SerializeField] Die D10_2D;
	[SerializeField] Die D12_2D;
	[SerializeField] Die D20_2D;

	[SerializeField] private RigidbodyDragger dragger;
	[SerializeField] private DieLockUI lockUI;

	private void Awake()
	{
		// Singleton
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

	/// <summary>
	/// Returns the 3D prefab for the given die type
	/// </summary>
	/// <param name="type"></param>
	/// <returns></returns>
	public Die Get3DPrefab(DieType type)
	{
		switch(type)
		{
			case DieType.d4:
				return D4_3D;

			case DieType.d6:
				return D6_3D;

			case DieType.d8:
				return D8_3D;

			case DieType.d10:
				return D10_3D;

			case DieType.d12:
				return D12_3D;

			case DieType.d20:
				return D20_3D;

			default:
				return D4_3D;
		}
	}

	/// <summary>
	/// Returns the 2D prefab for the given die type
	/// </summary>
	/// <param name="type"></param>
	/// <returns></returns>
	public Die Get2DPrefab(DieType type)
	{
		switch(type)
		{
			case DieType.d4:
				return D4_2D;

			case DieType.d6:
				return D6_2D;

			case DieType.d8:
				return D8_2D;

			case DieType.d10:
				return D10_2D;

			case DieType.d12:
				return D12_2D;

			case DieType.d20:
				return D20_2D;

			default:
				return D4_2D;
		}
	}

	/// <summary>
	/// Returns the prefab of the die dragger being used
	/// </summary>
	/// <returns></returns>
	public RigidbodyDragger GetDieDraggerPrefab()
	{
		return dragger;
	}

	/// <summary>
	/// Get the prefab for the lock UI shown on dice in edit mode
	/// </summary>
	public DieLockUI GetLockUIPrefab()
	{
		return lockUI;
	}
}
