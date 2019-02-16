using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Created By: Catlike Coding
/// Last Edited By: Jonathon Wigley - 8/1/2018
///
/// Purpose:
/// Manager for an object pool
/// <summary>

public class ObjectPool : MonoBehaviour
{
	PooledObject prefab;
	List<PooledObject> availableObjects = new List<PooledObject> ();

	/// <summary>
	/// Returns the object pool for the given prefab
	/// </summary>
	/// <param name="prefab"></param>
	/// <returns></returns>
	public static ObjectPool GetPool (PooledObject prefab)
	{
		GameObject obj;
		ObjectPool pool;
		if (Application.isEditor)
		{
			obj = GameObject.Find (prefab.name + " Pool");
			if (obj)
			{
				pool = obj.GetComponent<ObjectPool> ();
				if (pool)
				{
					return pool;
				}
			}
		}
		obj = new GameObject (prefab.name + " Pool");
		pool = obj.AddComponent<ObjectPool> ();
		pool.prefab = prefab;
		return pool;
	}

	/// <summary>
	/// Returns an object from the object pool to use
	/// </summary>
	/// <returns></returns>
	public PooledObject GetObject ()
	{
		PooledObject obj;
		int lastAvailableIndex = availableObjects.Count - 1;
		if (lastAvailableIndex >= 0)
		{
			obj = availableObjects[lastAvailableIndex];
			availableObjects.RemoveAt (lastAvailableIndex);
			obj.gameObject.SetActive (true);
		}
		else
		{
			obj = Instantiate<PooledObject> (prefab);
			obj.transform.SetParent (transform, false);
			obj.Pool = this;
		}
		return obj;
	}

	/// <summary>
	/// Adds an object to the object pool
	/// </summary>
	/// <param name="obj"></param>
	public void AddObject (PooledObject obj)
	{
		obj.gameObject.SetActive (false);
		availableObjects.Add (obj);
	}
}