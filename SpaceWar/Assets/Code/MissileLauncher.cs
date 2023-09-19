using UnityEngine;

/// <summary>
/// Adds ability to fire missiles.
/// Actual firing decisions are made in the Enemy (for enemy ship) KeyboardControl (for player) components.
/// </summary>
public class MissileLauncher : MonoBehaviour
{
    /// <summary>
    /// Prefab for missiles to fire
    /// </summary>
    public GameObject MissilePrefab;

    /// <summary>
    /// Initial speed of the missile
    /// </summary>
    public float MissileSpeed = 1;


    /// <summary>
    /// Shoot
    /// </summary>
    public void FireMissile()
    {
        // Make a new missile
        GameObject missile = Instantiate(MissilePrefab,
            // Place it 0.5 units in front of us
            transform.TransformPoint(new Vector3(0.5f, 0, 0)),
            // Point it in the same direction we're pointed in
            transform.rotation);
        var myVelocity = GetComponent<Rigidbody2D>().velocity;
        var missileRigidBody = missile.GetComponent<Rigidbody2D>();
        missileRigidBody.velocity = myVelocity + MissileVelocity;
        // Make it spin because it looks cool.
        missileRigidBody.angularVelocity = 800;
    }

    /// <summary>
    /// A vector that's MissileSpeed units, but pointing in our forward direction
    /// </summary>
    private Vector2 MissileVelocity
    {
        get { return transform.TransformDirection(new Vector3(MissileSpeed, 0, 0)); }
    }

    /// <summary>
    /// Visualizes missile launch point
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    internal void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + MissileVelocity);
    }
}