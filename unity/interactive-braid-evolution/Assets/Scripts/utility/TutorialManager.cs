using UnityEngine;
using System.Collections;

public class TutorialManager : MonoBehaviour
{

    private Animator anim;

    private void Start()
    {
        anim = GameObject.Find("TutorialCanvas").GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            anim.SetTrigger("advance");
    }
}