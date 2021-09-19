using System;
using UnityEngine;
using System.Collections;

public class ElectrostaticForceManager : MonoBehaviour {
    // Location of the screen edges in world coordinates (i.e. not pixel coordinates).
    // Calculated from camera projection in Start().
    public float Top;
    public float Bottom;
    public float Left;
    public float Right;
    public float E = 0.1f;
    
    /// <summary>
    /// All the particles in the level
    /// </summary>
    private ParticlePhysics[] particles;
    private ParticlePhysics.AppliedForce[] forces;

    /// <summary>
    /// Initialize screen coordinates
    /// </summary>
    internal void Start()
    {
        var main = Camera.main;
        var tl = main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));
        var br = main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
        Top = tl.y;
        Bottom = br.y;
        Left = tl.x;
        Right = br.x;

        particles = FindObjectsOfType<ParticlePhysics>();
        forces = new ParticlePhysics.AppliedForce[particles.Length];

        for (int i = 0; i < particles.Length; i++)
            forces[i] = particles[i].AddAppliedForce();
    }

    /// <summary>
    /// Check for and resolve collisions.
    /// Called once per physics update.
    /// Note this actually changes particle positions and velocities, so the abstraction barrier
    /// of the particles is leaky.
    /// </summary>
    internal void FixedUpdate()
    {
        // Update forces
        foreach (ParticlePhysics.AppliedForce f in forces)
            f.CurrentForce = Vector2.zero;

        for (int i = 0; i < particles.Length; i++)
        {
            var p = particles[i];
            for (int j = i + 1; j < particles.Length; j++)
            {
                var q = particles[j];
                var force = Force(p, q);
                forces[i].CurrentForce += force;
                forces[j].CurrentForce -= force;
            }
        }

        // Fix things that bounce off the screen
        foreach (var p in particles)
            BounceOffScreenEdge(p);
    }

    /// <summary>
    /// Compute force on p applied by q
    /// </summary>
    /// <param name="p">Particle "experiencing" force</param>
    /// <param name="q">Particle "exerting" force</param>
    /// <returns></returns>
    private Vector2 Force(ParticlePhysics p, ParticlePhysics q)
    {
        var pToq = q.transform.position - p.transform.position;
        var distance = pToq.magnitude;
        var dSquared = distance*distance;
        var electrostatic = - (E * p.Charge*q.Charge/(dSquared*distance));
        var repulsion = Math.Min(1f, 10f * electrostatic/distance);
        return pToq * (electrostatic + repulsion);
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
