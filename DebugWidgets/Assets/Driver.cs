using UnityEngine;

// ReSharper disable once UnusedMember.Global
public class Driver : MonoBehaviour
{
    private static readonly string[] Axes = {"X", "Y", "Axis 3", "Axis 4", "Axis 5", "Axis 6", "Axis 7", "Axis 8" };

    // Update is called once per frame
    // ReSharper disable once UnusedMember.Local
    void Update()
    {
        BarGraph.Find("Sin", new Vector2(10, 10), -.9f, .9f).SetReading(Mathf.Sin(3*Time.time));
        var position = new Vector2(10, 950);
        foreach (var axis in Axes)
        {
            BarGraph.Find(axis, position, -1, 1).SetReading(Input.GetAxis(axis));
            position.y -= 100;
        }

        position = new Vector2(600, 950);
        for (var button = 0; button < 10; button++)
        {
            var axis = $"Button {button}";
            BarGraph.Find(axis, position, 0, 1).SetReading(Input.GetAxis(axis));
            position.y -= 100;
        }
    }
}
