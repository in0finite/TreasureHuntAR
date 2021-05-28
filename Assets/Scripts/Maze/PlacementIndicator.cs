using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlacementIndicator : MonoBehaviour
{
    private ARRaycastManager _raycastManager;
    private GameObject _visual;

    void Start()
    {
        _raycastManager = FindObjectOfType<ARRaycastManager>();
        _visual.SetActive(false);
    }

    void Update()
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        _raycastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);

        if (hits.Count > 0)
        {
            transform.position = hits[0].pose.position;
            transform.rotation = hits[0].pose.rotation;


            if (!_visual.gameObject.activeInHierarchy)
            {
                _visual.gameObject.SetActive(true);
            }
        }
    }
}
