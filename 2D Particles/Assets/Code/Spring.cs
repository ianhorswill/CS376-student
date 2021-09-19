using System;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Implements spring forces between two particles, or hard constraint (infinite stiffness)
///  if in Verlet mode and PhysicsParameters.SpringsAreConstraints is set.
/// </summary>
[ExecuteInEditMode]
public class Spring : MonoBehaviour
{
    /// <summary>
    /// Particle attached to spring
    /// </summary>
    public ParticlePhysics Particle1;
    /// <summary>
    /// Other particle attached to spring
    /// </summary>
    public ParticlePhysics Particle2;

    /// <summary>
    /// Initial length of spring; produces no force when it is this length.
    /// </summary>
    private float restingLength;
    /// <summary>
    /// AppliedForce objects for the two particles.
    /// </summary>
    private ParticlePhysics.AppliedForce p1Force, p2Force;

    /// <summary>
    /// Spring should act as a hard constraint (infinite stiffness).
    /// </summary>
    static bool SpringsAreConstraints
    {
        get
        {
            return PhysicsParameters.SpringsAreConstraints
                && PhysicsParameters.Integrator == IntegratorType.Verlet;
        }
    }

    private static bool ShowConstraints => PhysicsParameters.ShowConstraints;

	/// <summary>
    /// Called at startup time.
    /// </summary>
	internal void Start ()
	{
	    restingLength = Vector3.Distance(Particle1.transform.position, Particle2.transform.position);
	    if (!SpringsAreConstraints)
	    {
	        p1Force = Particle1.AddAppliedForce();
	        p2Force = Particle2.AddAppliedForce();
	    }
	}
	
	/// <summary>
	/// Recompute forces applied to particles
    /// Called once per physics update.
    /// If in constraint mode, then directly resets positions of particles.
    /// </summary>
	internal void FixedUpdate ()
	{
	    var p1 = Particle1.transform.position;
	    var p2 = Particle2.transform.position;
	    var delta = p1 - p2;
	    var length = delta.magnitude;
	    var deltaNorm = delta/length;
	    if (SpringsAreConstraints)
	    {
	        //var v1 = Particle1.Velocity;
	        //var v2 = Particle2.Velocity;
	        var adjustment = 0.5f*(length - restingLength)*deltaNorm;
	        Particle1.transform.position -= adjustment;
            Particle2.transform.position += adjustment;
	        //Particle1.Velocity = v1;
	        //Particle2.Velocity = v2;
	    }
	    else
	    {
	        Vector2 springForce = deltaNorm*(restingLength - length)*PhysicsParameters.K;
	        var relativeVelocity = Particle1.Velocity - Particle2.Velocity;
	        Vector2 dampingForce = -deltaNorm*Vector2.Dot(relativeVelocity, deltaNorm)*PhysicsParameters.Damping;
	        var force = springForce + dampingForce;

	        p1Force.CurrentForce = force;
	        p2Force.CurrentForce = -force;
	    }
	}

    [MenuItem("Particles/Connect with springs")]
    public static void AttachParticles()
    {
        var selected = Selection.gameObjects;
        for (int i = 0; i < selected.Length; i++)
        {
            var pi = selected[i].GetComponent<ParticlePhysics>();
            if (pi != null)
                for (int j = i + 1; j < selected.Length; j++)
                {
                    var pj = selected[j].GetComponent<ParticlePhysics>();
                    if (pj != null)
                        Link(pi, pj);
                }
        }
    }

    private static void Link(ParticlePhysics p1, ParticlePhysics p2)
    {
        var springGameObject = new GameObject("Spring");
        // This is how you programmatically put one gameobject inside another
        springGameObject.transform.parent = p1.transform;
        var spring = springGameObject.AddComponent<Spring>();
        spring.Particle1 = p1;
        spring.Particle2 = p2;
    }

    /// <summary>
    /// Shader to use to draw springs
    /// </summary>
    private static Material springMaterial;

    /// <summary>
    /// Draw the line representing the spring.
    /// </summary>
    internal void OnRenderObject()
    {
        if (!ShowConstraints)
            return;

        if (!springMaterial)
        {
            springMaterial = new Material(Shader.Find("Unlit/Color"))
            {
                color = Selection.Contains(gameObject) ? Color.green : Color.yellow
            };
        }

        // This is spectacularly inefficient.
        // Should batch the lines from all the springs at some point.
        springMaterial.SetPass(0);
        GL.PushMatrix();
        GL.MultMatrix(transform.localToWorldMatrix);
        GL.Begin(GL.LINES);
        Vector2 p1 = Particle1.transform.position;
        Vector2 p2 = Particle2.transform.position;

        if (SpringsAreConstraints)
        {
            // Nice and simple - draw one line
            GL.Vertex(p1);
            GL.Vertex(p2);
        }
        else
        {
            // Huge pain.
            // Draw a zig-zag line to represent the coil of the spring, including the traditional
            // straight segments on either end.
            int coilCount = Math.Max((int) ((restingLength - 0.2f)*8), 8);
            int coilLines = coilCount + 2;
            float totalLines = coilLines + 2;
            Vector2 delta = p2 - p1;
            var length = delta.magnitude;
            var direction = delta/length;
            var perp = 0.1f*new Vector2(-direction.y, direction.x);
            var previous = p1 + delta*(1/totalLines);

            // First line (straight)
            GL.Vertex(p1);
            GL.Vertex(previous);

            // Draw all of the coil except for the last segment
            for (int i = 0; i < coilLines - 1; i++)
            {
                // Draw a coil line
                GL.Vertex(previous);
                var newV = p1 + delta*((i + 2)/totalLines) + (2*(i & 1) - 1)*perp;
                GL.Vertex(newV);
                previous = newV;
            }

            // Last coil line
            var penultimateVertex = p2 - delta*(1/totalLines);
            GL.Vertex(previous);
            GL.Vertex(penultimateVertex);

            // Last line (straight)
            GL.Vertex(penultimateVertex);
            GL.Vertex(p2);
        }
        GL.PopMatrix();
        GL.End();
    }
}
