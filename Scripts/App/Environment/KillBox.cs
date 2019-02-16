using UnityEngine;
using System.Collections;

/// <summary>
/// Created By: Jonathon Wigley - 09/23/2018
/// Last Edited By: Jonathon Wigley - 09/23/2018
/// </summary>

/// <summary>
/// Purpose:
/// Destroys any object with collision that this comes is contact with
/// <summary>

public class KillBox : MonoBehaviour
{
	GameSettings settings;
	public bool Active { get; private set; }

	private void Awake()
	{
		settings = GameManager.Instance.GameSettings;
		ToggleCollider(false);
	}

	private IEnumerator Start()
	{
		yield return new WaitForSeconds(settings.KillBoxInitializeDelay);

		ToggleCollider(true);
	}

	/// <summary>
	/// Enable or disable the collider attached to this object
	/// </summary>
	/// <param name="val">If true, enable the collider, if false, disable it</param>
	private void ToggleCollider(bool val)
	{
		GetComponent<Collider>().enabled = val;
	}

	/// <summary>
	/// Destroy objects when they come in contact with this one
	/// </summary>
	/// <param name="other">Object that came into contact with this one</param>
	private void OnTriggerEnter(Collider other)
	{
		if(settings == null)
			settings = GameManager.Instance.GameSettings;

		// Destroy the object if the killbox layermask contains the objects layer
		if(settings.KillBoxLayerMask == (settings.KillBoxLayerMask | (1 >> gameObject.layer)))
		{
			// Only kill the object if it is killable
			IKillable killable = other.GetComponent<IKillable>();
			if(killable == null) killable = other.GetComponentInParent<IKillable>();
			if(killable == null) killable = other.GetComponentInChildren<IKillable>();

			if(killable != null)
			{
				Debug.Log("Killed: " + other.gameObject, this);
				killable.Kill(this.gameObject);
			}

			Debug.Log("Killable: " + other.gameObject, this);
		}
	}
}
