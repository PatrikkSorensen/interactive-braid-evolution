using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class ScreenShotScript : MonoBehaviour {

    public GameObject storyboardContainer;
    public GameObject backgroundUI; 
    private string folderPath;
    private int currentId, imgWidth, imgHeight;
    private int uiRawImageWidth, borderOffset; 
    private bool loadedTexture; 


    void Start()
    {
        imgWidth = imgHeight = 256;
        borderOffset = 12; 
        uiRawImageWidth = 100; 
        folderPath = Application.dataPath + "/Resources/StoryboardImages/screenshot_"; 
        currentId = 0;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
            TakeScreenshot();

        if (Input.GetKeyDown(KeyCode.Y))
            StartCoroutine(CreateRenderTexture());

        if (Input.GetKeyDown(KeyCode.Z))
        {
            SetBackgroundSize(); 
            LoadStoryboardImages();
        }
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

        Texture2D tex = new Texture2D(imgWidth, imgHeight, TextureFormat.RGB24, false);
        Rect r = new Rect(Screen.width * 0.5f - 100.0f, Screen.height * 0.50f - 100.0f, Screen.width * 0.50f + 100, Screen.height * 0.50f + 100);

        tex.ReadPixels(r, 0, 0);
        tex.Apply();

        var bytes = tex.EncodeToPNG();
        Destroy(tex);

        File.WriteAllBytes(folderPath + currentId.ToString() + ".png", bytes);

        currentId++;
        Debug.Log("Created image with texture rendering!");
    }

    void LoadStoryboardImages()
    {
        int id = currentId - 1;
        Texture2D[] images = LoadAllImagesFromFolder(folderPath); 

        for(int i = 0; i < images.Length; i++)
            CreateRawImageGameobject(images[i]);

        SetUIImagesPosition(); 
        Debug.Log("Changed storyboard texture from disk");
    }

    void SetUIImagesPosition()
    {
        int index = 0;
        int Xoffset = (int) (backgroundUI.GetComponent<RectTransform>().rect.width / 2) - uiRawImageWidth / 2;
        Xoffset -= borderOffset; 

        Debug.Log(backgroundUI.GetComponent<RectTransform>().rect.width); 

        foreach(Transform t in storyboardContainer.transform)
        {
            int x = (index * 100) - Xoffset;
            t.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, 0);
            index++;
        }

        storyboardContainer.transform.position = backgroundUI.transform.position; 
    }

    void SetBackgroundSize()
    {
        float height = 200.0f; 
        backgroundUI.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 0.75f, Screen.height * 0.25f);
    }
    void CreateRawImageGameobject(Texture2D image)
    {
        GameObject gb = new GameObject();
        gb.AddComponent<RawImage>();
        gb.GetComponent<RawImage>().texture = image;
        gb.transform.SetParent(storyboardContainer.transform); 
    }

    Texture2D[] LoadAllImagesFromFolder(string path)
    {
        string[] files = System.IO.Directory.GetFiles(Application.dataPath + "/Resources/StoryboardImages/", "*.png");
        Texture2D[] images = new Texture2D[files.Length];
        int index = 0; 

        foreach (string s in files)
        {
            byte[] fileData = File.ReadAllBytes(s);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
            images[index++] = tex; 
        }

        return images; 
    }
}
