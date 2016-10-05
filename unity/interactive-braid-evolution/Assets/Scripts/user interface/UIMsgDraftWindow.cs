using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIMsgDraftWindow : MonoBehaviour {

    public KeyCode debugKey = KeyCode.T;

    private bool toggle = true;
    private GameObject msgWindow;
    private Text text;

    private int[] pValues;
    private int NUM_ELEMENTS = 3;

    void Awake()
    {
        pValues = new int[NUM_ELEMENTS];
        pValues[0] = 0;
        pValues[1] = 0;
        pValues[2] = 0;

        msgWindow = GameObject.FindGameObjectWithTag("UIMsgDraftWindow");
        text = msgWindow.GetComponentInChildren<Text>();
        text.text += "\n";
    }

    void Update()
    {
        if (Input.GetKeyDown(debugKey))
        {
            msgWindow.SetActive(!toggle);
            toggle = !toggle;
        }
    }

    public void AddMessage(string v)
    {
        text.text += v + "\n";
    }

    public void AddParam(string key, int value)
    {
        if (key == "Height")
            pValues[0] = value;
        else if (key == "Population size")
            pValues[1] = value;
        else if (key == "novelty")
            pValues[2] = value;
        else
            Debug.LogWarning("No matching key found!");

        // beggining of key + key length + char ':' + char ' ' gives position of value
        int index = text.text.IndexOf(key + ':') + key.Length + 1 + 1; 

        // Note have to be casted from int to string and then back to char 
        char[] array = text.text.ToCharArray();
        string c = value.ToString(); 
        array[index] = c.ToCharArray()[0];
        text.text = new string(array);
    }

    public int[] GetParams()
    {
        int number, index = 0;
        string[] lines = text.text.Split('\n');
        string[] chars;
        foreach (string line in lines)
        {
            chars = line.Split(':');
            foreach (string c in chars)
            {
                bool result = int.TryParse(c, out number);
                if (result)
                    pValues[index++] = number;
            }
        }

        return pValues; 
    }
}
