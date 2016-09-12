using UnityEngine;
using System.Collections;
using UnityEngine.UI; 

public class SliderUpdater : MonoBehaviour {

    private Text txt; 
	// Use this for initialization
	void Start () {
        txt = GetComponentInChildren<Text>(); 
	}
	
	// Update is called once per frame
	void Update () {
        txt.text = "hello world"; 
	}
}
