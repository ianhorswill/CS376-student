namespace FakeUnity
{
    /// <summary>
    /// This is just a stub to stand in for the Unity CircleCollider2D component
    /// That's an object that tells the physics system about the collision geometry
    /// of the GameObject that contains it.  It tells the system the object can be
    /// approximated as a circle with the specified radius.
    ///
    /// Since we aren't actually implementing a game engine here, this is just a placeholder.
    /// </summary>
    public class CircleCollider2D : Component
    {
        public float Radius;
    }
}
