using UnityEngine;

// Simple sequencer to play an explosion animation, apply forces to objects in the areas,
// and then delete itself.
public class ExplosionController : MonoBehaviour
{
    // How long to apply forces for.
    public float ExplosionDuration = 0.1f;

    // Apply forces, and set self-destruct timer
	internal void Start ()
	{
	    GetComponent<PointEffector2D>().enabled = true;
        Invoke("SelfDestruct", ExplosionDuration);
	}

    // Get rid of this game object
	internal void SelfDestruct() {
	    Destroy(gameObject);
	}
}
