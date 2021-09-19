using UnityEngine;

[ExecuteInEditMode]
public class Circle : Polygon
{
    public float Radius=1;
    public int Segments=5;

    internal void Start()
    {
        Vertices = new Vector2[Segments + 1];
        for (int i = 0; i < Vertices.Length; i++)
        {
            var angle = -i * Mathf.PI * 2 / Segments;
            Vertices[i] = new Vector2(Radius * Mathf.Cos(angle), Radius * Mathf.Sin(angle));
        }
    }

    internal void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}
