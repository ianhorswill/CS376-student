using UnityEngine;

/// <summary>
/// Finds random points to spawn objects at
/// </summary>
public static class SpawnUtilities
{
    /// <summary>
    /// World coordinates of the lower-left corner of the screen.
    /// </summary>
    public static Vector2 Min;
    /// <summary>
    /// World coordinates of the upper-right corner of the screen
    /// </summary>
    public static Vector2 Max;

    /// <summary>
    /// Find the bounds of the screen in world coordinates
    /// This is called by the run-time system when we first try to call one of the methods below
    /// </summary>
    static SpawnUtilities()
    {
        Min = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        Max = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }

    /// <summary>
    /// Random point on the screen
    /// </summary>
    public static Vector2 RandomVisiblePoint
        // ReSharper disable once ArrangeObjectCreationWhenTypeEvident
        => new Vector2(Random.Range(Min.x, Max.x),
            Random.Range(Min.y, Max.y));

    /// <summary>
    /// Find a random point on the screen that doesn't have anything within radius units
    /// If it can't find one, it just gives you a random location and you're on your own.
    /// </summary>
    public static Vector2 RandomFreePoint(float radius)
    {
        var position = RandomVisiblePoint;
        for (var i = 0; i < 50 && !PointFree(position, radius); i++)
            position = RandomVisiblePoint;
        return position;
    }

    /// <summary>
    /// Check if the specified point is free of any objects for a distance of radius.
    /// </summary>
    public static bool PointFree(Vector2 position, float radius)
    {
        return Physics2D.CircleCast(position, radius, Vector2.up, 0);
    }
}
