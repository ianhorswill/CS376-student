using UnityEngine;

/// <summary>
/// Handles collisions particle/particle and particle/edge-of-screen collisions
/// </summary>
public class CollisionManager : MonoBehaviour
{
    // Location of the screen edges in world coordinates (i.e. not pixel coordinates).
    // Calculated from camera projection in Start().
    public float Top;
    public float Bottom;
    public float Left;
    public float Right;

    /// <summary>
    /// All the particles in the level
    /// </summary>
    ParticlePhysics[] particles;

    /// <summary>
    /// Total kinetic energy of all particles
    /// </summary>
    public float KineticEnergy;

	/// <summary>
    /// Initialize screen coordinates
    /// </summary>
	internal void Start ()
	{
	    var main = Camera.main;
	    var tl = main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));
        var br = main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
        Top = tl.y;
	    Bottom = br.y;
	    Left = tl.x;
	    Right = br.x;

	    particles = FindObjectsOfType<ParticlePhysics>();
	}
	
	/// <summary>
    /// Check for and resolve collisions.
    /// Called once per physics update.
    /// Note this actually changes particle positions and velocities, so the abstraction barrier
    /// of the particles is leaky.
    /// </summary>
	internal void FixedUpdate () {
	    foreach (var p in particles)
	        BounceOffScreenEdge(p);
	    HandleParticleCollisions();
	    float ke = 0;
	    foreach (var p in particles)
	        ke += 0.5f*p.Mass + p.Velocity.sqrMagnitude;
	    KineticEnergy = ke;
	}

    internal void OnGUI()
    {
        GUI.Label(new Rect(100, 100, 300, 50), "Kinetic energy: "+KineticEnergy);
    }

    /// <summary>
    /// Check for and handle particle/particle collisions
    /// </summary>
    private void HandleParticleCollisions()
    {
        for (int i = 0; i < particles.Length; i++)
        {
            var pi = particles[i];
            var posi = pi.transform.position;
            for (int j = i + 1; j < particles.Length; j++)
            {
                var pj = particles[j];
                var posj = pj.transform.position;
                var delta = posi - posj;
                var distance = delta.magnitude;
                var minimumOffset = pi.Radius + pj.Radius;
                if (distance < minimumOffset)
                {
                    // Collision!

                    // Remember the velocities of the particles before we change the positions
                    // or the Verlet integrator will get confused.
                    var piv = pi.Velocity;
                    var pjv = pj.Velocity;

                    // First, separate the particles
                    // Figure out how far they need to move apart, then move each particle half that amount
                    // This is very approximate.  A physics engine that cared about accuracy would be fancier.
                    var separationDirection = delta/Mathf.Max(distance, 0.00001f); // take the max to avoid dividing by zero if we're unluck enough for the particles to exactly align
                    var halfSeparationNeeded = 0.5f*(minimumOffset-distance)*separationDirection;
                    // Update positions
                    posi += halfSeparationNeeded;
                    posj -= halfSeparationNeeded;
                    pi.transform.position = posi;
                    pj.transform.position = posj;

                    // Now update the velocities
                    if (PhysicsParameters.InelasticParticleCollisions)
                    {
                        pi.Velocity = pj.Velocity = Vector2.zero;
                    }
                    else
                    {
                        var totalMass = pi.Mass + pj.Mass;
                        var deltaV = piv - pjv;
                        var distSq = distance*distance;
                        var normalImpulse = (Vector2.Dot(deltaV, delta)/distSq)*(Vector2) delta;
                        pi.Velocity = piv - (2*pj.Mass/totalMass)*normalImpulse;
                        pj.Velocity = pjv + (2*pi.Mass/totalMass)*normalImpulse;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Check for and handle particle running off screen
    /// </summary>
    /// <param name="p">Particle to check</param>
    private void BounceOffScreenEdge(ParticlePhysics p)
    {
        var pos = p.transform.position;
        if (pos.x < Left + p.Radius)
        {
            var v = p.Velocity;
            //pos.x = 2 * (Left + p.Radius) - pos.x;
            pos.x = Left + p.Radius;
            p.transform.position = pos;
            if (PhysicsParameters.InelasticScreenCollisions)
                p.Velocity = Vector2.zero;
            else
            {
                v.x = -v.x;
                p.Velocity = v;
            }
        }

        if (pos.x > Right - p.Radius)
        {
            var v = p.Velocity;
            //pos.x = 2 * (Right - p.Radius) - pos.x;
            pos.x = Right - p.Radius;
            p.transform.position = pos;
            if (PhysicsParameters.InelasticScreenCollisions)
                p.Velocity = Vector2.zero;
            else
            {
                v.x = -v.x;
                p.Velocity = v;
            }
        }

        if (pos.y < Bottom + p.Radius)
        {
            var v = p.Velocity;
            //pos.y = 2*(Bottom + p.Radius) - pos.y;
            pos.y = Bottom + p.Radius;
            p.transform.position = pos;
            if (PhysicsParameters.InelasticScreenCollisions)
                p.Velocity = Vector2.zero;
            else
            {
                v.y = -v.y;
                p.Velocity = v;
            }
        }

        if (pos.y > Top - p.Radius)
        {
            var v = p.Velocity;
            //pos.y = 2 * (Top - p.Radius) - pos.y;
            pos.y = Top - p.Radius;
            p.transform.position = pos;
            if (PhysicsParameters.InelasticScreenCollisions)
                p.Velocity = Vector2.zero;
            else
            {
                v.y = -v.y;
                p.Velocity = v;
            }
        }

    }
}
