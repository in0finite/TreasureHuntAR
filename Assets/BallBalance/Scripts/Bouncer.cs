﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : MonoBehaviour
{
    public float force = 1f;
    public float timeInterval = 0.5f;

    private float _timeWhenAppliedForce;


    private void OnCollisionEnter(Collision other)
    {
        if (null == other.rigidbody)
            return;

        if (Time.time - _timeWhenAppliedForce < this.timeInterval)
            return;

        _timeWhenAppliedForce = Time.time;

        Vector3 newVelocity = Vector3.Reflect(other.rigidbody.velocity, this.transform.up);
        newVelocity += this.transform.up * this.force;

        other.rigidbody.velocity = newVelocity;
    }
}