using UnityEngine;

[ExecuteInEditMode]
// ReSharper disable once CheckNamespace
public class Polygon : MonoBehaviour
{
    public Vector2[] Vertices;
    public Color Color;
    private static Material polygonMaterial;

    /// <summary>
    /// This is a super inefficient way to draw things, but it doesn't really matter for this game.
    /// </summary>
    internal void OnRenderObject()
    {
        if (!polygonMaterial)
            polygonMaterial = new Material(Shader.Find("Unlit/Color"));

        polygonMaterial.color = Color;
        polygonMaterial.SetPass(0);
        GL.PushMatrix();
        GL.MultMatrix(transform.localToWorldMatrix);
        switch (Vertices.Length)
        {
            case 3:
                GL.Begin(GL.TRIANGLES);
                foreach (var v in Vertices)
                {
                    GL.Vertex3(v.x, v.y, 0);
                }
                break;

            case 4:
                GL.Begin(GL.QUADS);
                foreach (var v in Vertices)
                {
                    GL.Vertex3(v.x, v.y, 0);
                }
                GL.End();
                break;

            default:
                // This would be more efficient with triangle strips, but I'm not going to worry
                // about it for version 0.
                GL.Begin(GL.TRIANGLES);
                Vector2 w;
                for (int i=0;i<Vertices.Length-1; i++)
                {
                    GL.Vertex3(0,0,0);
                    w = Vertices[i];
                    GL.Vertex3(w.x, w.y, 0);
                    w = Vertices[i+1];
                    GL.Vertex3(w.x, w.y, 0);
                }
                GL.Vertex3(0, 0, 0);
                w = Vertices[Vertices.Length-1];
                GL.Vertex3(w.x, w.y, 0);
                w = Vertices[0];
                GL.Vertex3(w.x, w.y, 0);
                break;
        }
        GL.End();
        GL.PopMatrix();
    }
}