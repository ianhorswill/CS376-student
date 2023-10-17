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
       Destroy(gameObject); // self-destruct when off screen
    } 

    /// <summary>
    /// If this is called, then we hit something
    /// Destroy ourselves unless the thing we hit was another Orb.
    /// </summary>
    // ReSharper disable once UnusedMember.Local
    void OnCollisionEnter2D(Collision2D collision)
    {
        // TODO
        if (collision.collider.GetComponent<Orb>() ==  null) // get component return the component or null
        {   // if we get the orb component, ignore
            // destroy on if the thing i hit was anything other than an orb
            Destroy(gameObject);
        }
    }
}
