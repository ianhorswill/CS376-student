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

using System;
using UnityEngine;

/// <summary>
/// Creates falling boxes at the top of the screen.
/// </summary>
public class BoxSpawner : MonoBehaviour
{
    [Header("Object drop from the top of the screen")]
    public Transform Prefab;

    [Header("How often to drop")]
    public float InitialSpawnDelay = 2;
    public float SpawnInterval = 5;
    public float SpawnAcceleration = 0.1f;
    public float MinSpawnInterval = 2;
    [Header("Where to drop")]
    public float SpawnHeight = 50;
    public float MinX = -50;
    public float MaxX = 50;
    public float Spacing = 0.5f;

    /// <summary>
    /// Next location to drop from
    /// </summary>
    private float nextX;

	/// <summary>
    /// Initialize component at startup.
    /// </summary>
	internal void Start () {
	    Invoke("Spawn", InitialSpawnDelay);
	    nextX = MinX;
	}

    /// <summary>
    /// Called via Invoke() when it's time to drop a box.
    /// </summary>
    internal void Spawn ()
    {
        // Make the box
        Instantiate(Prefab, new Vector3(nextX, SpawnHeight, 0), Quaternion.identity);

        // Set up for the next box drop.
        nextX += Spacing;
        if (nextX > MaxX)
            nextX = MinX;

        // Schedule the next box drop
        SpawnInterval = Math.Max(MinSpawnInterval, SpawnInterval - SpawnAcceleration);
        Invoke("Spawn", SpawnInterval);
    }
}
