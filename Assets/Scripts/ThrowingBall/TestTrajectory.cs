using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TestTrajectory : MonoBehaviour
{
    [SerializeField] private int _numberOfPoints = 10;
    [SerializeField] private float _timeOffset = 0.1f;

	private LineRenderer _trajectory;

    private void Awake()
    {
		_trajectory = GetComponent<LineRenderer>();
		if (_trajectory == null)
        {
			Debug.LogError("This component requires LineRenderer!");
        }
    }

    public void DrawTrajectory(Vector3 startPos, Vector3 dir, float force)
    {
        _trajectory.positionCount = _numberOfPoints;

        for (int i = 0; i < _numberOfPoints; i++)
        {
            var point = GetPointInTime(startPos, dir, force, i * _timeOffset);
            _trajectory.SetPosition(i, point);
        }
    }

	public void ResetTrajectory()
    {
        _trajectory.positionCount = 0;
    }

	// S = ut + 1/2 at^2
	private Vector3 GetPointInTime(Vector3 startPos, Vector3 dir, float force, float t)
    {
		var point = startPos + (dir.normalized * force * t) + 0.5f * Physics.gravity * t * t;
		return point;
	}
}
