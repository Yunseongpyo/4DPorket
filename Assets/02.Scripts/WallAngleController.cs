using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class WallAngleController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    bool check;
    

    public void OnPointerDown(PointerEventData eventData)
    {
        check = true;
        TouchMgr.ischeckYscale = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        check = false;
        TouchMgr.ischeckYscale = false;
    }

    private void Update()
    {
        
        if(check)
        {
            if(this.gameObject.CompareTag("ANGLERIGHT"))
            {
                TouchMgr.spwanedObjs[TouchMgr.trackidNow].gameObject.transform.Rotate(0, 1, 0);
            }
            else if (this.gameObject.CompareTag("ANGLELEFT"))
            {
                TouchMgr.spwanedObjs[TouchMgr.trackidNow].gameObject.transform.Rotate(0, -1, 0);
            }
        }
    }
}

