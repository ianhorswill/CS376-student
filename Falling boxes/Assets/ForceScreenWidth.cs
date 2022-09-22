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
/// Force an orthographic Camera to be a specified width in world units.
/// Place in the game object with the camera.
/// This is needed because the built-in Camera component lets you set the height
/// of the viewport in world coordinates, but now the width.
/// </summary>
public class ForceScreenWidth : MonoBehaviour
{
    public float WorldWidth=100;

	// Use this for initialization
    internal void Start()
    {
        if (Application.isEditor && !Application.isPlaying)
            // Don't screw around with things in the middle of the editor!
            return;

        var aspectRatio = ((float) Screen.width)/Screen.height;
        var thisObjectsCamera = GetComponent<Camera>();
        var currentWorldWidth = thisObjectsCamera.orthographicSize*aspectRatio;
        // Want the worldWidth to be 55.
        var correction = 0.5f*WorldWidth/currentWorldWidth;
        thisObjectsCamera.orthographicSize *= correction;
    }
}
