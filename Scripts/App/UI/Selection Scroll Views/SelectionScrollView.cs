using UnityEngine;
using DG.Tweening;

/// <summary>
/// Created By: Jonathon Wigley - 10/06/2018
/// Last Edited By: Jonathon Wigley - 10/06/2018
/// </summary>

/// <summary>
/// Base class for any selection scroll views.
/// </summary>
public class SelectionScrollView : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private GameObject containerObject;
	[SerializeField] private RectTransform maskTransform;

	[Header("Settings")]
	[SerializeField] private Direction expandDirection;
	[SerializeField] private float magnitude = 200f;
	[SerializeField] private Ease transitionEase = Ease.OutQuart;
	[SerializeField] private float transitionDuration = .2f;

	private Tweener showTween;

	/// <summary>
	/// Called by the toggle to show/hide the container
	/// </summary>
	/// <param name="val">Show the container if true, hide it if false</param>
	public void BTN_HandleToggle(bool val)
	{
		ToggleShowing(val);
	}

	/// <summary>
	/// Hides/Shows the selection container
	/// </summary>
	/// <param name="val">Show the container if true, hide it if false</param>
	/// <param name="duration">Amount of time it takes to show/hide the container</param>
	public void ToggleShowing(bool val, float duration = .2f)
	{
		if(showTween != null)
			showTween.Kill();

		if(val == true)
			containerObject.SetActive(true);

		Vector2 sizeDelta = expandDirection == Direction.Vertical ? new Vector2(0f, val ? magnitude : 0f) : new Vector2(val ? magnitude : 0f, 0f);
		showTween = maskTransform.DOSizeDelta(new Vector2(0f, val ? magnitude : 0f), transitionDuration).SetEase(transitionEase).OnComplete(()=>
		{
			if(val == false)
				containerObject.SetActive(false);
		});
	}
}
