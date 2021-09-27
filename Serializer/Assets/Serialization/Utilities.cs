using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Assets.FakeUnity;

namespace Assets.Serialization
{
    public static class Utilities
    {
        /// <summary>
        /// Find all the fields that are to be serialized in this object, along with their values
        /// </summary>
        /// <param name="o">Object you want to serialize</param>
        /// <returns>A series of KeyValuePairs, with .Key set to the name of the field, and .Value set to the field's value</returns>
        public static IEnumerable<KeyValuePair<string, object>> SerializedFields(object o)
        {
            // Crawl up the type hierarchy, starting with o's type
            for (var type = o.GetType(); type != null && type != typeof(object); type = type.BaseType)
                // Find all the fields declared in that particular type
            {
                var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
                foreach (var f in fields)
                    // Check if that field needs to be serialized
                    if (f.IsPublic || f.GetCustomAttribute<SerializeField>() != null)
                        // If so, tell our caller about it
                        yield return new KeyValuePair<string, object>(f.Name, f.GetValue(o));
            }
        }

        /// <summary>
        /// Update the value of a field in an object given the name of the field as a string.
        /// </summary>
        /// <param name="o">Object whose field should be changed</param>
        /// <param name="fieldName">Name of the field</param>
        /// <param name="newValue">New value to write into the field</param>
        public static void SetFieldByName(object o, string fieldName, object newValue)
        {
            // What type is the object?
            var t = o.GetType();
            // Get the object's field with the specified name
            var f = t.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (f == null)
                throw new MissingFieldException(t.Name, fieldName);
            // For a real system, we'd do a more general version of this
            // that would handle any element type.  But it's hairier to
            // do and not necessary for this assignment.
            if (f.FieldType == typeof(List<FakeComponent>))
                newValue = ConvertList<FakeComponent>(newValue);
            if (f.FieldType == typeof(List<FakeGameObject>))
                newValue = ConvertList<FakeGameObject>(newValue);
            if (f.FieldType == typeof(List<FakeTransform>))
                newValue = ConvertList<FakeTransform>(newValue);
            // Update the object's field's value.
            f.SetValue(o, newValue);
        }

        /// <summary>
        /// This is basically a type cast for lists and arrays.
        /// It returns a copy of the list as type List<T>, whatever T might be.
        /// If one of the elements of the original list can't be cast to T, it throws
        /// an exception.
        /// </summary>
        /// <typeparam name="T">Element type of the desired list</typeparam>
        /// <param name="list">Original list or array</param>
        /// <returns>The new list</returns>
        private static object ConvertList<T>(object list)
        {
            T ConvertElement(object e)
            {
                if (e is T e1)
                    return e1;
                throw new InvalidCastException($"Cannot convert {e} to type {typeof(T).Name}");
            }
            var iList = list as List<object>;
            if (iList == null)
                throw new ArgumentException($"Cannot convert {list} into type List<{typeof(T).Name}>");
            return iList.Select<object,T>(ConvertElement).ToList();
        }

        /// <summary>
        /// Make a new instance of the type with the specified name
        /// The type must have a default constructor, i.e. one without parameters.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static object MakeInstance(string typeName)
        {
            // Get the type object in this DLL with the specified name
            var t = Assembly.GetExecutingAssembly().GetType("Assets.FakeUnity."+typeName);
            if (t == null)
                throw new ArgumentException($"Can't find a type named {typeName}");
            // Call its default constructor.  Sorry this is such a mess; C#'s API for reflection isn't beautiful.
            return t.InvokeMember(null,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, 
                null, null, null);
        }
    }
}
