using UnityEngine;
using System.Collections;
using DG.Tweening; 

public class BraidSelector : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.tag == "Braid")
            {
                //Debug.Log("HOVERING");
            }
        }

        if (Input.GetMouseButtonDown(0))
        {


            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag == "Braid")
                {
                    UISelectionWindow.AddBraid(hit.transform.gameObject); 
                   
                    //Debug.Log("SELECTING");

                }
            }
        }
    }

    public static void CreateHardcodedFitness()
    {
        BraidController bc = Object.FindObjectOfType<BraidController>();
        Debug.Log("Hardcoded fitness applied to: " + bc.transform.name);
        bc.SetFitness(1.0f);
    }
}
