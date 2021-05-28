using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlacementIndicator : MonoBehaviour
{
    private ARRaycastManager _raycastManager;
    [SerializeField] private GameObject _visual;

    private bool _instantiated = false;

    void Start()
    {
        _raycastManager = FindObjectOfType<ARRaycastManager>();
        _visual.SetActive(false);
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

                if (!_visual.gameObject.activeInHierarchy)
                {
                    _visual.gameObject.SetActive(true);

                    _instantiated = true;

                }
            }
        }
    }
}
