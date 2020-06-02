using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ObjectScale : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hitobj;
    private Vector3 touchPosition;
    private Vector3 direction;

    private Camera arCamera;

    
    void Start()
    {
        arCamera = GameObject.Find("AR Camera").GetComponent<Camera>();

    }

    // Update is called once per frame
    void Update()
    {
        Touch touch = Input.GetTouch(0);
        if (Input.touchCount == 0) return;
        //UI터치시 리턴
        if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(touch.fingerId))
        {
            return;
        }

        if (touch.phase == TouchPhase.Moved)
        {
            ray = arCamera.ScreenPointToRay(Input.GetTouch(0).position);

            //Xscale 조정
            if (Physics.Raycast(ray, out hitobj, 100.0f, 1 << 9))
            {
                
                Transform wallTr = hitobj.transform.parent;
                touchPosition = arCamera.ScreenToViewportPoint(touch.deltaPosition);
                Vector2 viewportPoint = arCamera.ScreenToViewportPoint(touch.position);
                if(viewportPoint.x > 0.5f)
                {
                    wallTr.localScale = new Vector3(wallTr.localScale.x + touchPosition.x, wallTr.localScale.y, wallTr.localScale.z);
                }
                else
                {
                    wallTr.localScale = wallTr.localScale + Vector3.left * touchPosition.x;

                }
            }

            //Zscale 조정
            if (Physics.Raycast(ray, out hitobj, 100.0f, 1 << 10))
            {
                Transform wallTr = hitobj.transform.parent;
                touchPosition = arCamera.ScreenToViewportPoint(touch.deltaPosition);
                Vector2 viewportPoint = arCamera.ScreenToViewportPoint(touch.position);

                if (viewportPoint.y > 0.5f)
                {
                    Debug.Log(wallTr.localScale);
                    wallTr.localScale = wallTr.localScale + Vector3.forward * touchPosition.y;
                }
                else
                {
                    Debug.Log(wallTr.localScale);

                    wallTr.localScale = wallTr.localScale + Vector3.back * touchPosition.y;
                }
            }
            //if (Physics.Raycast(ray, out hitobj, 100.0f, 1 << 11))
            //{
            //    Transform wallTr = hitobj.transform.parent;
            //    touchPosition = arCamera.ScreenToWorldPoint(touch.deltaPosition);
            //    wallTr.localPosition = touchPosition;
            //    //wallTr.position = new Vector3(wallTr.position.x + touchPosition.x, wallTr.position.y, wallTr.position.z + touchPosition.z);
            //}
        }
    }
}
