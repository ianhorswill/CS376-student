using UnityEngine;

/// <summary>
/// Moves this GameObject to a random location on screen should it move off screen
/// It will try to pick a position that has at least FreeRadius units of free space around it.
/// </summary>
public class Respawner : MonoBehaviour
{
    public float FreeRadius = 10;
    
    /// <summary>
    /// If this is called, we're off screen, so move the object to a new location
    /// and zero out its velocity.
    /// </summary>
    // ReSharper disable once UnusedMember.Local
    void OnBecameInvisible()
    {
        transform.position = SpawnUtilities.RandomFreePoint(FreeRadius);
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }
}
