using UnityEngine;

/// <summary>
/// Created By: Jonathon Wigley - 10/24/2018
/// Last Edited By: Jonathon Wigley - 10/31/2018
/// </summary>

/// <summary>
/// Different types of colors set by the color palette manager
/// </summary>
public enum ColorPaletteType
{
	BackgroundPrimary,
	BackgroundSecondary,
	BackgroundTertiary,
	TextPrimary,
	TextSecondary
}

/// <summary>
/// Manager object that is used to change the color scheme and broadcasts
/// events when its colors are changed
/// </summary>
[ExecuteInEditMode]
public class ColorPaletteManager : MonoBehaviour
{
	private static ColorPaletteManager instance;
	public static ColorPaletteManager Instance
	{
		get
		{
			if(instance != null)
			{
				return instance;
			}
			else
			{
				instance = GameObject.FindObjectOfType<ColorPaletteManager>();
				return instance;
			}
		}
	}

	public Color PrimaryBackgroundColor { get { return primaryBackgroundColor; } }
	[SerializeField] private Color primaryBackgroundColor = Color.white;

	public Color SecondaryBackgroundColor { get { return secondaryBackgroundColor; } }
	[SerializeField] private Color secondaryBackgroundColor = Color.white;

	public Color TertiaryBackgroundColor { get { return tertiaryBackgroundColor; } }
	[SerializeField] private Color tertiaryBackgroundColor = Color.white;

	public Color PrimaryTextColor { get { return primaryTextColor; } }
	[SerializeField] private Color primaryTextColor = Color.white;

	public Color SecondaryTextColor { get { return secondaryTextColor; } }
	[SerializeField] private Color secondaryTextColor = Color.white;

	public Color ButtonHighlightedColor { get { return buttonHighlightedColor; } }
	[SerializeField] private Color buttonHighlightedColor = Color.white;

	public Color ButtonPressedColor { get { return buttonPressedColor; } }
	[SerializeField] private Color buttonPressedColor = Color.white;

	public Color ButtonDisabledColor { get { return buttonDisabledColor; } }
	[SerializeField] private Color buttonDisabledColor = Color.white;

	private Color cachedPrimaryBackgroundColor;
	private Color cachedSecondaryBackgroundColor;
	private Color cachedTertiaryBackgroundColor;
	private Color cachedPrimaryTextColor;
	private Color cachedSecondaryTextColor;
	private Color cachedButtonHighlightedColor;
	private Color cachedButtonPressedColor;
	private Color cachedButtonDisabledColor;

	public delegate void ColorChangedDelegate(ColorPaletteType type, Color color);
	public static event ColorChangedDelegate OnColorChanged;

	public delegate void ButtonColorChangedDelegate();
	public static event ButtonColorChangedDelegate OnButtonColorChanged;

	private void OnEnable()
	{
		cachedPrimaryBackgroundColor = primaryBackgroundColor;
		cachedSecondaryBackgroundColor = secondaryBackgroundColor;
		cachedTertiaryBackgroundColor = tertiaryBackgroundColor;
		cachedPrimaryTextColor = primaryTextColor;
		cachedSecondaryTextColor = secondaryTextColor;
	}

#if UNITY_EDITOR
	void Update ()
	{
		if (cachedPrimaryBackgroundColor != primaryBackgroundColor)
		{
			ChangeColor(ColorPaletteType.BackgroundPrimary, primaryBackgroundColor);
			cachedPrimaryBackgroundColor = primaryBackgroundColor;
		}

		if (cachedSecondaryBackgroundColor != secondaryBackgroundColor)
		{
			ChangeColor(ColorPaletteType.BackgroundSecondary, secondaryBackgroundColor);
			cachedSecondaryBackgroundColor = secondaryBackgroundColor;
		}

		if (cachedTertiaryBackgroundColor != tertiaryBackgroundColor)
		{
			ChangeColor(ColorPaletteType.BackgroundTertiary, tertiaryBackgroundColor);
			cachedTertiaryBackgroundColor = tertiaryBackgroundColor;
		}

		if (cachedPrimaryTextColor != primaryTextColor)
		{
			ChangeColor(ColorPaletteType.TextPrimary, primaryTextColor);
			cachedPrimaryTextColor = primaryTextColor;
		}

		if (cachedSecondaryTextColor != secondaryTextColor)
		{
			ChangeColor(ColorPaletteType.TextSecondary, secondaryTextColor);
			cachedSecondaryTextColor = secondaryTextColor;
		}

		if (cachedButtonPressedColor != buttonPressedColor ||
			cachedButtonHighlightedColor != buttonHighlightedColor ||
			cachedButtonDisabledColor != buttonDisabledColor)
		{
			ChangeButtonColors();
		}
	}
#endif

	/// <summary>
	/// Change the color of a certain type
	/// </summary>
	/// <param name="type">Type to change</param>
	/// <param name="color">Color to change the type to</param>
	private void ChangeColor(ColorPaletteType type, Color color)
	{
		OnColorChanged?.Invoke(type, color);
	}

	/// <summary>
	/// Dispatch the button color changing event and reset cached button colors
	/// </summary>
	private void ChangeButtonColors()
	{
		cachedButtonPressedColor = buttonPressedColor;
		cachedButtonHighlightedColor = buttonHighlightedColor;
		cachedButtonDisabledColor = buttonDisabledColor;
		OnButtonColorChanged?.Invoke();
	}

	/// <summary>
	/// Get the color of the given type
	/// </summary>
	/// <param name="type">Type of color to get</param>
	public Color GetColor(ColorPaletteType type)
	{
		switch(type)
		{
			case ColorPaletteType.BackgroundPrimary:
				return PrimaryBackgroundColor;

			case ColorPaletteType.BackgroundSecondary:
				return SecondaryBackgroundColor;

			case ColorPaletteType.BackgroundTertiary:
				return TertiaryBackgroundColor;

			case ColorPaletteType.TextPrimary:
				return primaryTextColor;

			case ColorPaletteType.TextSecondary:
				return secondaryTextColor;

			default:
				return PrimaryBackgroundColor;
		}
	}

	/// <summary>
	/// Send out an event that will update all colors
	/// </summary>
	[EasyButtons.Button("Update")]
	private void BroadcastAllColors()
	{
		ChangeColor(ColorPaletteType.BackgroundPrimary, PrimaryBackgroundColor);
		ChangeColor(ColorPaletteType.BackgroundSecondary, SecondaryBackgroundColor);
		ChangeColor(ColorPaletteType.BackgroundTertiary, TertiaryBackgroundColor);
		ChangeColor(ColorPaletteType.TextPrimary, PrimaryTextColor);
		ChangeColor(ColorPaletteType.TextSecondary, SecondaryTextColor);
	}
}
