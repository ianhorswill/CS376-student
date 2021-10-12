using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Vector2 UpForce;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.UpArrow))
            GetComponent<Rigidbody2D>().AddForce(UpForce);
    }
}
