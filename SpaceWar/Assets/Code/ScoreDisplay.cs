using UnityEngine;

/// <summary>
/// Display the current score
/// </summary>
// ReSharper disable once UnusedMember.Global
public class ScoreDisplay : MonoBehaviour
{
    public GUIStyle Style;
    public Rect Rect = new Rect(100, 100, 300, 100);

    private ScoreKeeper Player;

    internal void Start()
    {
        Player = GetComponent<ScoreKeeper>();
    }

    /// <summary>
    /// Display the score
    /// Called once each GUI update.
    /// </summary>
    internal void OnGUI()
    {
        GUI.Label(Rect, $"Score: {Player.Score:F3}", Style);
    }
}
