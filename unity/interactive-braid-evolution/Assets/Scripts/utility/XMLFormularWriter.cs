using UnityEngine;
using System.Collections;
using System.Xml;

public class XMLFormularWriter : MonoBehaviour {

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
            SaveInfo(); 
    }


    void SaveInfo() {
        XmlDocument doc = new XmlDocument();
        XmlElement el = (XmlElement)doc.AppendChild(doc.CreateElement("info"));
   
        el.AppendChild(doc.CreateElement("name")).InnerText = "placeholder goes here";
        el.AppendChild(doc.CreateElement("description")).InnerText = "A lot of placeholder text will be added here";
        el.AppendChild(doc.CreateElement("feedback")).InnerText = "A lot of feedback is gonna be right here";
        el.AppendChild(doc.CreateElement("email")).InnerText = "Email is gonna be right here bitches";

        string path = Application.dataPath + "/Resources/";
 
        doc.Save(path + "file.xml");
        Debug.Log("Saved xml to: " + path); 
    }
}
