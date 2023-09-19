using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Keeps track of the player's score
/// </summary>
public class ScoreKeeper : MonoBehaviour
{
    private int kills;
    private int lives=1;
    private float scoringTime;
    bool inScoringZone;
    float enterScoringZoneTimestamp;

    public float Score
    {
        get
        {
            var timeInCurrentZone = inScoringZone ? (Time.time - enterScoringZoneTimestamp) : 0;
            return kills+(timeInCurrentZone+scoringTime)/lives;
        }
    }

    /// <summary>
    /// We died.  Reduce the score
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    internal void OnRespawn()
    {
        lives++;
    }

    /// <summary>
    /// We entered the scoring zone; start increasing the score.
    /// </summary>
    /// <param name="scoringZone"></param>
    internal void OnTriggerEnter2D(Collider2D scoringZone)
    {
        inScoringZone = true;
        enterScoringZoneTimestamp = Time.time;
    }

    /// <summary>
    /// We exited the scoring zone.  Stop increasing the score.
    /// </summary>
    /// <param name="scoringZone"></param>
    // ReSharper disable once UnusedMember.Global
    internal void OnTriggerExit2D(Collider2D scoringZone)
    {
        scoringTime += Time.time - enterScoringZoneTimestamp;
        inScoringZone = false;
    }

    public void ScoreKill()
    {
        kills++;
    }
}