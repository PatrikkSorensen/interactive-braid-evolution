using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class XMLFormularWriter : MonoBehaviour {

    public Text authorInfo;
    public Text braidName;
    public Text feedback;

    public string savePath;
    string path;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
            CreateXML();
    }
    private void Start()
    {
         path = Application.dataPath + "/Experiments/";
    }

    public void SaveUserInfo ()
    {
        CreateXML();
    }

    public void SaveImages(string[] filePaths)
    {
        Debug.Log("Saved all images: "); 
        Directory.CreateDirectory(savePath + "images");
        int id = 0;
        foreach (string image in filePaths) {
            string dest = savePath + "images/" + id.ToString() + ".png";
            Debug.Log(dest);
            File.Copy(image, dest);
            id++; 
        }
    }

    private void CreateXML() {

        string m_author = authorInfo.text;
        string m_braidName = braidName.text;
        string m_feedback = feedback.text; 

        XmlDocument doc = new XmlDocument();
        XmlElement el = (XmlElement)doc.AppendChild(doc.CreateElement("info"));
   
        el.AppendChild(doc.CreateElement("author")).InnerText = m_author;
        el.AppendChild(doc.CreateElement("braidname")).InnerText = m_braidName;
        el.AppendChild(doc.CreateElement("feedback")).InnerText = m_feedback; 

        savePath = path + m_author + "/";

        if (Directory.Exists(savePath))
        {
            Debug.Log("This folder already exists!");
            doc.Save(savePath + "info.xml");
        }
        else
        {
            Directory.CreateDirectory(savePath);
            doc.Save(savePath +  "info.xml");
            Debug.Log("No folder here, lets create one");
        }

        Debug.Log("Saved xml to: " + savePath); 
    }

    public void SaveModels(FileInfo[] files)
    {
        string modelSavePath = savePath + "models/"; 
        Directory.CreateDirectory(modelSavePath);
        int id = 0; 
        foreach (FileInfo f in files)
        {
            f.CopyTo(modelSavePath + id.ToString() + ".obj");
            //File.Copy(f.FullName, modelSavePath);
            id++; 
        }

        Debug.Log("Saved " + files.Length + " models to: " + modelSavePath); 
    }
}
