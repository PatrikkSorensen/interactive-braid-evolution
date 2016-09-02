using UnityEngine;
using System.Collections;

public class ModelMessager : MonoBehaviour {

    private UDPSender sender;
    private string s = ""; 

	void Start () {
        sender = Camera.main.GetComponent<UDPSender>();
    }
	
	void Update () {
	    
	}
}
