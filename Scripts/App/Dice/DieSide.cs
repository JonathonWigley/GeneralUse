using UnityEngine;

/// <summary>
/// Created By: Jonathon Wigley - 4/8/18
/// Last Edited By: Jonathon Wigley - 10/12/2018
///
/// Attaches to each side trigger of a die and detects if its on the ground or not
/// Also contains the value of the given trigger for the die
/// </summary>

public class DieSide : MonoBehaviour
{
    /// <summary>
    /// Value of this side
    /// </summary>
    public int RollValue { get; private set; }

    /// <summary>
    /// True when this side is touching the ground
    /// </summary>
    public bool bOnGround { get; private set; }

    private void Awake()
    {
        // Get the value of the die from the name
        RollValue = ParseValueFromName();
    }

    private void OnTriggerEnter(Collider other)
    {
        // This side's trigger is touching the ground
        if("Ground" == LayerMask.LayerToName(other.gameObject.layer))
        {
            bOnGround = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // This side's trigger is stopped touching the ground
        if("Ground" == LayerMask.LayerToName(other.gameObject.layer))
        {
            bOnGround = false;
        }
    }

    /// <summary>
    /// Calculate the distance from this side to the ground
    /// </summary>
    /// <returns>Distance from the side to the ground, positive infinity if ground was not hit</returns>
    public float GetDistanceFromGround()
    {
        // Cast from the side to the ground
        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, 50f, LayerMask.GetMask("Ground")))
        {
            return Vector3.Distance(transform.position, hit.point);
        }

        // Return infinity if nothing was hit
        return float.PositiveInfinity;
    }

    // Get the side value based on the name of the die side
    // The side value should be contained at the end of a name after an underscore, EX: Decahedron_Trigger_01 would get value 1
    private int ParseValueFromName()
    {
        string name = gameObject.name;
        int lastUnderscoreIndex = name.LastIndexOf('_');
        string number = name.Substring(lastUnderscoreIndex + 1);
        return int.Parse(number);
    }
}