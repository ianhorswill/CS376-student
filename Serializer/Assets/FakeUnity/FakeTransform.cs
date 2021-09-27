using System.Collections.Generic;
using System.Diagnostics;

namespace Assets.FakeUnity
{
    public class FakeTransform : FakeComponent
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
        public FakeTransform parent;
        /// <summary>
        /// The transforms of our child gameobjects
        /// </summary>
        [SerializeField] private List<FakeTransform> children = new List<FakeTransform>();


        public int GetChildCount() => children.Count;

        public FakeTransform GetChild(int index) => children[index];

        internal void AddChild(FakeTransform t)
        {
            Debug.Assert(t.parent == null);
            children.Add(t);
            t.parent = this;
        }
    }
}
