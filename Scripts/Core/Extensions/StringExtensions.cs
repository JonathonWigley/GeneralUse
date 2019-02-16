/// <summary>
/// Created By: Jonathon Wigley - 10/12/2018
/// Last Edited By: Jonathon Wigley - 10/12/2018
/// </summary>

/// <summary>
/// Collection of extensions for the string class
/// </summary>
public static class StringExtensions
{
	/// <summary>
	/// Removes all characters from the first character of the given string
	/// to the end of this string
	/// </summary>
	/// <param name="str">This string</param>
	/// <param name="startTrimString">String to match that will be used to trim the string</param>
	/// <returns>String with all characters after the trim string removed</returns>
	public static string TrimAfterString(this string str, string startTrimString)
	{
		// Trims the string from the first index of the given character to the end of the string
		if(str.Contains(startTrimString))
			return str.Remove(str.IndexOf(startTrimString)).Trim();

		return str;
	}
}
