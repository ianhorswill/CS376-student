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
/// Tracks score and displays it.
/// </summary>
public class ScoreCounter : MonoBehaviour
{
    /// <summary>
    /// Font, etc. for displaying the score.
    /// </summary>
    public GUIStyle GUIStyle;

    /// <summary>
    /// Screen location for the score.
    /// </summary>
    public Rect ScreenLocation;
    
    /// <summary>
    /// The player's current score
    /// </summary>
    public static int Score;

    /// <summary>
    /// Add points to the score.
    /// Points may be negative.
    /// </summary>
    /// <param name="points">Points to add/subtract</param>
    public static void AddScore(int points)
    {
        Score = Math.Max(0, Score + points);
    }

    /// <summary>
    /// Display the score.
    /// </summary>
    internal void OnGUI()
    {
        GUI.Label(ScreenLocation, Score.ToString(), GUIStyle);
    }
}
