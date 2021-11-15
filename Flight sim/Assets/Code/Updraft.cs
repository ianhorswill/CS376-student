using UnityEngine;

/// <summary>
/// Placeholder component to mark an objecct as an updraft
/// </summary>
public class Updraft : MonoBehaviour {
    /// <summary>
    /// Wind velocity of wind in the updraft
    /// </summary>
    [Header("Wind within the updraft")]
    public Vector3 WindVelocity = new Vector3(0, 10, 0);
}
