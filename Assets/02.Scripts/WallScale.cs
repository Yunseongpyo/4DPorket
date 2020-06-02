using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class WallScale : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hitobj;
    private Vector3 touchPosition;
    private Vector3 direction;

    private Camera arCamera;

    private Color originMatcolor;
    private bool isselected;

    //처음 터치 시작 지점
    private Vector2 startviewportPoint;

    public Scrollbar scaleBar;

    void Start()
    {
        arCamera = GameObject.Find("AR Camera").GetComponent<Camera>();
    }
    void Update()
    {
        Touch touch = Input.GetTouch(0);
        if (Input.touchCount == 0) return;
        //UI터치시 리턴
        if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(touch.fingerId))
        {
            return;
        }

        if(touch.phase == TouchPhase.Began)
        {
            startviewportPoint = arCamera.ScreenToViewportPoint(touch.position);
            if (Physics.Raycast(ray, out hitobj, 100.0f, 1 << 9) && isselected == false)
            {
                Material selectMat = hitobj.collider.gameObject.GetComponent<MeshRenderer>().material;
                originMatcolor = selectMat.color;
                selectMat.SetColor("_COLOR", Color.red);
                isselected = true;
                Debug.Log(selectMat.color);
            }
            else if (Physics.Raycast(ray, out hitobj, 100.0f, 1 << 9) && isselected == true)
            {
                Material selectMat = hitobj.collider.gameObject.GetComponent<MeshRenderer>().material;
                selectMat.SetColor("_COLOR", originMatcolor);
                isselected = false;
            }
        }

        if (touch.phase == TouchPhase.Moved)
        {

            ray = arCamera.ScreenPointToRay(Input.GetTouch(0).position);
            //SideScale 조정
            if (Physics.Raycast(ray, out hitobj, 100.0f, 1<<9) && isselected == true)
            {
                Transform wallTr = hitobj.transform.parent;
                touchPosition = arCamera.ScreenToViewportPoint(touch.deltaPosition);
                Vector2 viewportPoint = arCamera.ScreenToViewportPoint(touch.position);
                Debug.Log(touch.deltaPosition);
                wallTr.localScale = wallTr.localScale + Vector3.right * touchPosition.x;

            }
            //if (Physics.Raycast(ray, out hitobj, 100.0f, 1 << 13))
            //{
            //    Transform wallTr = hitobj.transform.parent;
            //    touchPosition = arCamera.ScreenToViewportPoint(touch.deltaPosition);
            //    Vector2 viewportPoint = arCamera.ScreenToViewportPoint(touch.position);
            //    Debug.Log(touch.deltaPosition);
            //    wallTr.localScale = wallTr.localScale + Vector3.up * touchPosition.y;
            //}
            //if (Physics.Raycast(ray, out hitobj, 100.0f, 1 << 10))
            //{
            //    Transform wallTr = hitobj.transform.parent;
            //    touchPosition = arCamera.ScreenToViewportPoint(touch.deltaPosition);
            //    Vector2 viewportPoint = arCamera.ScreenToViewportPoint(touch.position);
            //    Debug.Log(touch.deltaPosition);
            //    wallTr.localScale = wallTr.localScale + Vector3.forward * touchPosition.y;
            //}

            //arSessionOrigin.MakeContentAppearAt(hitWall.transform, hits[0].pose.position, hits[0].pose.rotation);

        }
    }
}
