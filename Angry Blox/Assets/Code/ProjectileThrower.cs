using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls slingshot-style launching of projectile.
/// This component lives inside the projectile, not the catapult.
/// </summary>
public class ProjectileThrower : MonoBehaviour {

    /// <summary>
    /// Cached copy of the RigidBody2D so we don't have to keep looking it up.
    /// GetComponent used to be slow; Unity claims its fast now and that we don't have
    /// to do this kind of caching, but their documentation is inconsistent.  So we'll cache.
    /// </summary>
    private Rigidbody2D myRigidBody;
    /// <summary>
    /// The spring attached the projectile to the catapult base
    /// </summary>
    private SpringJoint2D springJoint; 
    /// <summary>
    /// Where the spring attaches to the catapult.
    /// Initialized to the initial location of the projectile.
    /// </summary>
    private Vector3 springAttachmentPoint;

    /// <summary>
    /// Where we are in the ready-aim-fire sequence.
    /// </summary>
    FiringState firingState = FiringState.Idle;

    enum FiringState { Idle, Aiming, Firing }

    /// <summary>
    /// Position of a GameObject in screen coordinates
    /// </summary>
    /// <param name="o"></param>
    /// <returns></returns>
    Vector2 ScreenPosition(GameObject o)
    {
        // Project through the main (only) camera to get screen coordinates
        return Camera.main.WorldToScreenPoint(o.transform.position);
    }

    /// <summary>
    /// True if gameobject is off the screen
    /// </summary>
    bool IsOffScreen(GameObject o)
    {
        var pos = ScreenPosition(o);
        return pos.x < 0 || pos.y < 0 || pos.x > Screen.width || pos.y > Screen.height;
    }

    /// <summary>
    /// True if we are still waiting for this object to finish falling or settling.
    /// </summary>
    /// <param name="o">GameObject</param>
    /// <returns></returns>
    bool IsActive(GameObject o)
    {
        return !IsOffScreen(o) && o.GetComponent<Rigidbody2D>().IsAwake();
    }

    bool IsActive(Rigidbody2D rb)
    {
        return IsActive(rb.gameObject);
    }

    /// <summary>
    /// True when we're still waiting for things to stop flying around
    /// </summary>
    /// <returns></returns>
    bool WaitingForPhysicsToSettle()
    {
        return true;  // Replace this
    }

    /// <summary>
    /// Initialize component
    /// </summary>
    internal void Start() {
        myRigidBody = GetComponent<Rigidbody2D>();
        springJoint = GetComponent<SpringJoint2D>();
        springAttachmentPoint = transform.position;
    }

    internal void Update()
    {
        FireControl();
    }

    /// <summary>
    /// Reload the current level
    /// </summary>
    private void ResetForFiring()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void FireControl()
    {
        switch (firingState)
        {
            case FiringState.Idle:
                if (Input.GetMouseButtonDown(0))
                {
                    // click to pull back
                    firingState = FiringState.Aiming;
                }
                break;

            case FiringState.Aiming:
                MoveProjectileToMousePosition();
                if (Input.GetMouseButtonUp(0))
                {
                    // Release the slingshot
                    firingState = FiringState.Firing;
                    // we damp it when we're pulling back so that it doesn't oscillate
                    // Now turn this off to let it accelerate
                    springJoint.dampingRatio = 0f;
                }
                break;

            case FiringState.Firing:
                if (transform.position.x >= springAttachmentPoint.x)
                {
                    springJoint.enabled = false;
                    // if we're close enough to the center, turn off the spring (so that the projectile flies)
                    GetComponent<DistanceJoint2D>().enabled = false; // also turn off the distance lock
                }
                break;
        }
    }

    void MoveProjectileToMousePosition() {
        // find where the mouse is, and convert that to world coordinates
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var offset = Vector3.ClampMagnitude(mousePos - transform.position, 5f); // find the difference and clamp its magnitude
        myRigidBody.MovePosition(transform.position + offset);
    }
}
