using UnityEngine;

/// <summary>
/// Handles respawning of GameObject after destruction.
/// Respawning is implemented by teleporting to a spawn point
/// </summary>
public class Respawner : MonoBehaviour
{
    /// <summary>
    /// Move the attached GameObject to an unoccupied SpawnPoint.
    /// </summary>
    public void Respawn()
    {
        // Find an unoccupied SpawnPoint
        var spawnPointPose = SpawnPoint.FindFree().transform;

        // Move there and reset our velocity
        var rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.MovePosition(spawnPointPose.position);
        rigidBody.angularVelocity = 0;
        rigidBody.velocity = Vector2.zero;

        // Align our orientation with the SpawnPoint's orientation
        rigidBody.MoveRotation(spawnPointPose.eulerAngles.z);

        // Inform the other components we've respawned
        SendMessage("OnRespawn", SendMessageOptions.DontRequireReceiver);
    }

    /// <summary>
    /// Try to respawn the specified GameObject
    /// </summary>
    /// <param name="o">GameObject to respawn</param>
    /// <returns>True if successful</returns>
    public static bool TryRespawn(GameObject o)
    {
        var r = o.GetComponent<Respawner>();
        if (r)
        {
            r.Respawn();
            return true;
        }
        // This GameObject doesn't have a Respawn component
        return false;
    }
}
