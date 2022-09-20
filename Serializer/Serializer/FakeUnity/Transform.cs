using System.Collections.Generic;
using System.Diagnostics;

namespace FakeUnity
{
    /// <summary>
    /// Placeholder for the Unity Transform component class
    /// A transform specifies the translation, rotation, and scale to apply to a game object
    /// relative to its parent Transform.  Every GameObject has its own Transform, whether
    /// you need it or not.  And if a GameObject X has children (other GameObjects inside it)
    /// then X's transform has the transforms of the children inside it.
    /// </summary>
    public class Transform : Component
    {
        /// <summary>
        /// X coordinate of the object on screen
        /// In the real unity Transform object, the X and Y coordinates are packaged in a single vector called position.
        /// </summary>
        public float X;
        /// <summary>
        /// X coordinate of the object on screen
        /// In the real unity Transform object, the X and Y coordinates are packaged in a single vector called position.
        /// </summary>
        public float Y;

        /// <summary>
        /// The transform of our parent GameObject
        /// </summary>
        public Transform parent;
        /// <summary>
        /// The transforms of our child gameobjects
        /// </summary>
        [SerializeField] private List<Transform> children = new List<Transform>();

        /// <summary>
        /// Tell how many "child" transforms are inside this one.  That's essentially the
        /// same as how many GameObjects are inside this Transform's GameObject.
        /// </summary>
        public int GetChildCount() => children.Count;

        /// <summary>
        /// Get the child transform with the specified index (0 to GetChildCount()-1).
        /// You usually don't actually want the transform, you want the gameobject the
        /// transform belongs to.  So the way you get the ith child of a gameobject go
        /// is to say: go.transform.GetChild(i).gameObject
        /// </summary>
        public Transform GetChild(int index) => children[index];

        /// <summary>
        /// Add a new child.
        /// </summary>
        internal void AddChild(Transform t)
        {
            Debug.Assert(t.parent == null);
            children.Add(t);
            t.parent = this;
        }
    }
}
