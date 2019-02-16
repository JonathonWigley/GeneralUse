using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Created By: Jonathon Wigley - 10/27/2018
/// Last Edited By: Jonathon Wigley - 02/03/2019
/// </summary>

/// <summary>
/// Manages the individual cells used for selecting dice skins
/// </summary>
public class DiceSkinCell : MonoBehaviour
{
    /// <summary>
    /// Dice skin variable that this cell represents
    /// </summary>
    private DiceSkinData skinData;

    /// <summary>
    /// Reference to the rawimage that displays the die's material preview
    /// </summary>
    [SerializeField] private Image previewImage;

    /// <summary>
    /// Dispatched whenever this button is pressed
    /// </summary>
    /// <param name="sender"></param>
    public delegate void PressedDelegate(DiceSkinData skin);
    public static event PressedDelegate OnPressed;

    /// <summary>
    /// Called by the attached button in editor
    /// </summary>
    public void BTN_HandleButtonPress()
    {
        OnPressed?.Invoke(skinData);
    }

    /// <summary>
    /// Set the dice skin of for this cell
    /// </summary>
    /// <param name="mat"></param>
    public void SetDiceSkin(DiceSkinData skin)
    {
        skinData = skin;
    }
}
