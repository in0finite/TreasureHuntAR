using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour
{
    public static Goal Singleton { get; private set; }

    public Text text;

    public bool IsGameOver { get; private set; }

    private void Awake()
    {
        Singleton = this;

        this.text.enabled = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (IsGameOver)
            return;

        IsGameOver = true;

        Destroy(other.gameObject);

        this.text.enabled = true;
    }
}
