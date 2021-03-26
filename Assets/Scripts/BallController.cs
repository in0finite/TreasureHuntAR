using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            direction = Camera.main.transform.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction = Camera.main.transform.up * -1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction = Vector3.Cross(Camera.main.transform.up, Vector3.up);
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction = Vector3.Cross(Camera.main.transform.up, Vector3.up) * -1;
        }

        _rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, direction, _speed * Time.deltaTime);
    }
}
