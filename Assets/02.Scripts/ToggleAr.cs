using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;

public class ToggleAr : MonoBehaviour
{
    public ARPlaneManager planeAR;
    public ARPointCloudManager pointAR;

    public void OnvisualChanged(bool isOn)
    {
        VisuallizePlanes(isOn);
        VisuallizePoints(isOn);
    }

    private void VisuallizePlanes(bool active)
    {
        planeAR.enabled = active;
        foreach (ARPlane plane in planeAR.trackables)
        {
            plane.gameObject.SetActive(active);
        }
    }

    private void VisuallizePoints(bool active)
    {
        pointAR.enabled = active;
        foreach (ARPointCloud point in pointAR.trackables)
        {
            point.gameObject.SetActive(active);
        }
    }
}
