
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Assets.FakeUnity
{
    /// <summary>
    /// A game object is mostly just a collection of components
    /// Rather than making subclasses of GameObject and putting special fields and methods in the subclasses,
    /// we make subclasses of Component, and then mix and match components within the one game object class.
    /// </summary>
    public sealed class FakeGameObject
    {
        /// <summary>
        /// Don't let people make their own game objects; they have to use Create().
        /// </summary>
        private FakeGameObject() { }
        
        /// <summary>
        /// Just a string that appears in the editor so you can tell one object from the other
        /// Except that these won't really show up in the editor because the editor only shows
        /// "real" game objects and these are just fake game objects.
        /// </summary>
        public string name = "unnamed game object";

        /// <summary>
        /// An easy way of getting at the Transform component of this object.
        /// </summary>
        public FakeTransform transform => GetComponent<FakeTransform>();
        
        /// <summary>
        /// This is the thing that holds the real list of components
        /// </summary>
        [SerializeField]
        private readonly List<FakeComponent> components = new List<FakeComponent>();

        /// <summary>
        /// Return the component in this game object that is of type T.
        /// </summary>
        /// <typeparam name="T">Type of the component; must be a subclass of Component</typeparam>
        public T GetComponent<T>() where T:FakeComponent => (T)components.First(c => c is T);

        public T AddComponent<T>() where T: FakeComponent, new()
        {
            var component = new T() {gameObject =  this};
            components.Add(component);
            return component;
        }

        /// <summary>
        /// Create a new game object and a transform for it
        /// </summary>
        /// <param name="name">Name for the object</param>
        /// <param name="parent">Parent object in the hierarchy</param>
        /// <returns></returns>
        public static FakeGameObject Create(string name, FakeGameObject parent)
        {
            var o = new FakeGameObject() { name = name };
            var t = o.AddComponent<FakeTransform>();
            if (parent != null)
                parent.transform.AddChild(t);
            else
                t.parent = null;
            return o;
        }

        /// <summary>
        /// This isn't part of unity; it's just to check a GameObject to make sure it seems sane.
        /// </summary>
        public void CheckConsistency()
        {
            var t = transform;
            if (t == null)
                throw new Exception($"GameObject {name} has no transform component");
            
            // Check that all the components realize they're part of this game object
            foreach (var component in components)
                Debug.Assert(component.gameObject == this);
            
            // Now check the children
            for (var i = 0; i < t.GetChildCount(); i++)
                t.GetChild(i).gameObject.CheckConsistency();
        }

        public override string ToString()
        {
            return $"FakeGameObject {name}";
        }
    }
}
