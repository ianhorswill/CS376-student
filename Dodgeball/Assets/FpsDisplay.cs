using TMPro;
using UnityEngine;

/// <summary>
/// Sets the text of whatever text component is in the same game object as this one.
/// </summary>
[RequireComponent(typeof(TMP_Text))]
public class FpsDisplay : MonoBehaviour
{
    /// <summary>
    /// The text component itself
    /// We look this up at Start time so we don't have to do the lookup every frame
    /// </summary>
    private TMP_Text textDisplay;

    /// <summary>
    /// Find the text component
    /// </summary>
    // ReSharper disable once UnusedMember.Local
    void Start()
    {
        textDisplay = GetComponent<TMP_Text>();
    }

    /// <summary>
    /// Update the text component to display the current number of frames per second
    /// </summary>
    // ReSharper disable once UnusedMember.Local
    void Update()
    {
        textDisplay.text = $"{1 / Time.deltaTime:0.00} frames/second";
    }
}
