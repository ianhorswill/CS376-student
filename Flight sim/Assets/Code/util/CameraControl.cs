using UnityEngine;

public class CameraControl : MonoBehaviour {

    public Transform target;
    public float smoothing = 5f;

    private Vector3 offset;

	internal void Start () {
        offset = transform.position - target.position;
	}
	
	// okay so we need to use fixedupdate if we move with physics and do this in update if we move the shitty way
	internal void FixedUpdate () {
        Vector3 targetCamPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
	}
}
