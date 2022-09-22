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
/// Controller for the player's paddle.
/// Also contains logic for the tutorial
/// </summary>
public class PlayerController : MonoBehaviour {
    [Header("Spring constants")]
    public float MouseStiffness = 10;
    public float VerticalStiffness = 10;
    public float TorsionalStiffness = 10;
    [Header("Keyboard control parameters")]
    public float JumpHeight = 1;
    public float TiltAngle = 45;
    [Header("Whether paddle rises with score")]
    public float ScoreRise = 0;

    /// <summary>
    /// Cached RodigBody2D component
    /// </summary>
    private Rigidbody2D rigidBody;

    /// <summary>
    /// Initialize component at startup time
    /// </summary>
    internal void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }
    
    /// <summary>
    /// Called once per physics update.
    /// Add spring forces to paddle.
    /// </summary>
	internal void FixedUpdate ()
	{
	    var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	    var playerPosition = transform.position;

	    var mouseOffset = mousePosition-playerPosition;
	    mouseOffset.y = 0;

	    rigidBody.AddForce(MouseStiffness*mouseOffset);
        rigidBody.AddForce(new Vector2(
            0,
            VerticalStiffness * (VerticalSetPoint + ScoreCounter.Score*ScoreRise - transform.position.y)));

        rigidBody.AddTorque(TorsionalStiffness * (RotationalSetPoint - rigidBody.rotation));
	}

    /// <summary>
    /// Number of degrees to rotate the paddle
    /// </summary>
    /// <returns></returns>
    private float RotationalSetPoint
    {
        get { return TiltLeft ? (TiltAngle) : TiltRight ? -TiltAngle : 0; }
    }

    /// <summary>
    /// Number of units to raise/lower the paddle
    /// </summary>
    /// <returns></returns>
    private float VerticalSetPoint
    {
        get
        {
            return PushUp ? JumpHeight : PushDown ? (-JumpHeight) : 0;
        }
    }

    #region Keyboard controls
    /// <summary>
    /// Player wants to tile the paddle right
    /// </summary>
    private static bool TiltRight
    {
        get
        {
            // TODO: Fix this
            return false;
        }
    }

    /// <summary>
    /// Player wants to tilt the paddle left
    /// </summary>
    private static bool TiltLeft
    {
        get
        {
            // TODO: Fix this
            return false;
        }
    }

    /// <summary>
    /// Player wants to push the paddle down
    /// </summary>
    private static bool PushDown
    {
        get
        {
            // TODO: Fix this
            return false;
        }
    }

    /// <summary>
    /// Player wants to push the paddle up
    /// </summary>
    private static bool PushUp
    {
        get
        {
            // TODO: Fix this
            return false;
        }
    }

    /// <summary>
    /// Let the player quit the game (in case this isn't being run in the editor.
    /// </summary>
    internal void OnGUI()
    {
        if (Event.current.type == EventType.KeyDown)
            if (Event.current.keyCode == KeyCode.Escape || Event.current.keyCode == KeyCode.Q)
                Application.Quit();
    }
#endregion
}
