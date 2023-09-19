using UnityEngine;

/// <summary>
/// Causes the attached GameObject to die and respawn when it hits things
/// </summary>
public class DieOnCollision : MonoBehaviour
{
    /// <summary>
    /// True if player gets a point when this is hit with a missile.
    /// </summary>
    public bool IsPlayerTarget;

    /// <summary>
    /// Called each time the attached GameObject's Collider(s) initiate contact with another Collider
    /// </summary>
    /// <param name="obstacle">Information about what was collided with</param>
    internal void OnCollisionEnter2D(Collision2D obstacle)
    {
        if (IsPlayerTarget && obstacle.collider.tag == "Missile")
            FindObjectOfType<ScoreKeeper>().ScoreKill();

        if (!Respawner.TryRespawn(gameObject))
            Destroy(gameObject);
    }
}
