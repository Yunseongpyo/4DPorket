using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.XR.ARSubsystems;
using Newtonsoft.Json;
using TMPro;
using UnityEngine.UI;

public class TestJasonData
{
    public Vector3 saveDatas;

    public TestJasonData(Vector3 saveDatas)
    {
        this.saveDatas = saveDatas;
    }
}


public class JsonTest : MonoBehaviour
{
    public Camera testCamera;
    public Image images;
    public RawImage mainimages;

    public GameObject tempObj;
    public GameObject loadObj;
    List<TestJasonData> data = new List<TestJasonData>();
    public TextMeshProUGUI loadText;
    private void Start()
    {
        string str = File.ReadAllText(Path.Combine(Application.dataPath, "placeData.json"));
        data = JsonConvert.DeserializeObject<List<TestJasonData>>(str);
        if (str == "[]")
        {
            loadText.text = "NO DATA";
        }
        else
        {
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
    public void Clear()
    {
        data.Clear();
        string jsonDatas = JsonConvert.SerializeObject(data);
        File.WriteAllText(Path.Combine(Application.dataPath, "placeData.json"), jsonDatas);
        loadText.text = "NO DATA";
    }
    public void Save()
    {
        //Vector3 nowObjScale = TouchMgr.spwanedObjs[TouchMgr.trackidNow].gameObject.transform.localScale;
        Vector3 nowObjScale = tempObj.transform.localScale;
        //Vector3 zeros = Vector3.zero;
        //testData.saveDatas.Add(nowObjScale);
        data.Add(new TestJasonData(nowObjScale));
        //data.Add(new TestJasonData(zeros));
        string jsonDatas = JsonConvert.SerializeObject(data);
        File.WriteAllText(Path.Combine(Application.dataPath, "placeData.json"), jsonDatas);
        string str = File.ReadAllText(Path.Combine(Application.dataPath, "placeData.json"));
        data = JsonConvert.DeserializeObject<List<TestJasonData>>(str);
        ReUpdateText();
    }
    //public void Load()
    //{
    //    string str = File.ReadAllText(Path.Combine(Application.dataPath, "placeData.json"));
    //    data = JsonConvert.DeserializeObject<List<TestJasonData>>(str);
    //    foreach(var y in data)
    //    {
    //        Debug.Log(y.saveDatas);
    //    }
    //}

    public void CaptureStart()
    {

        StartCoroutine(RendertoTexture(255, 255, mainimages.mainTexture));
    }

    IEnumerator RendertoTexture(int renderSizeX, int renderSizeY, Texture _image)
    {
       


        //챕처를 하려면 프래임이 끝나는 순간 해야지 완벽히 출력안되는 상황을 피할 수 있다.
        yield return new WaitForEndOfFrame();

        //cpacture Camera Ative
        //testCamera.enabled = true;

        //카메라 위치 고정
        //testCamera.transform.localPosition = new Vector3(_nowObjPosition.x,
        //                                                    _nowObjScale.x * 1.7f + _nowObjScale.z * 1.7f,
        //                                                    _nowObjPosition.z);


        //렌더텍스쳐 생성
        RenderTexture rt = new RenderTexture(renderSizeX, renderSizeY, 24);
        //renderTexure를 저장하기 위한 Texture2D 생성

        Texture2D screenShot = new Texture2D(renderSizeX, renderSizeY, TextureFormat.RGB24, false);

        testCamera.targetTexture = rt;
        testCamera.Render();

        //read하기 위해, 렌더링된 렌터텍스쳐를 active 설정
        RenderTexture.active = rt;

        //read
        screenShot.ReadPixels(new Rect(0, 0, renderSizeX, renderSizeY), 0, 0);
        screenShot.Apply();

        //byte[] bytes = screenShot.EncodeToPNG();
        //File.WriteAllBytes(name, bytes);

        // 캡쳐 완료
        Debug.Log("나왔니?");

        _image = screenShot;

        RenderTexture.active = null;
        testCamera.targetTexture = null;
        //testCamera.enabled = false;

        Destroy(rt);
    }



}
