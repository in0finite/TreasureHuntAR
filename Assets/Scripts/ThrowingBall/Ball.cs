using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Ball : MonoBehaviour
{
    public float forceMultiplier = 3;
    public float maxPullDistance = 150;

    private Vector3 mousePressDownPos;
    private Vector3 mouseReleasePos;
    private Vector3 startPosition;

    private Rigidbody rb;
    private Transform t;

    private bool isShot;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        t = GetComponent<Transform>();
        startPosition = t.position;
    }

    private void OnMouseDown()
    {
        mousePressDownPos = Input.mousePosition;
    }

    private void OnMouseUp()
    {
        mouseReleasePos = Input.mousePosition;
        rb.useGravity = true;
        Vector3 force = mousePressDownPos - mouseReleasePos;
        Shoot(force.magnitude > maxPullDistance ? force.normalized * maxPullDistance : force);
    }

    private void Shoot(Vector3 force)
    {
        if (isShot)
            return;
        rb.AddForce(new Vector3(force.x, force.y, force.y) * forceMultiplier);
        isShot = true;
    }
}
