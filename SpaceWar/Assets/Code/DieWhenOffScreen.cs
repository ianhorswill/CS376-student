using UnityEngine;

/// <summary>
/// Causes the attached object to die if it moves off screen.
/// </summary>
public class DieWhenOffScreen : MonoBehaviour {
    /// <summary>
    /// Called once per frame.
    /// </summary>
	internal void Update ()
	{
	    // Check if the attached GameObject is outside the screen bounds, and if so, kill it.
	    var screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPosition.x<-10 
            || screenPosition.y<-10 
            || screenPosition.y > Screen.height+10 
            || screenPosition.x > Screen.width+10)
            Destroy(gameObject);
	}
}
