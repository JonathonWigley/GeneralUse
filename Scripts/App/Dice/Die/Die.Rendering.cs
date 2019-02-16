using UnityEngine;

/// <summary>
/// Created By: Jonathon Wigley - 11/02/2018
/// Last Edited By: Jonathon Wigley - 02/03/2019
/// </summary>

/// <summary>
/// Controls rendering effects of the dice
/// </summary>
public partial class Die : PooledObject
{
	new Renderer renderer;

	/// <summary>
	/// Initialize references dealing with the rendering of the dice
	/// </summary>
	protected void GetRenderingReferences()
	{
		renderer = GetComponent<Renderer>();
	}

    /// <summary>
    /// Change the skin of the die
    /// </summary>
    /// <param name="skin">Skin to change to</param>
    public void SetSkin(DiceSkinData skin)
    {
        if(skin == null)
            return;

		renderer.material.SetColor("_DiceColor", skin.BaseColor);
		renderer.material.SetColor("_ASEOutlineColor", skin.OutlineColor);
    }
}
