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

    public static string savePath;
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

    public static void SaveModelsAndImages ()
    {
        SaveImages(); 
        SaveModels();
    }

    private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
    {
        // Get the subdirectories for the specified directory.
        DirectoryInfo dir = new DirectoryInfo(sourceDirName);

        if (!dir.Exists)
        {
            throw new DirectoryNotFoundException(
                "Source directory does not exist or could not be found: "
                + sourceDirName);
        }

        DirectoryInfo[] dirs = dir.GetDirectories();
        // If the destination directory doesn't exist, create it.
        if (!Directory.Exists(destDirName))
        {
            Directory.CreateDirectory(destDirName);
        }

        // Get the files in the directory and copy them to the new location.
        FileInfo[] files = dir.GetFiles();
        foreach (FileInfo file in files)
        {
            string temppath = Path.Combine(destDirName, file.Name);
            file.CopyTo(temppath, false);
        }

        // If copying subdirectories, copy them and their contents to new location.
        if (copySubDirs)
        {
            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = Path.Combine(destDirName, subdir.Name);
                DirectoryCopy(subdir.FullName, temppath, copySubDirs);
            }
        }
    }

    private void CreateXML() {

        string m_author, m_braidName, m_feedback;

        if (authorInfo)
            m_author = authorInfo.text;
        else
            m_author = "0";

        if (braidName)
            m_braidName = braidName.text;
        else
            m_braidName = "0";

        if (feedback)
            m_feedback = feedback.text;
        else
            m_feedback = "0"; 

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

    public static void SaveModels()
    {
        string source = "C:/Users/pves/Desktop/braid-evolution/unity/interactive-braid-evolution/Assets/Geometry/TempModels/";
        DirectoryCopy(source, savePath + "models/", true);
    }

    public static void SaveImages()
    {
        string source = "C:/Users/pves/Desktop/braid-evolution/unity/interactive-braid-evolution/Assets/Geometry/StoryboardImages/";
        DirectoryCopy(source, savePath + "images/", true);
    }
}
