using UnityEngine;

/// <summary>
/// Even handler for Orb objects
/// </summary>
public class Orb : MonoBehaviour
{
    /// <summary>
    /// If this gets called, then we're off screen
    /// So destroy ourselves
    /// </summary>
    // ReSharper disable once UnusedMember.Local
    void OnBecameInvisible()
    {
        // TODO
    }

    /// <summary>
    /// If this is called, then we hit something
    /// Destroy ourselves unless the thing we hit was another Orb.
    /// </summary>
    // ReSharper disable once UnusedMember.Local
    void OnCollisionEnter2D(Collision2D collision)
    {
        // TODO
    }
}
