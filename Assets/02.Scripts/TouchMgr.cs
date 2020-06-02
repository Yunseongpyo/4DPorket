using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;
using System.IO;

[System.Serializable]
public class PlaceData
{
    public GameObject SaveObject;
}

public class TouchMgr : MonoBehaviour
{
    //싱글톤 선언
    public static TouchMgr instance;

    //y스케일 버튼 static 선언
    public static bool ischeckYscale;
    //생성될 게임 오브젝트
    public GameObject placeObject;

    //ar 컴퍼넌트
    private ARSessionOrigin arSessionOrigin;
    private ARRaycastManager ratcastMgr;
    private ARPlaneManager arPlaneManager;

    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private bool ischeckUI = false;
    private bool isActive = false;

    //공간 저장소 
    public static Dictionary<TrackableId, GameObject> spwanedObjs = new Dictionary<TrackableId, GameObject>();
    private List<TrackableId> tempTrackId = new List<TrackableId>();

    public static GameObject tempGameObject;



    //arplane 갯수
    private List<ARPlane> addPlanes;
    private List<ARPlane> removedPlanes;

    //현재 선택된 트랙아이디
    public static TrackableId trackidNow = TrackableId.invalidId;

    //UI오브젝트
    public GameObject UIObject;

    //카메라 컴터넌트
    private Camera arCamera;
    private Ray ray;
    private RaycastHit cameraHit;

    PlaceData placedata = new PlaceData();


    void Awake()
    {
        instance = this;
        ratcastMgr = GetComponent<ARRaycastManager>();
        arSessionOrigin = GetComponent<ARSessionOrigin>();
        arPlaneManager = GetComponent<ARPlaneManager>();
        arCamera = GameObject.Find("AR Camera").GetComponent<Camera>();
        //arPlaneManager.planesChanged += SpawnObjests;
        ARSession.stateChanged += OnStateChanged;


    }
    //arsession이 활성화 되어 있는지 확인 가능, 활성준비가 안되어 있으면 업데이트문 탈출
    //arsession은 무조건 씬에 하나만 있기 때문에 정적필드를 통해 arsession값을 가지고 올수 있음(변수 선언 필요없음)



    private void SpawnObjests(ARPlanesChangedEventArgs args)
    {
        addPlanes = args.added;
        removedPlanes = args.removed;
        if (addPlanes.Count > 0)
        {
            foreach (ARPlane plane in addPlanes)
            {
                //hitWall = Instantiate(placeObject, plane.center, plane.transform.rotation);
                Debug.Log("플랜 아이디 :" + plane.trackableId);
            }
        }
        if (removedPlanes.Count > 0)
        {
            Debug.Log("리무브");
            foreach (ARPlane plane in removedPlanes)
            {
                GameObject destoryObj = spwanedObjs[plane.trackableId];
                Destroy(destoryObj);
            }
        }

    }


    private void OnStateChanged(ARSessionStateChangedEventArgs args)
    {
        //Debug.Log(args.state);

        //ar을 지원하지 않을 경우 앱 종료
        if (args.state == ARSessionState.Unsupported)
        {
            Application.Quit();
            return;
        }

        if (args.state == ARSessionState.Ready)
        {
            isActive = true;
        }

    }
    
   
    void Update()
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

        if (touch.phase == TouchPhase.Began)
        {
            ray = arCamera.ScreenPointToRay(touch.position);
            if (Physics.Raycast(ray, out cameraHit, 100.0f))
            {
                if(cameraHit.collider.CompareTag("SPACE"))
                {
                    ischeckUI = !ischeckUI;
                    //UIObject.SetActive(ischeckUI);
                    Debug.Log(ischeckUI);
                }
            }

            if (ratcastMgr.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
            {
                if (hits[0].trackableId != trackidNow) // 현재 터치한 구간이 새로생긴 경우 
                {
                    trackidNow = hits[0].trackableId; // 현재 트랙 아이디 저장
                    foreach (TrackableId ischeckId in spwanedObjs.Keys) // 기존 아이디에 저장될 경우 리턴(한면당 한공간만 생성)
                    {
                        if (ischeckId == trackidNow)
                        {
                            return;
                        }
                    }
                    tempGameObject = Instantiate(placeObject, hits[0].pose.position, hits[0].pose.rotation);
                    Debug.Log("찍은 좌표" + tempGameObject.transform.position);

                    spwanedObjs.Add(trackidNow, tempGameObject); // 터치 아이디 및 오브젝트 추가

                    //Debug.Log("현재아이디 : " + trackidNow);
                }
                else if (hits[0].trackableId == trackidNow) // 현재 터치 구간일 경우 이동 가능
                {
                    Debug.Log("찍은 좌표" + hits[0].pose.position);
                    //tempGameObject.transform.Translate(hits[0].pose.position);
                    arSessionOrigin.MakeContentAppearAt(tempGameObject.transform, hits[0].pose.position, tempGameObject.transform.rotation);
                }
            }
          
        }

    }
}

