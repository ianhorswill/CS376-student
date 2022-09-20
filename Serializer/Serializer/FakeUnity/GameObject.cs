
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FakeUnity
{
    /// <summary>
    /// A game object is mostly just a collection of components
    /// Rather than making subclasses of GameObject and putting special fields and methods in the subclasses,
    /// we make subclasses of Component, and then mix and match components within the one game object class.
    /// </summary>
    public sealed class GameObject
    {
        /// <summary>
        /// Don't let people make their own game objects; they have to use Create().
        /// </summary>
        private GameObject() { }
        
        /// <summary>
        /// Just a string that appears in the editor so you can tell one object from the other
        /// Except that these won't really show up in the editor because the editor only shows
        /// "real" game objects and these are just fake game objects.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public string name = "unnamed game object";

        /// <summary>
        /// An easy way of getting at the Transform component of this object.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public Transform transform => GetComponent<Transform>();
        
        /// <summary>
        /// This is the thing that holds the real list of components
        /// </summary>
        [SerializeField]
        private readonly List<Component> components = new List<Component>();

        /// <summary>
        /// Return the component in this game object that is of type T.
        /// </summary>
        /// <typeparam name="T">Type of the component; must be a subclass of Component</typeparam>
        public T GetComponent<T>() where T:Component => (T)components.First(c => c is T);

        public T AddComponent<T>() where T: Component, new()
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
        public static GameObject Create(string name, GameObject parent)
        {
            var o = new GameObject() { name = name };
            var t = o.AddComponent<Transform>();
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
            return $"GameObject {name}";
        }
    }
}
