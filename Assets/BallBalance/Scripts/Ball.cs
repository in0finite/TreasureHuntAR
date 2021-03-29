﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public static Ball Singleton { get; private set; }

    private Rigidbody _rigidBody;

    public Transform startPosition;
    public float force = 1f;
    public Vector3 cameraOffset = new Vector3(0f, 0.2f, -0.2f);
    public float minYPos = -5f;

    private HashSet<Collider> _collidingColliders = new HashSet<Collider>();
    public bool IsColliding => _collidingColliders.Count > 0;


    void Awake()
    {
        Singleton = this;

        _rigidBody = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (this.transform.position.y < this.minYPos)
        {
            this.Respawn();
        }

        if (this.IsColliding)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");

            _rigidBody.AddForce(new Vector3(x, 0f, z) * this.force * Time.deltaTime, ForceMode.Impulse);
        }

        if (Camera.main != null)
        {
            Camera.main.transform.position = this.transform.position + this.cameraOffset;
            Camera.main.transform.LookAt(this.transform.position);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        _collidingColliders.Add(other.collider);
    }

    private void OnCollisionExit(Collision other)
    {
        _collidingColliders.Remove(other.collider);
    }

    public void Respawn()
    {
        this.transform.position = this.startPosition.position;
        _rigidBody.velocity = Vector3.zero;
        _rigidBody.angularVelocity = Vector3.zero;
    }
}