using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Created By: Jonathon Wigley - 11/04/2018
/// Last Edited By: Jonathon Wigley - 11/04/2018
/// </summary>

/// <summary>
/// Parent class for any UI cell component that is spawned as part of database UI
/// </summary>
public abstract class DatabaseUICell : PooledObject
{
	/// <summary>
	/// Data associated with the cell
	/// </summary>
	[SerializeField] public DatabaseData Data { get; protected set;}

	/// <summary>
	/// Image that displays a preview of the cell's data in the UI
	/// </summary>
	[SerializeField] protected Image previewImage;

	/// <summary>
	/// Dispatched when the cell is pressed
	/// </summary>
	/// <param name="data">Data that is associated with the pressed cell</param>
	public static event PressedDelegate OnPressed;
	public delegate void PressedDelegate(DatabaseData data);
	public void DispatchPressedDelegate(DatabaseData data) { if(OnPressed != null) { OnPressed(data); } }

	/// <summary>
	/// Called by the button associated with this cell
	/// </summary>
	public void BTN_HandleButtonPress()
	{
		DispatchPressedDelegate(Data);
	}

	/// <summary>
	/// Set the data associated with this cell
	/// </summary>
	/// <param name="newData">Data to be assigned</param>
	public virtual void SetData(DatabaseData newData)
	{
		Data = newData;
	}
}