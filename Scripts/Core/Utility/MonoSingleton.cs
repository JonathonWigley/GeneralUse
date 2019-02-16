using UnityEngine;

/// <summary>
/// Created By: Jonathon Wigley - 8/5/2018
/// Last Edited By: Jonathon Wigley - 8/5/2018
///
/// Purpose:
/// Abstract class to be inherited by singletons
/// <summary>

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
	protected static volatile T instance;
	public static T Instance
	{
		get
		{
			if(instance == null)
				instance = GameObject.FindObjectOfType<T>();

			return instance;
		}
	}

	protected virtual void Awake()
	{
		if (instance == null)
		{
			instance = this as T;
		}

		else if(instance != this)
		{
			Debug.Log("Destroying singleton duplicate: " + gameObject);
			Destroy(this.gameObject);
			return;
		}
	}
}
