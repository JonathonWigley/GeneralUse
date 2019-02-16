using UnityEngine;

/// <summary>
/// Created By: Jonathon Wigley - 10/12/2018
/// Last Edited By: Jonathon Wigley - 10/12/2018
/// </summary>

/// <summary>
/// Partial class of Die
/// Handles implementation of the Killable interface
/// </summary>
public partial class Die : PooledObject, IKillable
{
	/// <summary>
	/// Reference to this Die's killable interface implementation
	/// </summary>
	IKillable killable;

	/// <summary>
	/// Intialize references for the killable interace
	/// </summary>
	private void FindIKillableReference()
	{
		killable = this.GetComponent<IKillable>();
	}

	/// <summary>
	/// Called when a killable is killed
	/// </summary>
    void IKillable.Kill(GameObject killer)
    {
        ReturnToPool();
    }
}
