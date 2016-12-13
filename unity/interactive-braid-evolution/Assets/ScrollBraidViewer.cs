using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class ScrollBraidViewer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.X))
            Scroll(); 
	}

    void Scroll()
    {
        float h = Input.GetAxis("Horizontal");
        Debug.Log(h); 
        Scrollbar scrollbar = FindObjectOfType<Scrollbar>();
        scrollbar.value += h * Time.deltaTime; 
    }
}
