using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class SaveDataManager : MonoBehaviour
{
	/// <summary>
	/// List of all scriptable objects to be saved
	/// </summary>
	[Tooltip("All Scriptable Objects to be saved")]
	[SerializeField] private List<ScriptableObject> savedObjects = new List<ScriptableObject>();

	bool bDataSaved = false;

	private void OnEnable()
	{
		bDataSaved = false;
		ReadData();
	}

	private void OnDisable()
	{
		if(bDataSaved == false)
			SaveData();
	}

	private void OnDestroy()
	{
		if(bDataSaved == false)
			SaveData();
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if(pauseStatus == true && bDataSaved == false)
			SaveData();
	}

	/// <summary>
	/// Save all of the saved objects to storage
	/// </summary>
	private void SaveData()
	{
		string persitentDataPath = Application.persistentDataPath;
		Debug.Log("Saving to " + persitentDataPath, this);

		string fileName = "newFile";
		string json = "";

		// Write each object to its own json file
		for (int i = 0; i < savedObjects.Count; i++)
		{
			fileName = "\\" + savedObjects[i].name + ".json";
			json = JsonUtility.ToJson(savedObjects[i]);

			File.WriteAllText(persitentDataPath + fileName, json);
		}

		bDataSaved = true;
	}

	/// <summary>
	/// Read data from storage and overwrite the saved objects
	/// </summary>
	private void ReadData()
	{
		string persitentDataPath = Application.persistentDataPath;
		Debug.Log("Reading from " + persitentDataPath, this);

		string fileName = "newFile";
		string json = "";

		// Write each object to its own json file
		for (int i = 0; i < savedObjects.Count; i++)
		{
			fileName = "\\" + savedObjects[i].name + ".json";

			if(File.Exists(persitentDataPath + fileName))
			{
				// Read the json file and overwrite the object
				json = File.ReadAllText(persitentDataPath + fileName);
				JsonUtility.FromJsonOverwrite(json, savedObjects[i]);
			}
			else
			{
				Debug.Log("Could not find saved data", this);
			}
		}
	}
}
