using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System;

public class LeapSelector : MonoBehaviour {

    public Camera renderCamera;

    public Material startMat;
    public Material hoverMat;
    public Material selectedMat;

    [HideInInspector]
    public bool selected;

    private Material[] mats;
    private MeshRenderer r;

    GameObject braid; 

    public void SetBraidObject ()
    {
        braid = renderCamera.transform.GetChild(0).gameObject;
        r = braid.GetComponentInChildren<MeshRenderer>();
        mats = r.materials;
    }

	//public void Clicked ()
 //   {
 //       if (!braid)
 //           SetBraidObject();

 //       for (int i = 0; i < mats.Length; i++)
 //           mats[i] = hoverMat;

 //       r.materials = mats;

 //       Debug.Log(gameObject.name + " was clicked!"); 
 //   }

    public void Clicked()
    {
        if (!braid)
            SetBraidObject();

        if (!selected)
        {
            for (int i = 0; i < mats.Length; i++)
                mats[i] = selectedMat;

            r.materials = mats;
            selected = true;

            //GameObject gb = GameObject.Find("unit_" + Regex.Match(gameObject.name, @"\d+").Value);
            //gb.GetComponent<BraidController>().SetFitness(10.0f);
        }
        else
        {
            for (int i = 0; i < mats.Length; i++)
                mats[i] = startMat;

            r.materials = mats;
            selected = false;

            //GameObject gb = GameObject.Find("unit_" + Regex.Match(gameObject.name, @"\d+").Value);
            //gb.GetComponent<BraidController>().SetFitness(0.0f);
        }
    }


}
