using System.Collections;
using Assets.FakeUnity;
using Assets.Serialization;
using TMPro;
using UnityEngine;

public class Driver : MonoBehaviour
{
    // We use an input field component rather than a text component
    // so that users can select text and copy it in case they need to
    // put it in an email or a discord post
    public TMP_InputField TextDisplay;

    private string[] outputs = new [] { "No output generated" };

    // Start is called before the first frame update
    void Start()
    {
        Score = 0;
        StartCoroutine(AlternateOutputs());

        TestAndScoreSerializer();
    }

    /// <summary>
    /// Run unit tests and log their results, then log a final score
    /// </summary>
    private void TestAndScoreSerializer()
    {
        TestSimpleSerialization();

        var gSerialization = TestGraphSerialization();
        SetScreen(gSerialization);
        AssertScore(gSerialization is string, "Serialize GameObject with children and extra components doesn't crash", 10);

        TestSimpleDeserialization();

        // Deserialize the four-object graph from TestSerialization.
        var deserialized = Deserializer.Deserialize(gSerialization);
        // Test that if we reserialize the deserialized graph, we get the same serialization
        var reserialization = Serializer.Serialize(deserialized);
        AssertScore(gSerialization, reserialization, "Serialization and reserialization are identical", 10);

        // Display both versions of the serialization
        SetScreen(gSerialization, reserialization);

        // Look at the deserialized object graph to make sure it looks right.
        TestDeserializationContents(deserialized);
        Debug.Log($"Final score: {Score}");
    }

    /// <summary>
    /// Test that individual kinds of objects can be serialized.
    /// </summary>
    private void TestSimpleSerialization()
    {
        AssertScore(Serializer.Serialize(1).Trim(), "1", "Serialize integer", 1);
        AssertScore(Serializer.Serialize(1.1f).Trim(), "1.1", "Serialize float", 1);
        AssertScore(Serializer.Serialize("foo").Trim(), "\"foo\"", "Serialize string", 1);
        AssertScore(Serializer.Serialize(true).Trim(), "True", "Serialize Boolean", 1);
        AssertScore(Serializer.Serialize(null).Trim(), "null", "Serialize null", 1);
        AssertScore(Serializer.Serialize(new FakeCircleCollider2D()) is string, "Serialize component runs", 1);
    }

    /// <summary>
    /// Test that a complex graph can be serialized
    /// Note that this doesn't test that the serialization is "right".
    /// We'll test that when we test the deserializer.
    /// </summary>
    /// <returns>Serialization of the graph</returns>
    private string TestGraphSerialization()
    {
        var parent = Assets.FakeUnity.FakeGameObject.Create("test", null);
        AssertScore(Serializer.Serialize(parent) is string, "Serialialize GameObject runs", 10);
        var child1 = Assets.FakeUnity.FakeGameObject.Create("child 1", parent);
        AssertScore(Serializer.Serialize(parent) is string, "Serialialize GameObject with child runs", 10);
        AddCircle(child1, 10, 100, 100);
        Assets.FakeUnity.FakeGameObject.Create("child 2", child1);
        var child3 = Assets.FakeUnity.FakeGameObject.Create("child 3", parent);
        AddCircle(child3, 200, 500, 550);

        var serialization = Serializer.Serialize(parent);
        return serialization;
    }

    /// <summary>
    /// Test the deserialization of simple objects
    /// </summary>
    private void TestSimpleDeserialization()
    {
        // Test deserialization of simple objects
        AssertScore(Deserializer.Deserialize(Serializer.Serialize(1)), 1, "Deserialize integer", 1);
        AssertScore(Deserializer.Deserialize(Serializer.Serialize(1.1f)), 1.1f, "Deserialize float", 1);
        AssertScore(Deserializer.Deserialize(Serializer.Serialize("foo")), "foo", "Deserialize string", 1);
        AssertScore(Deserializer.Deserialize(Serializer.Serialize(false)), false, "Deserialize Boolean", 1);
        AssertScore(Deserializer.Deserialize(Serializer.Serialize(null)), null, "Deserialize null", 1);
        AssertScore(Deserializer.Deserialize(Serializer.Serialize(new FakeCircleCollider2D())) is FakeCircleCollider2D,
            "Deserialize FakeCircleCollider2D runs", 10);
        AssertScore(Deserializer.Deserialize(Serializer.Serialize(FakeGameObject.Create("test", null))) is FakeGameObject,
            "Deserialize FakeGameObject runs", 10);
}

    /// <summary>
    /// Check that the deserialized contents of the graph look like the original graph
    /// </summary>
    /// <param name="deserialized">Deserialized graph</param>
    private void TestDeserializationContents(object deserialized)
    {
        var reconstructed = deserialized as FakeGameObject;
        var rChild1 = reconstructed.transform.GetChild(0).gameObject;
        CheckCircle(rChild1, 10, 100, 100);
        var rChild2 = rChild1.transform.GetChild(0).gameObject;
        AssertScore(rChild2 is FakeGameObject, "Reconstructed graph has second child gameObject", 5);
        var rChild3 = reconstructed.transform.GetChild(1).gameObject;
        CheckCircle(rChild3, 200, 500, 550);
    }


    /// <summary>
    /// Have the screen rotate through displaying the different strings passed in as arguments.
    /// If you call this twice, it will forget the strings from the first call.
    /// </summary>
    /// <param name="strings">Strings to display</param>
    void SetScreen(params string[] strings)
    {
        outputs = strings;
    }

    /// <summary>
    /// Add the components for a circle to a game object
    /// </summary>
    /// <param name="gameObject">GameObject to which to add the components</param>
    /// <param name="radius">Desired radius of the circle collider</param>
    /// <param name="x">Desired x coordinate of the circle collider</param>
    /// <param name="y">Desired y coordinate of the circle collider</param>
    private static void AddCircle(FakeGameObject gameObject, float radius, float x, float y)
    {
        var collider = gameObject.AddComponent<FakeCircleCollider2D>();
        collider.Radius = radius;
        var sprite = gameObject.AddComponent<FakeSpriteRenderer>();
        sprite.FileName = "circle.jpg";
        var transform = gameObject.GetComponent<FakeTransform>();
        transform.X = x;
        transform.Y = y;
    }

    /// <summary>
    /// Check the game object has the expected components with the expected field values
    /// </summary>
    /// <param name="gameObject">GameObject to which to add the components</param>
    /// <param name="radius">Desired radius of the circle collider</param>
    /// <param name="x">Desired x coordinate of the circle collider</param>
    /// <param name="y">Desired y coordinate of the circle collider</param>
    private void CheckCircle(FakeGameObject gameObject, float radius, float x, float y)
    {
        var collider = gameObject.GetComponent<FakeCircleCollider2D>();
        AssertScore(collider.Radius, radius, $"Radius of {gameObject.name}'s collider", 3);
        var sprite = gameObject.GetComponent<FakeSpriteRenderer>();
        AssertScore(sprite.FileName, "circle.jpg", $"FileName of {gameObject.name}'s sprite renderer", 3);
        var transform = gameObject.GetComponent<FakeTransform>();
        AssertScore(transform.X, x, $"{gameObject.name}'s X coordinate", 3);
        AssertScore(transform.Y, y, $"{gameObject.name}'s Y coordinate", 3);
    }

    IEnumerator AlternateOutputs()
    {
        while (true)
            foreach (var o in outputs)
            {
                TextDisplay.text = o;
                yield return new WaitForSeconds(1);
            }
    }

    public int Score;

    private void GivePoints(int points) => Score += points;

    private void AssertScore(bool test, string description, int points)
    {
        Debug.Log((test ? "Succeeded: " : "Failed: ") + description);
        if (test) GivePoints(points);
    }

    private void AssertScore(object testExp, object desiredValue, string description, int points)
    {
        if (testExp == null?(desiredValue==null):testExp.Equals(desiredValue))
        {
            Debug.Log("Succeeded: " + description);
            GivePoints(points);
        }
        else
        {
            Debug.Log("Failed: " + description);
            Debug.Log($"Expected {desiredValue}, but got {testExp}");
        }

    }
}
