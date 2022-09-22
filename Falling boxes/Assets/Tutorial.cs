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

using JetBrains.Annotations;
using UnityEngine;

/// <summary>
/// Displays instructions on screen.
/// </summary>
public class Tutorial : MonoBehaviour
{
    #region Inspector-tunable parameters
    public bool ShowTutorial = true;
    public GUIStyle GUIStyle;
    public Rect ScreenLocation;
    #endregion

    #region Internal state variables
    /// <summary>
    /// Save mouse position from previous tick (For detecting mouse motion).
    /// </summary>
    private Vector3 oldMousePosition;
    #endregion

    #region Tutorial data
    /// <summary>
    /// Where we are in the tutorial
    /// </summary>
    public enum TutorialState
    {
        Blank,
        MoveMouse,
        CatchBox,
        DepositBox,
        StackBox,
        PressKeys,
        DoneWithTutorial
    };

    /// <summary>
    /// Current position in tutorial.
    /// </summary>
    private static TutorialState tutorialState = TutorialState.Blank;

    /// <summary>
    /// String to display, indexed by tutorialState.
    /// </summary>
    [NotNull] private static readonly string[] TutorialStrings =
    {
        "",
        "Use the mouse to move the paddle",
        "Catch a box with the paddle",
        "Drop the box on the orange platform to score points",
        "Stack the boxes on the paddle to make them worth more",
        "Use arrow keys or WASD to tilt and raise the platform",
        ""
    };
    #endregion

    /// <summary>
    /// Tell tutoral that the user did something it might be waiting for.
    /// </summary>
    /// <param name="action">What the user did.</param>
    public static void UserAction(TutorialState action)
    {
        if (action == tutorialState)
            tutorialState++;
    }

    /// <summary>
    /// Display the tutorial itself.
    /// </summary>
    internal void OnGUI()
    {
        if (!ShowTutorial)
            return;

        if (tutorialState == TutorialState.MoveMouse && Input.mousePosition != oldMousePosition)
            UserAction(TutorialState.MoveMouse);
        oldMousePosition = Input.mousePosition;

        if (tutorialState == TutorialState.PressKeys && Input.anyKeyDown)
            UserAction(TutorialState.PressKeys);

        // Don't display anything for the first two seconds because the player won't see it anyway.
        if (tutorialState == TutorialState.Blank && Time.time > 2)
            tutorialState++;

        GUI.Label(ScreenLocation, TutorialStrings[(int)tutorialState], GUIStyle);
    }
}
