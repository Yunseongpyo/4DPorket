using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ScreenShot : MonoBehaviour
{
    public Camera camera;       //보여지는 카메라.
    private GUITexture tempTexture;

    private int resWidth;
    private int resHeight;
    string path;
    // Use this for initialization
    void Start()
    {
        resWidth = 256;
        resHeight = 256;
        path = Application.persistentDataPath + "/ScreenShot/";
        Debug.Log(path);
    }

    
    IEnumerable RendertoTexture(int renderSizeX, int renderSizeY)
    {
        //챕처를 하려면 프래임이 끝나는 순간 해야지 완벽히 출력안되는 상황을 피할 수 있다.
        yield return new WaitForEndOfFrame();

        //렌더텍스쳐 생성
        RenderTexture rt = new RenderTexture(renderSizeX, renderSizeY, 24);
        //renderTexure를 저장하기 위한 Texture2D 생성

        Texture2D screenShot = new Texture2D(renderSizeX, renderSizeY, TextureFormat.RGB24, false);

        camera.targetTexture = rt;
        camera.Render();

        //read하기 위해, 렌더링된 렌터텍스쳐를 actie 설정
        RenderTexture.active = rt;

        //read
        screenShot.ReadPixels(new Rect(0, 0, renderSizeX, renderSizeY), 0, 0);
        screenShot.Apply();
        // 캡쳐 완료

        RenderTexture.active = null;
        camera.targetTexture = null;
        Destroy(rt);
    }



    public void ClickScreenShot()
    {
        //폴더가 없을 경우 폴더 생성
        DirectoryInfo dir = new DirectoryInfo(path);
        if (!dir.Exists)
        {
            Directory.CreateDirectory(path);
        }

        //캡쳐한 이미지 저장
        string name;
        name = path + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";


        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);

        camera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        Rect rec = new Rect(0, 0, screenShot.width, screenShot.height);
        camera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        screenShot.Apply();

        byte[] bytes = screenShot.EncodeToPNG();
        File.WriteAllBytes(name, bytes);
    }
}