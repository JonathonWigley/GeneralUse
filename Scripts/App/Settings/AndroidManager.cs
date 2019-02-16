using UnityEngine;

/// <summary>
/// Created By: Jonathon Wigley - 11/03/2018
/// Last Edited By: Jonathon Wigley - 11/03/2018
/// </summary>

/// <summary>
/// Manager for any settings or setup to do with Android
/// </summary>
public class AndroidManager : MonoBehaviour
{
	public delegate void AndroidEventDelegate();
	public static event AndroidEventDelegate OnAndroidBackButton;

#if UNITY_ANDROID && !UNITY_EDITOR
	private void Awake()
	{
		Screen.fullScreen = false;
		AndroidNavigationBarUtility.dimmed = true;
	}

	private void Update()
	{
		// Android back button on the navigation bar is escape
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			OnAndroidBackButton?.Invoke();
		}
	}
#endif

	/// <summary>
	/// Move the task to the background on android
	/// </summary>
	public static void SuspendApp()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
			{
				activity.Call<bool>("moveTaskToBack", true);
			}
		}
#endif
	}

	/// <summary>
	/// Vibrate the device
	/// </summary>
	/// <param name="milliseconds">Time to vibrate the device for</param>
	public static void VibrateDevice(long milliseconds)
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		Vibration.Vibrate(milliseconds);
#endif
	}
}