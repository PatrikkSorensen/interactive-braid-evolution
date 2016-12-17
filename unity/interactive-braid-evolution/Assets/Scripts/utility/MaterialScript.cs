using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System;

public class MaterialScript : MonoBehaviour {

    public Material startMat;
    public Material hoverMat;
    public Material selectedMat;

    [HideInInspector]
    public bool selected;

    private Material[] mats; 
    private MeshRenderer r;


    void Start()
    {
        r = GetComponent<MeshRenderer>();
        mats = r.materials;

        startMat = r.materials[0];
        hoverMat = Resources.Load("HoverMaterial") as Material;
        selectedMat = Resources.Load("SelectedMaterial") as Material;
    }

    void OnMouseOver()
    {
        if (!selected)
        {

            for (int i = 0; i < mats.Length; i++)
                mats[i] = hoverMat;

            r.materials = mats;
        }
    }

    void OnMouseExit()
    {
        if (!selected)
        {
            for (int i = 0; i < mats.Length; i++)
                mats[i] = startMat;

            r.materials = mats;
        }
    }

    void OnMouseDown()
    {
        if (!selected)
        {
            for (int i = 0; i < mats.Length; i++)
                mats[i] = selectedMat;

            r.materials = mats;
            selected = true;

            GameObject gb = GameObject.Find("unit_" + Regex.Match(gameObject.name, @"\d+").Value);
            gb.GetComponent<BraidController>().SetFitness(10.0f);
        } else
        {
            for (int i = 0; i < mats.Length; i++)
                mats[i] = startMat;

            r.materials = mats;
            selected = false;

            GameObject gb = GameObject.Find("unit_" + Regex.Match(gameObject.name, @"\d+").Value);
            gb.GetComponent<BraidController>().SetFitness(0.0f);
        }
    }
}
