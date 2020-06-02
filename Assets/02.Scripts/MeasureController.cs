using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;
using UnityEngine.EventSystems;

public class MeasureController : MonoBehaviour
{
    [SerializeField] // 측정 포인트 Prefab
    private GameObject measurementPointPrefab;

    [SerializeField] // 측정 거리 표시
    private TextMeshProUGUI distanceText;

    [SerializeField] // 벽 오브젝트 프리팹
    private GameObject wallPrefab;


    // 측정 좌표
    public TextMeshProUGUI starPointText;
    public TextMeshProUGUI endPointText;

    

    //측정 포인트
    private GameObject startPoint;
    private GameObject endPoint;

    //이벤트용 리스트
    private Dictionary<bool, TrackableId> planeTrackId;
    private bool isCheckOnWall;

    //수직 수평 확인용 불값
    private bool isCheckVertical;


    //라인 렌더러
    private LineRenderer measureline;


    //ar 컴퍼넌트
    private ARRaycastManager arRaycastManager;
    private ARPlaneManager arPlaneManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    //카메라 컴터넌트
    private Camera arCamera;
    private Ray ray;
    private RaycastHit cameraHit;

    private TrackableId trackidBefore = TrackableId.invalidId;


    private void Awake()
    {
        arRaycastManager = this.GetComponent<ARRaycastManager>();
        arPlaneManager = this.GetComponent<ARPlaneManager>();

        startPoint = Instantiate(measurementPointPrefab, Vector3.zero, Quaternion.identity);
        endPoint = Instantiate(measurementPointPrefab, Vector3.zero, Quaternion.identity);

        measureline = this.GetComponent<LineRenderer>();

        arCamera = GameObject.Find("AR Camera").GetComponent<Camera>();


        startPoint.SetActive(false);
        endPoint.SetActive(false);

    }
  


    void Update()
    {
        if (Input.touchCount == 0)
        {
            return;
        }
        Touch touch = Input.GetTouch(0);

        //UI터치시 리턴
        if (Input.touchCount >0 && EventSystem.current.IsPointerOverGameObject(touch.fingerId))
        {
            return;
        }

        if (touch.phase == TouchPhase.Began)
        {
            ray = arCamera.ScreenPointToRay(touch.position);
            //if(arRaycastManager.Raycast(ray, hits, TrackableType.PlaneWithinInfinity))
            //{

            //}

            if (arRaycastManager.Raycast(ray, hits, TrackableType.PlaneEstimated))
            {
                if (arPlaneManager.GetPlane(hits[0].trackableId).alignment == PlaneAlignment.Vertical)
                {
                    isCheckVertical = true;
                }
                else
                {
                    isCheckVertical = false;
                }

                if (hits[0].trackableId != trackidBefore)
                {
                    trackidBefore = hits[0].trackableId;
                    isCheckOnWall = true;
                }
                else if (hits[0].trackableId == trackidBefore && isCheckOnWall == false)
                {
                    return;
                }

                //측정 포인트 생성
                startPoint.SetActive(true);
                Pose hitpose = hits[0].pose;
                startPoint.transform.SetPositionAndRotation(hitpose.position, hitpose.rotation);
            }
        }
        if(touch.phase == TouchPhase.Moved)
        {
            ray = arCamera.ScreenPointToRay(touch.position);

            if (!isCheckOnWall)
            {
                return;
            }
            if (arRaycastManager.Raycast(ray, hits, TrackableType.PlaneEstimated))
            {
                measureline.gameObject.SetActive(true);
                endPoint.SetActive(true);
                Pose hitpose = hits[0].pose;
                endPoint.transform.SetPositionAndRotation(hitpose.position, hitpose.rotation);
            }
        }
        if (startPoint.activeSelf && endPoint.activeSelf)
        {

            measureline.SetPosition(0, startPoint.transform.position);
            measureline.SetPosition(1, endPoint.transform.position);

            float distance = Vector3.Distance(startPoint.transform.position, endPoint.transform.position);

            distanceText.text = distance.ToString();
            starPointText.text = "StartPoint" + startPoint.transform.position.ToString();
            endPointText.text = "Vertical" + isCheckVertical.ToString();

            Debug.Log("Vertical : " + isCheckVertical);

        }

    }

    public void MakeWallButton()
    {
        MakeWall(startPoint.transform.position, endPoint.transform.position);
    }

    private void MakeWall(Vector3 _startPoint, Vector3 _endPoint)
    {
        float startPointX = _startPoint.x;
        float startPointY = _startPoint.y;
        float startPointZ = _startPoint.z;

        float endPointX = _endPoint.x;
        float endPointY = _endPoint.y;
        float endPointZ = _endPoint.z;

        float wallX = Mathf.Abs(startPointX - endPointX);
        float wallY = Mathf.Abs(startPointY - endPointY);
        float wallZ = Mathf.Abs(startPointZ - endPointZ);

        Vector3 wallPrefabPos = Vector3.Lerp(_startPoint, _endPoint, 0.5f);

        GameObject wall = Instantiate(wallPrefab, wallPrefabPos, Quaternion.Euler(0,0,0));

        if (isCheckVertical == true)
        {
            wall.transform.localScale = new Vector3(wall.transform.localScale.x, wallY, wallZ);

        }
        else if (isCheckVertical == false)
        {
            wall.transform.localScale = new Vector3(wallX, wall.transform.localScale.y, wallZ);
        }

        startPoint.SetActive(false);
        endPoint.SetActive(false);
        isCheckOnWall = false;
        measureline.SetPosition(0, Vector3.zero);
        measureline.SetPosition(1, Vector3.zero);
        //Destroy(arPlaneManager.GetPlane(trackidBefore).gameObject);

    }
}
