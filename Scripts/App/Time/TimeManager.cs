using UnityEngine;

/// <summary>
/// Created By: Jonathon Wigley
/// Last Edited By: Jonathon Wigley - 8/5/2018
///
/// Purpose:
/// Manages timescale, pausing, and any other time related functionality
/// <summary>

public class TimeManager : MonoBehaviour
{
	private GameSettings settings;

	private void Start()
	{
		settings = GameManager.Instance.GameSettings;
		Time.timeScale = settings.TimeScale;
	}
}
