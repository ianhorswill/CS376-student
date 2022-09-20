namespace FakeUnity
{
    /// <summary>
    /// The component class is where all the real work gets done
    /// Components implement kinds of functionality for rendering, physics, NPC behavior, etc.
    /// Then they get grouped together into game objects
    /// </summary>
    public abstract class Component
    {
        /// <summary>
        /// The game object to which this belongs
        /// </summary>
        public GameObject gameObject;
        
        /// <summary>
        /// These are a bunch of methods that can be called on components.
        /// There here so you can see them, but they won't be used in this assignment.
        /// </summary>
        public virtual void Start() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
    }
}
