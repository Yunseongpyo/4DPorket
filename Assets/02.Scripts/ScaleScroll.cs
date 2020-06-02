using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;

public class ScaleScroll : MonoBehaviour
{
    public Scrollbar scaleBar;
    private Transform[] hitwallList;
    private GameObject spaceObject;

    private bool ischeck;

    private List<GameObject> hitwallObjets = new List<GameObject>();
    public enum ScaleDicretion
    {
        RIGHT_LEFT, HEIGHT, FORWARD_BACK
    };
    [SerializeField]
    private ScaleDicretion scaleDirection;

    private float angleincrement;

    //자식 오브젝트 저장하기
    private List<GameObject> ChageGameobject(Transform[] _list)
    {
        List<GameObject> temp = new List<GameObject>();
        for (int i = 1; i < _list.Length; i++)
        {
            temp.Add(_list[i].gameObject);
        }
        return temp;
    }

    private void InitColor()
    {
        for (int i = 1; i < 5; i++)
        {
            hitwallObjets[i].GetComponent<MeshRenderer>().material.SetColor("_COLOR", Color.white);
        }

    }
    private void Update()
    {
       
        Touch touch = Input.GetTouch(0);
        if(TouchMgr.spwanedObjs != null)
        {
            spaceObject = TouchMgr.spwanedObjs[TouchMgr.trackidNow].gameObject;
            hitwallList = spaceObject.GetComponentsInChildren<Transform>();
            hitwallObjets = ChageGameobject(hitwallList);
        }

        if (touch.phase == TouchPhase.Ended)
        {
            scaleBar.value = 0.5f;
            InitColor();
        }
        if (touch.phase == TouchPhase.Moved)
        {
            //if(spaceObject.transform.localScale.x <0.001f || spaceObject.transform.localScale.y < 0.001f || spaceObject.transform.localScale.z < 0.001f)
            //{
            //    spaceObject.transform.localScale = new Vector3(0.005f, 0.005f, 0.005f);

            //    return;
            //}
            if (scaleBar.value > 0.5f)
            {
                switch (scaleDirection)
                {
                    case ScaleDicretion.RIGHT_LEFT:
                        hitwallObjets[1].GetComponent<MeshRenderer>().material.SetColor("_COLOR", Color.red);
                        hitwallObjets[2].GetComponent<MeshRenderer>().material.SetColor("_COLOR", Color.red);
                        spaceObject.transform.localScale = spaceObject.transform.localScale + Vector3.right * 0.01f;
                        break;
                    case ScaleDicretion.FORWARD_BACK:
                        hitwallObjets[3].GetComponent<MeshRenderer>().material.SetColor("_COLOR", Color.red);
                        hitwallObjets[4].GetComponent<MeshRenderer>().material.SetColor("_COLOR", Color.red);
                        spaceObject.transform.localScale = spaceObject.transform.localScale + Vector3.forward * 0.01f;
                        break;
                    case ScaleDicretion.HEIGHT:
                        hitwallObjets[1].GetComponent<MeshRenderer>().material.SetColor("_COLOR", Color.red);
                        hitwallObjets[2].GetComponent<MeshRenderer>().material.SetColor("_COLOR", Color.red);
                        hitwallObjets[3].GetComponent<MeshRenderer>().material.SetColor("_COLOR", Color.red);
                        hitwallObjets[4].GetComponent<MeshRenderer>().material.SetColor("_COLOR", Color.red);
                        spaceObject.transform.localScale = spaceObject.transform.localScale + Vector3.up * 0.01f;
                        break;
                }

            }
            else if (scaleBar.value < 0.5f)
            {
                switch (scaleDirection)
                {
                    case ScaleDicretion.RIGHT_LEFT:
                        hitwallObjets[1].GetComponent<MeshRenderer>().material.SetColor("_COLOR", Color.red);
                        hitwallObjets[2].GetComponent<MeshRenderer>().material.SetColor("_COLOR", Color.red);
                        spaceObject.transform.localScale = spaceObject.transform.localScale + Vector3.left * 0.01f;
                        break;
                    case ScaleDicretion.FORWARD_BACK:
                        hitwallObjets[3].GetComponent<MeshRenderer>().material.SetColor("_COLOR", Color.red);
                        hitwallObjets[4].GetComponent<MeshRenderer>().material.SetColor("_COLOR", Color.red);
                        spaceObject.transform.localScale = spaceObject.transform.localScale + Vector3.back * 0.01f;
                        break;
                    case ScaleDicretion.HEIGHT:
                        hitwallObjets[1].GetComponent<MeshRenderer>().material.SetColor("_COLOR", Color.red);
                        hitwallObjets[2].GetComponent<MeshRenderer>().material.SetColor("_COLOR", Color.red);
                        hitwallObjets[3].GetComponent<MeshRenderer>().material.SetColor("_COLOR", Color.red);
                        hitwallObjets[4].GetComponent<MeshRenderer>().material.SetColor("_COLOR", Color.red);
                        spaceObject.transform.localScale = spaceObject.transform.localScale + Vector3.down * 0.01f;
                        break;
                }
            }
        }
    }


}

