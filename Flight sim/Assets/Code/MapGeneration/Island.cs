using UnityEngine;

public class Island : MonoBehaviour {
    public float Height = 0.75f;
    public float LengthX = 32f;
    public float LengthY = 32f;
    public float x { get { return transform.localPosition.x; } }
    public float y { get { return transform.localPosition.y; } }
}
