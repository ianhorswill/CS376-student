using System.Linq;
using UnityEngine;

/// <summary>
/// Causes the attached GameObject to respond to gravitational attraction of the stars.
/// </summary>
public class OrbitalGravity : MonoBehaviour
{
    /// <summary>
    /// Holds the Transform components for the stars.
    /// The Transform component of an object holds its position.
    /// </summary>
    Transform[] starTransforms;
    // ReSharper disable once InconsistentNaming
    private Rigidbody2D rigidbody2d;
    /// <summary>
    /// Strength of the gravitational force
    /// </summary>
    public float GravityForce = 3f;

    /// <summary>
    /// Called at level startup
    /// </summary>
    internal void Start()
    {
        starTransforms = GameObject.FindGameObjectsWithTag("Star").Select(star => star.transform).ToArray();
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Called once for each physics update step
    /// </summary>
    internal void FixedUpdate()
    {
        rigidbody2d.AddForce(GravitationalForce);
    }

    /// <summary>
    /// Total gravitational force exerted by all the stars.
    /// </summary>
    public Vector3 GravitationalForce
    {
        get
        {
            var force = Vector3.zero;
            foreach (var starTransform in starTransforms)
            {
                var offset = starTransform.position - transform.position;
                var scale = Mathf.Max(0.001f, offset.sqrMagnitude);
                force += offset.normalized*(GravityForce/scale);
            }
            return force;
        }
    }

    /// <summary>
    /// Visualize the force vectors of stars
    /// </summary>
    internal void OnDrawGizmos()
    {
        if (starTransforms == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position+GravitationalForce);
    }
}
