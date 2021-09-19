using System;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Implements the physics for a particle
/// Updates position and velocity based on forces.  Note that constraints and springs are implemented
/// by the Spring component and that collisions physics is implemented by the CollisionManager component.
/// </summary>
public class ParticlePhysics : MonoBehaviour
{
    public Vector2 Velocity
    {
        get
        {
            switch (PhysicsParameters.Integrator)
            {
                    case IntegratorType.Euler:
                    return eulerVelocity;

                    case IntegratorType.Verlet:
                    return ((Vector2)transform.position - previousPosition)/Time.fixedDeltaTime;

                default:
                    throw new InvalidOperationException("Invalid setting for Integrator: "+ PhysicsParameters.Integrator);
            }
        }
        set
        {
            switch (PhysicsParameters.Integrator)
            {
                case IntegratorType.Euler:
                    eulerVelocity = value;
                    break;

                case IntegratorType.Verlet:
                    previousPosition = (Vector2) transform.position - Time.fixedDeltaTime*value;
                    break;

                default:
                    throw new InvalidOperationException("Invalid setting for Integrator: " +
                                                        PhysicsParameters.Integrator);
            }
        }
    }
    public float Mass;
    public float Radius;
    public float Charge;
    
    // Verlet integrator state
    Vector2 previousPosition;

    // Euler integrator state
    Vector2 eulerVelocity;

    /// <summary>
    /// Forces applied to this particle
    /// </summary>
    readonly List<AppliedForce> appliedForces = new List<AppliedForce>();

    /// <summary>
    /// Holds the force that some other component is applying to this particle
    /// </summary>
    public class AppliedForce
    {
        public Vector2 CurrentForce;
    }

    /// <summary>
    /// Tells the particle about a new force to be applied.
    /// Called once; returns an AppliedForce object that you can
    /// update with the force value as you like
    /// </summary>
    /// <returns>AppliedForce object - update this with the value of the force.</returns>
    public AppliedForce AddAppliedForce()
    {
        var f = new AppliedForce();
        appliedForces.Add(f);
        return f;
    }

	// Called at level load time
	internal void Start ()
	{
	    var circle = GetComponent<Circle>();
        Debug.Assert(circle != null);
	    // ReSharper disable once PossibleNullReferenceException
	    Radius = circle.Radius;

	    // ReSharper disable once CompareOfFloatsByEqualityOperator
	    if (Mass == 0)
	        // Autocompute
	        Mass = Radius*Radius;

        // Initialize Verlet integrator
	    previousPosition = transform.position;
	}
	
	/// <summary>
    /// Update physics state
    /// </summary>
	internal void FixedUpdate ()
	{
	    // Compute force
	    var force = new Vector2(0, -PhysicsParameters.G);
	    foreach (var f in appliedForces)
	        force += f.CurrentForce;
         
        // Integrate
	    float deltaT = Time.fixedDeltaTime;
        Vector2 oldPosition = transform.position;
	    Vector2 newPosition;

        switch (PhysicsParameters.Integrator)
	    {
	        case IntegratorType.Euler:
	            eulerVelocity += deltaT*force/Mass;
	            newPosition = transform.position + (Vector3) (deltaT*eulerVelocity);
	            break;

	        case IntegratorType.Verlet:
	            newPosition = 2*oldPosition - previousPosition + force*deltaT*deltaT/Mass;
	            break;

	        default:
	            throw new InvalidOperationException("Invalid setting for Integrator: " + PhysicsParameters.Integrator);
	    }

	    if (PhysicsParameters.OneD)
	        newPosition.y = oldPosition.y;

	    transform.position = newPosition;
        previousPosition = oldPosition;
    }

    /// <summary>
    /// Draw debug graphics
    /// </summary>
    internal void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, transform.position + 0.2f*(Vector3) Velocity);
        }
    }

    /// <summary>
    /// Test if the particle contains a given point
    /// </summary>
    /// <param name="position">Point to check</param>
    /// <returns>True if the point is inside the particle.</returns>
    public bool Overlaps(Vector2 position)
    {
        return Vector2.Distance(position, transform.position) < Radius;
    }
}
