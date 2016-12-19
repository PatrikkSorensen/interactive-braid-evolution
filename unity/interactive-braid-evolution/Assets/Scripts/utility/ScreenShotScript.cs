using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System;

public class ScreenShotScript : MonoBehaviour {

    public GameObject storyboardContainer;
    public GameObject backgroundUI;

    public static string folderPath;
    public static string screenShotPrefix; 
    private int currentId, imgWidth, imgHeight;
    private int uiRawImageWidth, borderOffset; 
    private bool loadedTexture; 

    void Start()
    {
        backgroundUI.SetActive(false); 
        imgWidth = imgHeight = 512;
        borderOffset = 12; 
        uiRawImageWidth = 100; 
        folderPath = Application.dataPath + "/Geometry/StoryboardImages/"; 
        currentId = 0;
    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Z))
        {
            SetBackgroundSize();
            CreateStoryboardUI();
        }

    }

    public IEnumerator CreateRenderTexture(GameObject gb, int generation)
    {
        string directoryPath = folderPath + generation.ToString();
        string fileDest = directoryPath + "/" + screenShotPrefix + currentId.ToString() + ".png";

        if (!Directory.Exists(directoryPath))
            Directory.CreateDirectory(folderPath + generation.ToString()); 

        yield return new WaitForEndOfFrame();

        Texture2D tex = new Texture2D(imgWidth, imgHeight, TextureFormat.RGB24, false);
        Rect r = new Rect(Screen.width * 0.5f - 200.0f, Screen.height * 0.50f - 200.0f, Screen.width * 0.50f + 100, Screen.height * 0.50f + 100);

        tex.ReadPixels(r, 0, 0);
        tex.Apply();

        var bytes = tex.EncodeToPNG();
        Destroy(tex); 
        File.WriteAllBytes(fileDest, bytes);

        currentId++;
        Destroy(gb);
    }

    public void CreateStoryboardUI()
    {

        Texture2D[] images;
        int index = 0; 
        foreach (string d in Directory.GetDirectories(folderPath))
        {
            images = LoadAllImagesFromFolder(d, index++);
            for (int i = 0; i < images.Length; i++)
                CreateRawImageGameobject(images[i]);
        }

        SetUIImagesPosition();
        XMLFormularWriter.SaveModels();
        XMLFormularWriter.SaveImages(); 
         
        GameObject.Find("- ui storyboard").GetComponent<Animator>().SetTrigger("advance"); 
    }

    void SetUIImagesPosition()
    {
        int index = 0;
        int Xoffset = (int) (backgroundUI.GetComponent<RectTransform>().rect.width / 2) - uiRawImageWidth / 2;
        Xoffset -= borderOffset; 

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
        backgroundUI.SetActive(true); 
        backgroundUI.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 0.75f, Screen.height * 0.2f);
    }
    void CreateRawImageGameobject(Texture2D image)
    {
        GameObject gb = new GameObject();
        gb.AddComponent<RawImage>();
        gb.GetComponent<RawImage>().texture = image;
        gb.transform.SetParent(storyboardContainer.transform); 
    }

    Texture2D[] LoadAllImagesFromFolder(string path, int generation)
    {
        string[] files = Directory.GetFiles(path, "*.png");
        //FindObjectOfType<XMLFormularWriter>().SaveModelsAndImages(files, generation);

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
