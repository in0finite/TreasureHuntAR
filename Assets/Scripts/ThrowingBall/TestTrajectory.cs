using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTrajectory : MonoBehaviour
{
    [SerializeField] private Rigidbody ThrowingBall;
    [SerializeField] private float Force = 10;
    [SerializeField] private int NumberOfPoints = 10;
    [SerializeField] private Transform PointPrefab;

    private List<Transform> _points = new List<Transform>();
    private Vector3 _startPos;

    private void Start()
    {
        for (int i = 0; i < NumberOfPoints; i++)
        {
            _points.Add(Instantiate(PointPrefab, transform.position, Quaternion.identity));
        }

        _startPos = ThrowingBall.transform.position;
    }

    private void OnMouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 50f))
        {
            // Draw line from camera to transparent sphere
            Debug.DrawLine(Camera.main.transform.position, hit.point, Color.red, 5f);

            // Draw line from hit point on transparent sphere to the center ball
            Debug.DrawLine(hit.point, ThrowingBall.transform.position, Color.green, 5f);
        }

        var throwingBallScreenPos = Camera.main.WorldToScreenPoint(ThrowingBall.position);
        var clickPositionOnScreen = Input.mousePosition;
        var differenceVector = clickPositionOnScreen - throwingBallScreenPos;

        Debug.Log($"Difference is: {differenceVector}");
        Debug.Log($"Throwing ball position on screen is: {throwingBallScreenPos}");
        Debug.Log($"click position on screen is: {clickPositionOnScreen}");
        Debug.DrawLine(clickPositionOnScreen, throwingBallScreenPos, Color.blue, 5f);

        var dir = ThrowingBall.transform.position - hit.point;

        for (int i = 0; i < NumberOfPoints; i++)
        {
            _points[i].position = GetPointPos(dir, i * 0.1f);
        }
    }

    private void OnMouseDrag()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 50f))
        {
            ThrowingBall.transform.position = hit.point;
        }
    }

    private Vector3 GetPointPos(Vector3 dir, float t)
    {
        // S = ut + 1/2 at^2
        var pointPos = _startPos + (dir.normalized * Force * t) + 0.5f * Physics.gravity * t * t;
        return pointPos;
    }
}
