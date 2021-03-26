using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (Ball.Singleton == null)
            return;

        if (other.gameObject != Ball.Singleton.gameObject)
            return;

        Ball.Singleton.Respawn();
    }
}
