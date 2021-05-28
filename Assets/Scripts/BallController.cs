using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    private Rigidbody _rigidbody;
    private Joystick _joystick;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _joystick = FindObjectOfType<Joystick>();
    }

    private void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        if (_joystick != null)
        {
            Vector2 vector = _joystick.Direction;
            x += vector.x;
            z += vector.y;
        }

        var camera = Application.isEditor ? Camera.main : Camera.current;

        var transformVector = camera.transform.TransformVector(new Vector3(x, 0f, z));

        float length = transformVector.magnitude;

        transformVector.y = 0;

        transformVector = transformVector.normalized * length;

        _rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, transformVector, _speed * Time.deltaTime);

        #region legacy
        //Vector3 direction = Vector3.zero;

        //if (Input.GetKey(KeyCode.W))
        //{
        //    direction = Camera.main.transform.up;
        //}
        //if (Input.GetKey(KeyCode.S))
        //{
        //    direction = Camera.main.transform.up * -1;
        //}
        //if (Input.GetKey(KeyCode.A))
        //{
        //    direction = Vector3.Cross(Camera.main.transform.up, Vector3.up);
        //}
        //if (Input.GetKey(KeyCode.D))
        //{
        //    direction = Vector3.Cross(Camera.main.transform.up, Vector3.up) * -1;
        //}

        //_rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, direction, _speed * Time.deltaTime);
        #endregion
    }
}
