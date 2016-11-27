using UnityEngine;
using System.Collections;
using System.IO;

public class ScreenShotScript : MonoBehaviour {

    public GameObject storyboard;

    string path;
    int currentId, width, height;
    bool loadedTexture; 


    void Start()
    {
        width = height = 256;
        path = Application.dataPath + "/screenshot_"; 
        currentId = 0;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
            TakeScreenshot();

        if (Input.GetKeyDown(KeyCode.Y))
            StartCoroutine(CreateRenderTexture()); 
    }

    void TakeScreenshot()
    {
        string path = Application.dataPath + "/Screenshot.png";
        Debug.Log("Took screenshot and saved it to: " + path);
        Application.CaptureScreenshot(path);
    }



    IEnumerator CreateRenderTexture()
    {
        yield return new WaitForEndOfFrame();

        Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
        Rect r = new Rect(Screen.width * 0.5f - 100.0f, Screen.height * 0.50f - 100.0f, Screen.width * 0.50f + 100, Screen.height * 0.50f + 100);

        tex.ReadPixels(r, 0, 0);
        tex.Apply();

        var bytes = tex.EncodeToPNG();
        Destroy(tex);

        File.WriteAllBytes(path + currentId.ToString() + ".png", bytes);
        currentId++;

        Debug.Log("Created image with texture rendering!");
        ChangeStoryboardTexture(); 
    }

    void ChangeStoryboardTexture()
    {
        int id = currentId - 1;
        byte[] fileData = File.ReadAllBytes(path + id.ToString() + ".png");
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(fileData);

        Renderer r = storyboard.GetComponent<Renderer>();
        r.material.mainTexture = tex;
        Debug.Log("Changed storyboard texture from disk");
    }

    IEnumerator LoadStoryboardFromWeb()
    {
        string url = "http://images.earthcam.com/ec_metros/ourcams/fridays.jpg";
        Renderer r = storyboard.GetComponent<Renderer>();
        r.material.mainTexture = new Texture2D(4, 4, TextureFormat.DXT1, false);

        WWW www = new WWW(url);
        yield return www;
        www.LoadImageIntoTexture(r.material.mainTexture as Texture2D);
        Debug.Log("Changed storyboard texture from web");
    }
}
