using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ScrollBarController : MonoBehaviour
{
    private void Update()
    {

        if (Input.touchCount == 0)
        {
            return;
        }
        Touch touch = Input.GetTouch(0);

        if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(touch.fingerId)) // UI위에서는 터치가 안되게 하기
        {
            return;
        }

        if(touch.phase == TouchPhase.Began)
        {

        }
    }
}
    
   