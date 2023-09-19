using UnityEngine;

/// <summary>
/// Draws the scoring zone on the screen
/// </summary>
[ExecuteInEditMode]
public class ScoringZoneRenderer : MonoBehaviour
{
    Vector3[] vertices;
    private static Material lineMaterial;

    public Color Color = Color.green;
    const int CircleFaces = 16;

    /// <summary>
    /// Initialize lines based on radius of attached CircleCollider2D
    /// </summary>
    internal void Start()
    {
        var circle = GetComponent<CircleCollider2D>();
        if (circle != null)
        {
            vertices = new Vector3[2*CircleFaces];
            for (int i = 0; i < CircleFaces; i++)
            {
                vertices[2*i] = CirclePosition(circle.radius, i);
                vertices[2*i + 1] = CirclePosition(circle.radius, i + 1);
            }
        }
    }

    Vector3 CirclePosition(float radius , int vertexNumber )
    {
        return new Vector3(
            radius*Mathf.Cos(Mathf.PI*2*vertexNumber/CircleFaces),
            radius * Mathf.Sin(Mathf.PI * 2 * vertexNumber / CircleFaces),
            0);
    }

    /// <summary>
    /// This is a super inefficient way to draw things, but it doesn't really matter for this game.
    /// </summary>
    internal void OnRenderObject()
    {
        if (!lineMaterial)
            lineMaterial = new Material(Shader.Find("Unlit/Color"));

        lineMaterial.color = Color;
        lineMaterial.SetPass(0);
        GL.PushMatrix();
        GL.MultMatrix(transform.localToWorldMatrix);
        GL.Begin(GL.LINES);
        foreach (var v in vertices)
            GL.Vertex(v);
        GL.End();
        GL.PopMatrix();
    }
}