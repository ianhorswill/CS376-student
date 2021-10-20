using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadSpring : MonoBehaviour
{
    public GameObject End1;
    public GameObject End2;
    public float Length = 5;
    public float Stiffness = 10;

    private Transform transform1;
    private Transform transform2;
    private Rigidbody2D rb1;
    private Rigidbody2D rb2;

    private LineRenderer lr;

    void Start()
    {
        transform1 = End1.GetComponent<Transform>();
        transform2 = End2.GetComponent<Transform>();
        rb1 = End1.GetComponent<Rigidbody2D>();
        rb2 = End2.GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var offset = transform1.position - transform2.position;
        var distance = offset.magnitude;
        var displacement = Length - distance;
        var force = Stiffness * (displacement / distance) * offset;
        rb1.AddForce(force);
        rb2.AddForce(-force);
        lr.SetPositions(new [] { transform1.position, transform2.position });
    }
}
