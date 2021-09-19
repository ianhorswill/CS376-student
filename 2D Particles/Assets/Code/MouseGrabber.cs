using UnityEngine;

/// <summary>
/// Lets user grab and throw a particle with the mouse.
/// </summary>
public class MouseGrabber : MonoBehaviour
{
    public bool TapToApplyImpulse = false;
    public Vector2 VelocityChange;

    ParticlePhysics draggedParticle;
    float lastMouseTime;
    Vector2 lastMousePosition;

    /// <summary>
    /// Handle mouse events
    /// </summary>
    internal void OnGUI()
    {
        switch (Event.current.type)
        {
            case EventType.MouseDown:
                if (TapToApplyImpulse)
                {
                    var p = SelectedParticle();
                    if (p != null)
                        p.Velocity = p.Velocity + VelocityChange;
                }
                else
                {
                    StartDrag();
                    UpdateMousePosition();
                }
                break;

            case EventType.MouseMove:
                UpdateMousePosition();
                break;


            case EventType.MouseUp:
                EndDrag();
                break;
        }
    }

    private void UpdateMousePosition()
    {
        lastMouseTime = Time.time;
        lastMousePosition = MouseWorldPosition;
    }

    /// <summary>
    /// Stick particle to mouse.
    /// This needs to be in FixedUpdate() rather than Update() because
    /// it needs to be run every time physics updates particle positions.
    /// </summary>
    internal void FixedUpdate()
    {
        if (draggedParticle != null)
        {
            var position = MouseWorldPosition;

            if (PhysicsParameters.OneD)
                position.y = draggedParticle.transform.position.y;
            draggedParticle.transform.position = position;
            draggedParticle.Velocity = Vector2.zero;
        }
    }

    /// <summary>
    /// The mouse is up, let go of the particle, and set its velocity to the mouse velocity.
    /// </summary>
    private void EndDrag()
    {
        if (draggedParticle != null)
        draggedParticle.Velocity = MouseVelocity;
        draggedParticle = null;
    }

    /// <summary>
    /// Grab a particle and start dragging it.
    /// </summary>
    private void StartDrag()
    {
        draggedParticle = SelectedParticle();
    }

    private ParticlePhysics SelectedParticle()
    {
        var position = MouseWorldPosition;
        foreach (var p in FindObjectsOfType<ParticlePhysics>())
        {
            if (p.Overlaps(position))
                return p;
        }

        return null;
    }

    /// <summary>
    /// Where the mouse is in 2D world coordinates.
    /// </summary>
    private static Vector2 MouseWorldPosition
    {
        get { return Camera.main.ScreenToWorldPoint(Input.mousePosition); }
    }

    /// <summary>
    /// Speed of the mouse in 2D world coordinates.
    /// </summary>
    Vector2 MouseVelocity
    {
        get { return (MouseWorldPosition - lastMousePosition)/(Time.time - lastMouseTime); }
    }
}
