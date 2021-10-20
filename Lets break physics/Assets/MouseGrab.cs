using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseGrab : MonoBehaviour
{
    public void OnMouseDrag()
    {
        var mouseLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mouseLocation.x, mouseLocation.y, transform.position.z);
    }
}
