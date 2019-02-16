using UnityEngine;

/// <summary>
/// Created By: Jonathon Wigley - 09/23/2018
/// Last Edited By: Jonathon Wigley - 09/23/2018
/// </summary>

/// <summary>
/// Purpose:
/// Interface for any object that can be killed by killboxes
/// <summary>

public interface IKillable
{
	void Kill(GameObject killer);
}