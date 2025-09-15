using UnityEngine;

namespace SerializerTests.FakeUnity
{
    public class DummyClass
    {
        [SerializeField]
        public DummyClass MyField;

        public DummyClass(DummyClass myField)
        {
            MyField = myField;
        }
    }
}
