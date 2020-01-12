using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public class TouchMgr : MonoBehaviour
{
    public GameObject placeObject;

    private ARRaycastManager ratcastMgr;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    void Start()
    {
        ratcastMgr = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);
        if(touch.phase == TouchPhase.Began)
        {
            if(ratcastMgr.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
            {
                Instantiate(placeObject, hits[0].pose.position, hits[0].pose.rotation);
            }
        }
        
    }
}
