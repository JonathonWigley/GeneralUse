using UnityEngine;

/// <summary>
/// Created By: Catlike Coding
/// Last Edited By: Jonathon Wigley - 8/1/2018
///
/// Purpose:
/// Object type for any object that may be pooled
/// <summary>

public class PooledObject : MonoBehaviour
{
	[System.NonSerialized]
	ObjectPool poolInstanceForPrefab;
	public ObjectPool Pool { get; set; }

	/// <summary>
	/// Returns an instance of the object from the object pool
	/// Creates an object pool for an object if it doesn't already exist
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public T GetPooledInstance<T> () where T : PooledObject
	{
		if (!poolInstanceForPrefab)
			poolInstanceForPrefab = ObjectPool.GetPool (this);

		return (T) poolInstanceForPrefab.GetObject ();
	}

	/// <summary>
	/// Returns an object to its object pool
	/// Destroys the object if it exists
	/// </summary>
	public void ReturnToPool ()
	{
		if (Pool)
			Pool.AddObject (this);
		else
			Destroy (gameObject);
	}
}