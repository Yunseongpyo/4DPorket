using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenCameraController : MonoBehaviour
{
    public Camera scrennCamera;
    private GameObject thisObject;
    float xscale, zscale, cameraYpos, test;
    
    void Update()
    {
        cameraYpos = this.gameObject.transform.localScale.x * 1.7f + this.gameObject.transform.localScale.z * 1.7f;
        scrennCamera.transform.localPosition = new Vector3(scrennCamera.transform.localPosition.x, cameraYpos, scrennCamera.transform.localPosition.z);
    }
}
