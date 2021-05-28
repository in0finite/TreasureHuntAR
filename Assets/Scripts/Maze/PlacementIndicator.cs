using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlacementIndicator : MonoBehaviour
{
    private ARRaycastManager _raycastManager;
    [SerializeField] private GameObject _visual;
    [SerializeField] private GameObject _ball;

    private bool _instantiated = false;

    void Start()
    {
        _raycastManager = FindObjectOfType<ARRaycastManager>();
        _visual.SetActive(false);
        _ball.SetActive(false);
    }

    void Update()
    {
        if (_instantiated == false)
        {
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            _raycastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);

            if (hits.Count > 0)
            {
                _visual.transform.position = hits[0].pose.position;
                _visual.transform.rotation = hits[0].pose.rotation;

                if (!_visual.activeInHierarchy)
                {
                    _visual.SetActive(true);
                    _ball.SetActive(true);

                    _instantiated = true;

                }
            }
        }
    }
}
