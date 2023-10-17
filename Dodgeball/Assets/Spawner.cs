using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// Periodically spawns the specified prefab in a random location.
/// </summary>
public class Spawner : MonoBehaviour
{
    /// <summary>
    /// Object to spawn
    /// </summary>
    public GameObject Prefab;

    /// <summary>
    /// Seconds between spawn operations
    /// </summary>
    public float SpawnInterval = 20;

    /// <summary>
    /// How many units of free space to try to find around the spawned object
    /// </summary>
    public float FreeRadius = 10;

    /// i added this ~ hyunali
    private Vector2 randomPoint;

    private static float lapsedTime = 0;
    /// <summary>
    /// Check if we need to spawn and if so, do so.
    /// </summary>
    // ReSharper disable once UnusedMember.Local
    void Update()
    {
        // TODO
        
        // getting a random point to spawn at
        randomPoint = SpawnUtilities.RandomFreePoint(FreeRadius);
        
        // spawn an orb every 10 seconds
        if (Time.time > lapsedTime)
        {
            // instantiate an orb
            GameObject orb = Instantiate(Prefab, randomPoint, quaternion.identity);
            // orb.transform.position = randomPoint;
            lapsedTime += SpawnInterval;
        }
    }
}
