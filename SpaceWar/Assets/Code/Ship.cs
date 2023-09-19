using UnityEngine;

/// <summary>
/// Base class for ships (children are Enemy and Player)
/// </summary>
public abstract class Ship : MonoBehaviour
{
    /// <summary>
    /// Our RigidBody component.
    /// RigidBody components let us tell physics what forces to apply to an object.
    /// </summary>
    protected Rigidbody2D rigidBody;
    
    /// <summary>
    /// Find the rest of our components.
    /// Called at startup.
    /// </summary>
    internal virtual void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }
}