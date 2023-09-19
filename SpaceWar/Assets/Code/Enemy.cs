using UnityEngine;

/// <summary>
/// Adds (bad) automatic moving, steering, and shooting to an attached game object
/// </summary>
[RequireComponent(typeof(MissileLauncher), typeof(Rigidbody2D), typeof(OrbitalGravity))]
public class Enemy : Ship
{
    /// <summary>
    /// Our OrbitalGravity component
    /// It applies gravity to the ship by itself, but the steering code here needs to
    /// talk to the OrbitalGravity object to ask what direction gravity is pointed in.
    /// </summary>
    private OrbitalGravity gravity;
    /// <summary>
    /// Time of the last Missile launch
    /// </summary>
    private float lastMissileLaunch;

    /// <summary>
    /// How fast the ship accelerates 
    /// </summary>
    public float ForwardThrustStrength = 10;
    /// <summary>
    /// How fast the ship steers
    /// </summary>
    public float SteeringThrustStrength = 20;

    /// <summary>
    /// How many seconds between missile launches
    /// </summary>
    public float FiringPeriod = 5;

    /// <summary>
    /// Find the rest of our components.
    /// Called at startup.
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    internal override void Start()
    {
        base.Start();
        gravity = GetComponent<OrbitalGravity>();
    }

    /// <summary>
    /// Steer and thrust to counter the force of gravity
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    internal void FixedUpdate()
    {
        // Get the local gravitational force
        var gForce = gravity.GravitationalForce;
        // Figure out where we're pointed
        var forward = transform.TransformDirection(new Vector3(1,0,0));
        // Figure out how far off our heading is
        var angularError = AngleBetween(forward, gForce);
        // Steer to point away from the gravitational force
        rigidBody.angularVelocity = SteeringThrustStrength*angularError;

        if (gForce.magnitude > 1 && Mathf.Abs(angularError)<20)
            rigidBody.AddForce(ForwardThrustStrength * forward);
    }

    static float AngleBetween(Vector2 v1, Vector2 v2)
    {
        float difference = Direction(v1) - Direction(v2);
        if (difference > Mathf.PI)
            difference -= 2*Mathf.PI;
        if (difference < -Mathf.PI)
            difference += 2*Mathf.PI;
        return difference;
    }
    static float Direction(Vector2 v)
    {
        return Mathf.Atan2(v.y, v.x);
    }

    /// <summary>
    /// Fire periodically and respawn when we leave the screen
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    internal void Update()
    {
        if (Time.time - lastMissileLaunch > FiringPeriod)
        {
            GetComponent<MissileLauncher>().FireMissile();
            lastMissileLaunch = Time.time;
        }

        var screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPosition.x < -10
            || screenPosition.y < -10
            || screenPosition.y > Screen.height + 10
            || screenPosition.x > Screen.width + 10)
            Respawner.TryRespawn(gameObject);
    }
}
