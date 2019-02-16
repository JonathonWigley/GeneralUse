using UnityEngine;

/// <summary>
/// Created By: Jonathon Wigley - 12/29/2018
/// Last Edited By: Jonathon Wigley - 12/29/2018
/// </summary>

/// <summary>
/// Partial class of Die
/// Implementation of the ISwipable interface
/// </summary>
public partial class Die : ISwipable
{
	/// <summary>
	/// Reference to the ISwipable interface implemented by this die
	/// </summary>
	ISwipable swipable;

	/// <summary>
	/// Get the reference to the ISwipable interface on this object
	/// </summary>
	private void FindISwipableReference()
	{
		swipable = this.GetComponent<ISwipable>();
	}

	/// <summary>
	/// Implementation of the swipe function that is called when this die is swiped
	/// </summary>
    void ISwipable.Swipe(Vector3 swipeDirection)
    {
		HandleSwipeForPlayMode(swipeDirection);
    }
}
