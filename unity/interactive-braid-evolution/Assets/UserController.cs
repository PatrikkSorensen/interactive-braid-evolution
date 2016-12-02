using UnityEngine;
using System.Collections;

public class UserController : MonoBehaviour {

    public GameObject mainMenu; 
    float speed; 

	// Use this for initialization
	void Start () {
        mainMenu.GetComponent<MenuController>(); 
        speed = 0.25f; 
	}

	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
            MenuController.ToggleMenu(); 

        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        Vector3 v = Camera.main.transform.forward * ver; 
        transform.Translate(v * speed);

        Vector3 v2 = Camera.main.transform.right * hor;
        Debug.Log(v2); 
        transform.Translate(v2 * speed);
    }
}
