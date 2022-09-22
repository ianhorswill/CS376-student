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
/// Tracks the state of a falling box:
///   - Whether it's been caught by the player
///   - Whether it's touched the platform
///   - How many points it's worth
/// Also destroys the box and updates score when appropriate.
/// </summary>
public class BoxState : MonoBehaviour
{
    public AudioClip TouchSound;
    public AudioClip ScoreSound;
    public AudioClip FallSound;
    public AudioClip EnableSound;
    public AudioClip DisableSound;

    #region Internal state variables
    /// <summary>
    /// Number of points the box is currently worth.
    /// If negative, then the box hasn't been caught by the player yet.
    /// </summary>
    private int pointValue = -1;

    private bool alreadyFalling;
    
    /// <summary>
    /// When we first touched the platform
    /// </summary>
    private float touchPlatformStart;

    /// <summary>
    /// True when the box has touched the platform.
    /// The box can no longer change its value once it touches the platform.
    /// </summary>
    private bool touchedPlatform;

    /// <summary>
    /// Cached pointer to our rigid body physics component
    /// </summary>
    private Rigidbody2D rigidBody;

    private AudioSource audioSource;
    #endregion

    /// <summary>
    /// The player has caught this object at some point.
    /// </summary>
    private bool CaughtByPlayer
    {
        get { return pointValue > 0; }
    }

    /// <summary>
    /// Initialize component at the start of the game.
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    internal void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Called when something touches this box.
    /// </summary>
    /// <param name="c">The collider2D of the object that hit us</param>
    // ReSharper disable once UnusedMember.Global
    internal void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.name != "Platform")
            PlaySound(TouchSound);
        if (!touchedPlatform)
        {
            if (c.gameObject.name == "Platform")
            {
                TouchPlatform();
                Tutorial.UserAction(Tutorial.TutorialState.DepositBox);
            }
            if (!CaughtByPlayer)
            {
                if (c.gameObject.GetComponent<PlayerController>() != null)
                {
                    // Collided with the player's paddle
                    pointValue = 1;
                    UpdateColor();
                    PlaySound(EnableSound);
                    Tutorial.UserAction(Tutorial.TutorialState.CatchBox);
                }
                else
                {
                    var otherBox = c.gameObject.GetComponent<BoxState>();
                    if (otherBox != null)
                    {
                        if (otherBox.touchedPlatform)
                        {
                            TouchPlatform();
                        }
                        else
                        {
                            // Collided with another box
         
                            pointValue = Math.Max(pointValue, otherBox.pointValue + 3);
                            UpdateColor();
                            PlaySound(pointValue>0?EnableSound:DisableSound);
                            Tutorial.UserAction(Tutorial.TutorialState.StackBox);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Called whenever we touch the platform.
    /// 
    /// </summary>
    private void TouchPlatform()
    {
        touchPlatformStart = Time.time;
        touchedPlatform = true;
        if (CaughtByPlayer)
            PlaySound(ScoreSound);
        else
        {
            SetColor(Color.red);
            PlaySound(DisableSound);
        }
    }
    
    private void PlaySound(AudioClip clip)
    {
        if (clip == null)
            return;
        audioSource.loop = false;
        audioSource.PlayOneShot(clip);
    }

    /// <summary>
    /// Check to see if we need to destroy the object or update its color.
    /// Called once per display frame.
    /// </summary>
    internal void Update()
    {
        if (touchedPlatform && rigidBody.IsSleeping()  && CaughtByPlayer)
        {
            ScoreCounter.AddScore(pointValue);
            PlaySound(ScoreSound);
            Destroy(gameObject);
        }

        if (touchedPlatform && (Time.time - touchPlatformStart) > 60)
            Destroy(gameObject);

        var y = transform.position.y;

        if (y < 0)
        {
            if (!CaughtByPlayer)
                SetColor(Color.red);
            if (!alreadyFalling)
            {
                PlaySound(FallSound);
                alreadyFalling = true;
            }
        }

        if (y < -20)
        {
            Destroy(gameObject);
            ScoreCounter.AddScore(-1);
        }
    }

    void UpdateColor()
    {
        if (CaughtByPlayer)
        {
            SetColor(new Color(0, Mathf.Min(1, (pointValue+2)/11f), 0, 1));
        }
    }

    private void SetColor(Color color)
    {
        GetComponent<Quad>().Color = color;
    }
}
