using UnityEngine;
using System.Collections;

public class MaterialScript : MonoBehaviour {

    public Material startMat;
    public Material hoverMat;
    public Material selectedMat;

    private Material[] mats; 
    private MeshRenderer r;
    private bool selected; 

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
        if (UIStatusWindow.currentStatus != UIStatusWindow.STATUS.SIMULATING)
            return; 

        for (int i = 0; i < mats.Length; i++)
            mats[i] = selectedMat;

        r.materials = mats;
        selected = true;

        Debug.Log("Clicked!"); 
        UISelectionWindow.AddBraid(gameObject);
    }
}
