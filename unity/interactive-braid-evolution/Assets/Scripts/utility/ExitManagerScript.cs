using UnityEngine;
using System.Collections;

public class ExitManagerScript : MonoBehaviour {

    private Animator anim;

    private void Start()
    {
        anim = GameObject.Find("FormCanvas").GetComponent<Animator>(); 
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            anim.SetTrigger("advance"); 
    }
}
