using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Created By: Jonathon Wigley - 10/24/2018
/// Last Edited By: Jonathon Wigley - 10/24/2018
/// </summary>

/// <summary>
/// Attach to an object to make it change color based on the color
/// palette manager
/// </summary>
[ExecuteInEditMode]
public class ColorPaletteObject : MonoBehaviour
{
	[SerializeField] ColorPaletteType colorPaletteType = ColorPaletteType.BackgroundPrimary;
	private ColorPaletteType cachedColorPaletteType;

	[Tooltip("If true, the image component on an object with a button component will be ignored")]
	[SerializeField] private bool ignoreButtonImage = true;

	[Tooltip("If true, will ignore setting the highlighted, pressed, and disabled button colors")]
	[SerializeField] private bool ignoreButtonColors;

	private Image image;
	private Text text;
	private TextMeshProUGUI tmProText;
	private Button button;
	private Toggle toggle;

	private void OnEnable()
	{
		GetReferences();
		RefreshColor();

		ColorPaletteManager.OnColorChanged += ColorPaletteManager_ColorChanged;
	}

	private void OnDisable()
	{
		ColorPaletteManager.OnColorChanged -= ColorPaletteManager_ColorChanged;
	}

#if UNITY_EDITOR
	private void Update()
	{
		if(cachedColorPaletteType != colorPaletteType)
		{
			RefreshColor();
			cachedColorPaletteType = colorPaletteType;
		}
	}
#endif

	private void Reset()
	{
		GetReferences();
		RefreshColor();
	}

	private void GetReferences()
	{
		cachedColorPaletteType = colorPaletteType;
		image = GetComponent<Image>();
		text = GetComponent<Text>();
		tmProText = GetComponent<TextMeshProUGUI>();
		button = GetComponent<Button>();
		toggle = GetComponent<Toggle>();
	}

	/// <summary>
	/// Called whenever a color is changed on the color palette manager
	/// </summary>
	/// <param name="type">Color palette type being changed</param>
	/// <param name="color">Color to change to</param>
	private void ColorPaletteManager_ColorChanged(ColorPaletteType type, Color color)
	{
		if(colorPaletteType == type)
			ChangeColorOfComponents(color);
	}

	/// <summary>
	/// Called when one of the button colors has been changed in the palette manager
	/// </summary>
	private void ColorPaletteManager_ButtonColorChanged()
	{
		RefreshButtonColors();
	}

	/// <summary>
	/// Change the color of all attached components
	/// </summary>
	/// <param name="color">Color to change to</param>
	private void ChangeColorOfComponents(Color color)
	{
		if(text != null) text.color = color;
		if(tmProText != null) tmProText.color = color;
		if(button != null)
		{
			// Ignore any images on buttons and let the button control it
			if(ignoreButtonImage == true)
				image = null;

			RefreshButtonColors();
		}
		if(toggle != null)
		{
			if(ignoreButtonImage == true)
				image = null;
			RefreshButtonColors();
		}
		if(image != null) image.color = color;
	}

	/// <summary>
	/// Updates the button component with the color palette manager's current button colors
	/// </summary>
	private void RefreshButtonColors()
	{
		if(ignoreButtonColors == true)
		{
			if(button != null)
			{
				ColorBlock newColors = button.colors;
				newColors.normalColor = ColorPaletteManager.Instance.GetColor(colorPaletteType);
				button.colors = newColors;
			}

			if(toggle != null)
			{
				ColorBlock newColors = toggle.colors;
				newColors.normalColor = ColorPaletteManager.Instance.GetColor(colorPaletteType);
				toggle.colors = newColors;
			}

			return;
		}

		if(button != null)
		{
			ColorBlock newColors = button.colors;
			newColors.normalColor = ColorPaletteManager.Instance.GetColor(colorPaletteType);
			newColors.pressedColor = ColorPaletteManager.Instance.ButtonPressedColor;
			newColors.highlightedColor = ColorPaletteManager.Instance.ButtonHighlightedColor;
			newColors.disabledColor = ColorPaletteManager.Instance.ButtonDisabledColor;
			button.colors = newColors;
		}

		if(toggle != null)
		{
			ColorBlock newColors = toggle.colors;
			newColors.normalColor = ColorPaletteManager.Instance.GetColor(colorPaletteType);
			newColors.pressedColor = ColorPaletteManager.Instance.ButtonPressedColor;
			newColors.highlightedColor = ColorPaletteManager.Instance.ButtonHighlightedColor;
			newColors.disabledColor = ColorPaletteManager.Instance.ButtonDisabledColor;
			toggle.colors = newColors;
		}

	}

	/// <summary>
	/// Update this object to the current color of its type
	/// </summary>
	private void RefreshColor()
	{
		ChangeColorOfComponents(ColorPaletteManager.Instance.GetColor(colorPaletteType));
	}
}
