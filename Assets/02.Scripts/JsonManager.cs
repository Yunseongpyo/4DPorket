using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.XR.ARSubsystems;
using Newtonsoft.Json;
using TMPro;


public class JasonData
{
    public Vector3 saveDatas;
    public Vector3 saveDatasPosition;
    public JasonData(Vector3 saveDatas, Vector3 saveDatasPosition)
    {
        this.saveDatas = saveDatas;
        this.saveDatasPosition = saveDatasPosition;

    }
}


public class JsonManager : MonoBehaviour
{
    private bool ischeckFrist = true;

    public Camera capctureCamera;

    public TextMeshProUGUI loadText;
    public GameObject saveobject;
    public GameObject saveUIObject;
    List<JasonData> data = new List<JasonData>();
    Dictionary<GameObject, Vector3> saveDataObjects = new Dictionary<GameObject, Vector3>();

    string path;

    private void Awake()
    {
        capctureCamera.enabled = false;
    }
    private void Start()
    {
        path = Application.persistentDataPath + "/ScreenShot/";

        string str = File.ReadAllText(Path.Combine(Application.persistentDataPath, "placeData.json"));
        data = JsonConvert.DeserializeObject<List<JasonData>>(str);
        if(str == "[]")
        {
            loadText.text = "NO DATA";
        }
        else
        {
            foreach (var i in saveDataObjects.Keys)
            {
                Destroy(i);
            }
            saveDataObjects.Clear();
            for (int i=0; i<data.Count; i++)
            {
                MakeSavedataObject(data[i].saveDatas, data[i].saveDatasPosition);
            }
            ReUpdateText();
        }

    }

    private void ReUpdateText()
    {
        string temptexts = "";
        foreach (var y in data)
        {
            temptexts = temptexts + y.saveDatas.ToString() + "\n";
        }
        loadText.text = temptexts;
    }
    public void MakeSavedataObject(Vector3 _nowObjScale, Vector3 _nowObjPosition)
    {
        GameObject makeobject = Instantiate(saveobject);
        makeobject.transform.parent = saveUIObject.transform;
        Button nowbutton = makeobject.GetComponent<Button>();
        makeobject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = _nowObjScale.ToString();
        saveDataObjects.Add(makeobject, _nowObjScale);
        nowbutton.onClick.AddListener(delegate { Load(makeobject); });

        Sprite nowButtonImage = makeobject.transform.GetChild(1).gameObject.GetComponent<Image>().sprite;
        StartCoroutine(RendertoTexture(255, 255, nowButtonImage, _nowObjScale, _nowObjPosition));

        Debug.Log("버튼갯수:" + saveDataObjects.Count);

    }
    public void Save()
    {
        Vector3 nowObjScale = TouchMgr.spwanedObjs[TouchMgr.trackidNow].gameObject.transform.localScale;
        Vector3 nowObjPosition = TouchMgr.spwanedObjs[TouchMgr.trackidNow].gameObject.transform.localPosition;
        data.Add(new JasonData(nowObjScale, nowObjPosition));
        //json save
        string jsonDatas = JsonConvert.SerializeObject(data);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, "placeData.json"), jsonDatas);
        //json data update(바로 json 결과 반영)
        string loadJsonDatas = File.ReadAllText(Path.Combine(Application.persistentDataPath, "placeData.json"));
        data = JsonConvert.DeserializeObject<List<JasonData>>(loadJsonDatas);
        //버튼 생성
        MakeSavedataObject(nowObjScale, nowObjPosition);


        ReUpdateText();
    }


   
     
    IEnumerator RendertoTexture(int renderSizeX, int renderSizeY, Sprite _image, Vector3 _nowObjScale, Vector3 _nowObjPosition)
    {
        DirectoryInfo dir = new DirectoryInfo(path);
        if (!dir.Exists)
        {
            Directory.CreateDirectory(path);
        }

        string name;
        name = path + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";


        //챕처를 하려면 프래임이 끝나는 순간 해야지 완벽히 출력안되는 상황을 피할 수 있다.
        yield return new WaitForEndOfFrame();

        //cpacture Camera Ative
        capctureCamera.enabled = true;

        //카메라 위치 고정
        capctureCamera.transform.localPosition = new Vector3(_nowObjPosition.x,
                                                            _nowObjScale.x * 1.7f + _nowObjScale.z * 1.7f,
                                                            _nowObjPosition.z);


        //렌더텍스쳐 생성
        RenderTexture rt = new RenderTexture(renderSizeX, renderSizeY, 24);
        //renderTexure를 저장하기 위한 Texture2D 생성

        Texture2D screenShot = new Texture2D(renderSizeX, renderSizeY, TextureFormat.RGB24, false);

        capctureCamera.targetTexture = rt;
        capctureCamera.Render();

        //read하기 위해, 렌더링된 렌터텍스쳐를 active 설정
        RenderTexture.active = rt;

        //read
        screenShot.ReadPixels(new Rect(0, 0, renderSizeX, renderSizeY), 0, 0);
        screenShot.Apply();

        byte[] bytes = screenShot.EncodeToPNG();
        File.WriteAllBytes(name, bytes);

        // 캡쳐 완료


        _image = Sprite.Create(screenShot, new Rect(0, 0, renderSizeX, renderSizeY), new Vector2(0.5f, 0.5f));

        RenderTexture.active = null;
        capctureCamera.targetTexture = null;
        capctureCamera.enabled = false;

        Destroy(rt);
    }



    public void Clear()
    {
        data.Clear();
        foreach (var i in saveDataObjects.Keys)
        {
            Destroy(i);
        }
        saveDataObjects.Clear();
        string jsonDatas = JsonConvert.SerializeObject(data);
        File.WriteAllText(Path.Combine(Application.persistentDataPath, "placeData.json"), jsonDatas);
        loadText.text = "NO DATA";
    }

    public void Load(GameObject _button)
    {
        TouchMgr.spwanedObjs[TouchMgr.trackidNow].gameObject.transform.localScale = saveDataObjects[_button];
    }
}
