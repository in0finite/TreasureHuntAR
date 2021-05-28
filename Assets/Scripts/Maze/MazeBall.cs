using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeBall : MonoBehaviour
{
    [SerializeField] private GameObject _ballPrefab;

    public void Init(Vector3 position)
    {
        var ball = Instantiate(_ballPrefab, transform, false);
        var pos = position;
    }
}
