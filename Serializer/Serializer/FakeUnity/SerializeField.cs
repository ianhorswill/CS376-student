using System;

namespace FakeUnity
{
    /// <summary>
    /// This is the attribute (annotation) you can add to a field to tell the serializer
    /// to include it in the serialization of the containing object
    /// </summary>
    public class SerializeField : Attribute
    {
    }
}
