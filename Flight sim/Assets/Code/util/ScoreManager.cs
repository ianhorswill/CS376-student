using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Keeps track of the scores of the players.
/// </summary>
public class ScoreManager : MonoBehaviour {
    /// <summary>
    /// This is a singleton class (i.e. there's only supposed to be one instance)
    /// This makes it easy to find the one instance.
    /// </summary>
    private static ScoreManager theScoreScript;
    /// <summary>
    /// GameObjects for the players
    /// </summary>
    public GameObject[] Players;
    /// <summary>
    /// UI elements in which to display the respective players' scores.
    /// </summary>
    public Text[] ScoreFields;

    /// <summary>
    /// Scores for the different players
    /// </summary>
    private int[] scores;

    /// <summary>
    /// Initialize component
    /// </summary>
    internal void Start(){
        theScoreScript = this;
        scores = new int[Players.Length];
        UpdateText();
    }

    /// <summary>
    /// Position in the Players, Scores, and ScoreFields arrays of player
    /// </summary>
    /// <param name="player">Player to find</param>
    /// <returns>Index into the arrays</returns>
    static int PlayerNumber(GameObject player)
    {
        var playerNumber = Array.IndexOf(theScoreScript.Players, player);
        if (playerNumber < 0)
            Debug.Log("Unknown player: "+player.name);
        return playerNumber;
    }

    /// <summary>
    /// Increase the score for the designated player
    /// </summary>
    /// <param name="player">Player</param>
    /// <param name="val">Score</param>
    public static void IncreaseScore(GameObject player, int val)
    {
        var playerNumber = PlayerNumber(player);
        if (playerNumber>=0)
            theScoreScript.scores[playerNumber] += val;
        theScoreScript.UpdateText();
    }

    /// <summary>
    /// Update all the score fields
    /// </summary>
    private void UpdateText(){
        for (int i=0; i<Players.Length; i++)
            ScoreFields[i].text = string.Format("{0}: {1}", Players[i].name, scores[i]);
    }
}
