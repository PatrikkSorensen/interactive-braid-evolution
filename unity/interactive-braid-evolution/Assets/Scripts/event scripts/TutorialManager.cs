using UnityEngine;
using System.Collections;

public class TutorialManager : MonoBehaviour {

    Animator tutorialAnim; 
	// Use this for initialization
	void Start () {
        tutorialAnim = GameObject.Find("TutorialCanvas").GetComponent<Animator>(); 

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            tutorialAnim.SetTrigger("advance"); 

    }
}
