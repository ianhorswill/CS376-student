using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

/// <summary>
/// Controls a BarGraph widget
/// BarGraph widgets are GameObjects that have one of these as a component, as well as an Image (like a sprite but different)
/// and a text field.  There's a prefab for the widget in the Resources folder.
/// </summary>
public class BarGraph : MonoBehaviour
{
    /// <summary>
    /// The Image component of the bar part of the widget
    /// An Image is like a SpriteRenderer but different
    /// This is the literal bar that grows and shrinks
    /// </summary>
    public Image BarImage;
    /// <summary>
    /// This is the transform of the GameObject containing BarImage.
    /// Changing its scale changes the size of the bar.
    /// </summary>
    public RectTransform BarTransform;

    /// <summary>
    /// The text component of the widget
    /// Changing its .text field changes what's displayed.
    /// </summary>
    public TMP_Text Text;

    /// <summary>
    /// The minimum allowable value for this widget
    /// Anything less will make the widget display in red
    /// </summary>
    public float Min;
    /// <summary>
    /// The maximum allowable value for this widget
    /// Anything larger will make the widget display in red.
    /// </summary>
    public float Max = 1;

    // True if we're allowing negative values for this bar graph
    private bool signedDisplay;
    
    // Start is called before the first frame update
    // ReSharper disable once UnusedMember.Local
    void Start()
    {
        // TODO: Set the text to the name of this game object
        
        
        // This sets width to the width of the widget on screen
        var rectTransform = (RectTransform)transform;
        var width = rectTransform.sizeDelta.x * rectTransform.localScale.x;

        // TODO: If we're going to display signed numbers, then move the bar to the middle of the widget
        // and set signedDisplay to true.
        //
        // You can figure out if we're displaying signed numbers by looking at Min and/or Max.
        // Do they allow for the value being negative?
        //
        // You can move the bar to the middle of the widget by setting the x coordinate of its localPosition
        // to half the width (see above) of the widget.
        //
        // Important: remember that This BarGraph component is in a different game object than the
        // Bar.  So they have different RectTransforms.  How do you get the transform for the bar?
        
    }

    /// <summary>
    /// Change the value displayed in the bar graph
    /// This should both change the bar itself and also the text displayed below it.
    /// </summary>
    /// <param name="value">Value to display</param>
    public void SetReading(float value)
    {
        // TODO: Determine the color to display it in.
        // If it's out of range, display it in red
        // Otherwise, use green for positive values and blue for negative ones
        var color = Color.green;
        
        // TODO: if value is out of range (less than Min, greater than Max),
        // then move it in range (set it to Min/Max) so the bar doesn't draw
        // outside the widget.

        // TODO: Call SetWidthPercent to change the width of the bar and set its color
        
        // TODO: Update the text to read: {name} : {value}
        
    }

    /// <summary>
    /// Set the bar to value percent of its maximum width
    /// value = 0 means its zero-width
    /// value = 1 means it's maximum width
    /// value = -1 (signed display) means it's maximum width in the other direction
    /// </summary>
    /// <param name="value">Percentage of the maxim</param>
    /// <param name="c"></param>
    public void SetWidthPercent(float value, Color c)
    {
        // TODO: Set the color of the bar to c
        

        // TODO: Change BarTransform.localScale so that its x component is scaled by value.
        // If we're using signedDisplay, then we also want to cut the scale by a half so we can 
        // have half the widget for positive values and half for negative ones.
        // Leave the localScale's y component as is.
        
    }

    #region Dynamic creation
    /// <summary>
    /// Table of all the bar graphs made using the Find method below.
    /// </summary>
    // ReSharper disable once ArrangeObjectCreationWhenTypeEvident
    private static readonly Dictionary<string, BarGraph> BarGraphTable = new Dictionary<string, BarGraph>();

    /// <summary>
    /// Find the bar graph stored with the specified name and return it.
    /// If there isn't one, then make one in the specified position and give it the specified
    /// min and max values.  Then store it in the table so we can find it in the future.
    /// </summary>
    /// <param name="name">Name of the bar graph</param>
    /// <param name="position">Where to put it on the screen if it hasn't been made already</param>
    /// <param name="min">Minimum value for it to display</param>
    /// <param name="max">Maximum value for it to display</param>
    /// <returns></returns>
    public static BarGraph Find(string name, Vector2 position, float min, float max)
    {
        // TODO: Check if we've already made a bargraph of this name.  If so, return it.
        
        //
        // Otherwise, we need to make a new one
        //

        // The UI system requires that all UI widgets be inside of the GameObject that has the Canvas component.
        // So find the canvas component
        var canvas = FindObjectOfType<Canvas>();

        // TODO: Instantiate Prefab and put it inside of the game object that has the canvas.
        // Set its position to position and its rotation to the magic value Quaternion.identity, which means
        // "don't rotate it".
        GameObject go = null;  // Change null to a call to Instantiate

        // TODO: Give the GameObject the specified name
        

        // TODO: Get the BarGraph component from the game object we just made
        BarGraph bgComponent = null;  // Change null here

        // TODO set bgComponent's Min and Max fields to min and max
        
        // Add the BarGraph component to the table
        BarGraphTable[name] = bgComponent;

        // Return the BarGraph component
        return bgComponent;
    }

    // This is where we stash the prefab once we've loaded it so we don't have to keep loading it.
    private static GameObject prefab;

    // Return the prefab for bar graphs
    private static GameObject Prefab
    {
        get
        {
            // TODO: return prefab is null, set it to Resources.Load<GameObject>("BarGraph")


            // Now that prefab isn't null, we can return it.
            return prefab;
        }
    }
    #endregion
}
