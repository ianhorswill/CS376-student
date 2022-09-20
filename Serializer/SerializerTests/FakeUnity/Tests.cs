using System.Linq;
using Assets.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FakeUnity.Tests
{
    [TestClass()]
    public class Tests
    {
        string RemoveAllWhitespace(string x) => new(x.Where(c => !char.IsWhiteSpace(c)).ToArray());
        
        void TestSerialize<T>(T value, string expectedSerialization) 
            => Assert.AreEqual(RemoveAllWhitespace(expectedSerialization),
                RemoveAllWhitespace(Serializer.Serialize(value)),
                $"The value {value} did not serialize to the expected string");

        [TestMethod]
        public void SerializeInt() => TestSerialize(1, "1");

        [TestMethod]
        public void SerializeFloat() => TestSerialize(1.5f, "1.5");

        [TestMethod]
        public void SerializeBool()
        {
            TestSerialize(true, "True");
            TestSerialize(false, "False");
        }
        
        [TestMethod]
        public void SerializeString() => TestSerialize("foo", "\"foo\"");
        
        [TestMethod]
        public void SerializeNull() => TestSerialize<object>(null, "null");

        [TestMethod]
        public void SerializeDummyObject()
            => TestSerialize(new DummyClass() { MyField = null }, "#0{type:\"DummyClass\",MyField:null}");

        [TestMethod]
        public void DeserializeDummyObject()
        {
            var obj = Deserializer.Deserialize("#0{type:\"DummyClass\",MyField:null}");
            Assert.IsInstanceOfType(obj, typeof(DummyClass));
            Assert.AreEqual(null, ((DummyClass)obj).MyField);
        }

        [TestMethod]
        public void SerializeCyclicDummyObject()
        {
            var obj = new DummyClass();
            obj.MyField = obj;
            TestSerialize(obj, "#0{type:\"DummyClass\",MyField:#0}");
        }

        [TestMethod]
        public void DeserializeCyclicDummyObject()
        {
            var obj = Deserializer.Deserialize("#0{type:\"DummyClass\",MyField:#0}");
            Assert.IsInstanceOfType(obj, typeof(DummyClass));
            Assert.AreEqual(obj, ((DummyClass)obj).MyField);
        }

        [TestMethod]
        public void SerializeTransform()
            => TestSerialize(new Transform(), "#0{type:\"Transform\",X:0,Y:0,parent:null,children:[],gameObject:null}");

        [TestMethod]
        public void DeserializeTransform()
        {
            var obj = Deserializer.Deserialize(
                "#0{type:\"Transform\",X:0,Y:0,parent:null,children:[],gameObject:null}");
            Assert.IsInstanceOfType(obj, typeof(Transform));
            var transform = (Transform)obj;
            Assert.AreEqual(null, transform.parent);
            Assert.AreEqual(0, transform.X);
            Assert.AreEqual(0, transform.Y);

        }

        [TestMethod]
        public void SerializeEmptyGameObject()
            => TestSerialize(GameObject.Create("test", null),
                "#0{ type: \"GameObject\", name: \"test\",components: [ #1{ type: \"Transform\", X: 0, Y: 0, parent: null, children: [ ], gameObject: #0 } ] }");

        [TestMethod]
        public void DeserializeEmptyGameObject()
        {
            var obj = Deserializer.Deserialize(
                "#0{ type: \"GameObject\", name: \"test\",components: [ #1{ type: \"Transform\", X: 0, Y: 0, parent: null, children: [ ], gameObject: #0 } ] }");
            Assert.IsInstanceOfType(obj, typeof(GameObject));
            var go = (GameObject)obj;
            Assert.IsInstanceOfType(go.transform, typeof(Transform));
            Assert.AreEqual("test", go.name);
            var transform = go.transform;
            Assert.AreEqual(go, transform.gameObject);
            Assert.AreEqual(null, transform.parent);
            Assert.AreEqual(0, transform.X);
            Assert.AreEqual(0, transform.Y);
        }

        [TestMethod]
        public void SerializeComplexGraph()
        {
            var parent = GameObject.Create("test", null);
            var child1 = GameObject.Create("child 1", parent);
            AddCircle(child1, 10, 100, 100);
            GameObject.Create("child 2", child1);
            var child3 = GameObject.Create("child 3", parent);
            AddCircle(child3, 200, 500, 550);
            TestSerialize(parent, "#0{type:\"GameObject\",name:\"test\",components:[#1{type:\"Transform\",X:0,Y:0,parent:null,children:[#2{type:\"Transform\",X:100,Y:100,parent:#1,children:[#3{type:\"Transform\",X:0,Y:0,parent:#2,children:[],gameObject:#4{type:\"GameObject\",name:\"child2\",components:[#3]}}],gameObject:#5{type:\"GameObject\",name:\"child1\",components:[#2,#6{type:\"CircleCollider2D\",Radius:10,gameObject:#5},#7{type:\"SpriteRenderer\",FileName:\"circle.jpg\",gameObject:#5}]}},#8{type:\"Transform\",X:500,Y:550,parent:#1,children:[],gameObject:#9{type:\"GameObject\",name:\"child3\",components:[#8,#10{type:\"CircleCollider2D\",Radius:200,gameObject:#9},#11{type:\"SpriteRenderer\",FileName:\"circle.jpg\",gameObject:#9}]}}],gameObject:#0}]}");
        }

        [TestMethod]
        public void DeserializeComplexGraphParent()
        {
            var obj = Deserializer.Deserialize(
                "#0{type:\"GameObject\",name:\"test\",components:[#1{type:\"Transform\",X:0,Y:0,parent:null,children:[#2{type:\"Transform\",X:100,Y:100,parent:#1,children:[#3{type:\"Transform\",X:0,Y:0,parent:#2,children:[],gameObject:#4{type:\"GameObject\",name:\"child 2\",components:[#3]}}],gameObject:#5{type:\"GameObject\",name:\"child 1\",components:[#2,#6{type:\"CircleCollider2D\",Radius:10,gameObject:#5},#7{type:\"SpriteRenderer\",FileName:\"circle.jpg\",gameObject:#5}]}},#8{type:\"Transform\",X:500,Y:550,parent:#1,children:[],gameObject:#9{type:\"GameObject\",name:\"child 3\",components:[#8,#10{type:\"CircleCollider2D\",Radius:200,gameObject:#9},#11{type:\"SpriteRenderer\",FileName:\"circle.jpg\",gameObject:#9}]}}],gameObject:#0}]}");
            Assert.IsInstanceOfType(obj, typeof(GameObject));
            var parent = (GameObject)obj;
            Assert.AreEqual("test", parent.name);
            var transform = parent.transform;
            Assert.AreEqual(parent, transform.gameObject);
            Assert.AreEqual(null, transform.parent);
            Assert.AreEqual(0, transform.X);
            Assert.AreEqual(0, transform.Y);
            var child1 = transform.GetChild(0).gameObject;
            Assert.AreEqual(transform, child1.transform.parent);
            Assert.AreEqual("child 1", child1.name);
            var circle = child1.GetComponent<CircleCollider2D>();
            Assert.AreEqual(10, circle.Radius);
            Assert.AreEqual(100, child1.transform.X);
            Assert.AreEqual(100, child1.transform.Y);
            var spriteRenderer = child1.GetComponent<SpriteRenderer>();
            Assert.AreEqual("circle.jpg", spriteRenderer.FileName);
            var child2 = child1.transform.GetChild(0).gameObject;
            Assert.AreEqual("child 2", child2.name);
            Assert.AreEqual(child1.transform, child2.transform.parent);
            var child3 = parent.transform.GetChild(1).gameObject;
            Assert.AreEqual("child 3", child3.name);
            Assert.AreEqual(parent.transform, child3.transform.parent);
        }

        [TestMethod]
        public void DeserializeComplexGraphChild1()
        {
            var obj = Deserializer.Deserialize(
                "#0{type:\"GameObject\",name:\"test\",components:[#1{type:\"Transform\",X:0,Y:0,parent:null,children:[#2{type:\"Transform\",X:100,Y:100,parent:#1,children:[#3{type:\"Transform\",X:0,Y:0,parent:#2,children:[],gameObject:#4{type:\"GameObject\",name:\"child 2\",components:[#3]}}],gameObject:#5{type:\"GameObject\",name:\"child 1\",components:[#2,#6{type:\"CircleCollider2D\",Radius:10,gameObject:#5},#7{type:\"SpriteRenderer\",FileName:\"circle.jpg\",gameObject:#5}]}},#8{type:\"Transform\",X:500,Y:550,parent:#1,children:[],gameObject:#9{type:\"GameObject\",name:\"child 3\",components:[#8,#10{type:\"CircleCollider2D\",Radius:200,gameObject:#9},#11{type:\"SpriteRenderer\",FileName:\"circle.jpg\",gameObject:#9}]}}],gameObject:#0}]}");
            Assert.IsInstanceOfType(obj, typeof(GameObject));
            var parent = (GameObject)obj;
            var transform = parent.transform;
            var child1 = transform.GetChild(0).gameObject;
            Assert.AreEqual(transform, child1.transform.parent);
            Assert.AreEqual("child 1", child1.name);
            var circle = child1.GetComponent<CircleCollider2D>();
            Assert.AreEqual(10, circle.Radius);
            Assert.AreEqual(100, child1.transform.X);
            Assert.AreEqual(100, child1.transform.Y);
            var spriteRenderer = child1.GetComponent<SpriteRenderer>();
            Assert.AreEqual("circle.jpg", spriteRenderer.FileName);
        }

        [TestMethod]
        public void DeserializeComplexGraphChild2()
        {
            var obj = Deserializer.Deserialize(
                "#0{type:\"GameObject\",name:\"test\",components:[#1{type:\"Transform\",X:0,Y:0,parent:null,children:[#2{type:\"Transform\",X:100,Y:100,parent:#1,children:[#3{type:\"Transform\",X:0,Y:0,parent:#2,children:[],gameObject:#4{type:\"GameObject\",name:\"child 2\",components:[#3]}}],gameObject:#5{type:\"GameObject\",name:\"child 1\",components:[#2,#6{type:\"CircleCollider2D\",Radius:10,gameObject:#5},#7{type:\"SpriteRenderer\",FileName:\"circle.jpg\",gameObject:#5}]}},#8{type:\"Transform\",X:500,Y:550,parent:#1,children:[],gameObject:#9{type:\"GameObject\",name:\"child 3\",components:[#8,#10{type:\"CircleCollider2D\",Radius:200,gameObject:#9},#11{type:\"SpriteRenderer\",FileName:\"circle.jpg\",gameObject:#9}]}}],gameObject:#0}]}");
            Assert.IsInstanceOfType(obj, typeof(GameObject));
            var parent = (GameObject)obj;
            var transform = parent.transform;
            var child1 = transform.GetChild(0).gameObject;
            Assert.AreEqual(transform, child1.transform.parent);
            Assert.AreEqual("child 1", child1.name);
            var child2 = child1.transform.GetChild(0).gameObject;
            Assert.AreEqual("child 2", child2.name);
            Assert.AreEqual(child1.transform, child2.transform.parent);
        }

        [TestMethod]
        public void DeserializeComplexGraphChild3()
        {
            var obj = Deserializer.Deserialize(
                "#0{type:\"GameObject\",name:\"test\",components:[#1{type:\"Transform\",X:0,Y:0,parent:null,children:[#2{type:\"Transform\",X:100,Y:100,parent:#1,children:[#3{type:\"Transform\",X:0,Y:0,parent:#2,children:[],gameObject:#4{type:\"GameObject\",name:\"child 2\",components:[#3]}}],gameObject:#5{type:\"GameObject\",name:\"child 1\",components:[#2,#6{type:\"CircleCollider2D\",Radius:10,gameObject:#5},#7{type:\"SpriteRenderer\",FileName:\"circle.jpg\",gameObject:#5}]}},#8{type:\"Transform\",X:500,Y:550,parent:#1,children:[],gameObject:#9{type:\"GameObject\",name:\"child 3\",components:[#8,#10{type:\"CircleCollider2D\",Radius:200,gameObject:#9},#11{type:\"SpriteRenderer\",FileName:\"circle.jpg\",gameObject:#9}]}}],gameObject:#0}]}");
            Assert.IsInstanceOfType(obj, typeof(GameObject));
            var parent = (GameObject)obj;
            Assert.AreEqual("test", parent.name);
            var transform = parent.transform;
            Assert.AreEqual(parent, transform.gameObject);
            Assert.AreEqual(null, transform.parent);
            Assert.AreEqual(0, transform.X);
            Assert.AreEqual(0, transform.Y);
            var child3 = transform.GetChild(1).gameObject;
            Assert.AreEqual(transform, child3.transform.parent);
            Assert.AreEqual("child 3", child3.name);
            var circle = child3.GetComponent<CircleCollider2D>();
            Assert.AreEqual(200, circle.Radius);
            Assert.AreEqual(500, child3.transform.X);
            Assert.AreEqual(550, child3.transform.Y);
            var spriteRenderer = child3.GetComponent<SpriteRenderer>();
            Assert.AreEqual("circle.jpg", spriteRenderer.FileName);
        }

        /// <summary>
        /// Add the components for a circle to a game object
        /// </summary>
        /// <param name="gameObject">GameObject to which to add the components</param>
        /// <param name="radius">Desired radius of the circle collider</param>
        /// <param name="x">Desired x coordinate of the circle collider</param>
        /// <param name="y">Desired y coordinate of the circle collider</param>
        private static void AddCircle(GameObject gameObject, float radius, float x, float y)
        {
            var collider = gameObject.AddComponent<CircleCollider2D>();
            collider.Radius = radius;
            var sprite = gameObject.AddComponent<SpriteRenderer>();
            sprite.FileName = "circle.jpg";
            var transform = gameObject.GetComponent<Transform>();
            transform.X = x;
            transform.Y = y;
        }


#if notdef
        void TestDeserialize<T>(T value, string serialization)
            => Assert.AreEqual(value, (T)Deserializer.Deserialize(serialization), $"The serialization of {value} did not deserialize to itself");

        [TestMethod]
        public void DeserializeInt() => TestDeserialize(1, "1");

        [TestMethod]
        public void DeserializeFloat() => TestDeserialize(1.5f, "1.5");

        [TestMethod]
        public void DeserializeBool()
        {
            TestDeserialize(true, "True");
            TestDeserialize(false, "False");
        }
        
        [TestMethod]
        public void DeserializeString() => TestDeserialize("foo", "\"foo\"");
        
        [TestMethod]
        public void DeserializeNull() => TestDeserialize<object>(null, "null");
#endif
    }
}