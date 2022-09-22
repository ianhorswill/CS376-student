/*
 * Copyright (c) 2016 Ian Horswill
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
 * associated documentation files (the "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the
 * following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies or substantial
 * portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT
 * LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN
 * NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
 * SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using UnityEngine;

/// <summary>
/// Draws a colored rectangle overlapping this gameobject's BoxCollider2D.
/// </summary>
[ExecuteInEditMode, RequireComponent(typeof(BoxCollider2D))]
// ReSharper disable once CheckNamespace
public class Quad : MonoBehaviour
{
    /// <summary>
    /// Color in which to draw the quad
    /// </summary>
    public Color Color;

    public Material Material;

    /// <summary>
    /// Cached copy of the this GameObject's collider so we don't have to look it up every time we draw.
    /// </summary>
    private BoxCollider2D myCollider;

    /// <summary>
    /// Find the collider.
    /// Called once at the start of the game.
    /// </summary>
    internal void Start()
    {
        myCollider = GetComponent<BoxCollider2D>();
    }

    /// <summary>
    /// Draw the quad.
    /// This is a super inefficient way to draw things, but it doesn't really matter for this game.
    /// </summary>
    internal void OnRenderObject()
    {
        // Set up for drawing
        Material.color = Color;
        Material.SetPass(0);
        GL.PushMatrix();
        GL.MultMatrix(transform.localToWorldMatrix);

        // Draw one quad :-(
        GL.Begin(GL.QUADS);
        float halfWidth = myCollider.size.x * 0.5f;
        float halfHeight = myCollider.size.y * 0.5f;
        var offset = myCollider.offset;

        GL.Vertex3(-halfWidth + offset.x, halfHeight + offset.y, 0);
        GL.Vertex3(halfWidth + offset.x, halfHeight + offset.y, 0);
        GL.Vertex3(halfWidth + offset.x, -halfHeight + offset.y, 0);
        GL.Vertex3(-halfWidth + offset.x, -halfHeight + offset.y, 0);
        GL.End();

        GL.End();
        GL.PopMatrix();
    }
}