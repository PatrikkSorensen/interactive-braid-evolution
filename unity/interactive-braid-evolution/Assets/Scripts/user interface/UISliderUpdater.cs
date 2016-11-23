using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UISliderUpdater : MonoBehaviour {

    public string key; 

    private Text txt;
    private static Slider sl;

    void Start () {

        sl = GetComponentInChildren<Slider>();
        sl.onValueChanged.AddListener(delegate { OnValueChange(); });

        txt = GetComponentInChildren<Text>();
        txt.text = key + " : " + sl.value;
    }

    //TODO: Make this change the actual message values
    public void OnValueChange()
    {
        txt.text = key + " : " + sl.value;
       
    }

    internal static int GetValue()
    {
        return (int) 10.0f; 
    }
}
