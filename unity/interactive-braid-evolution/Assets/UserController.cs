using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; 

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

        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(0); 

        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        Vector3 v = Camera.main.transform.forward * ver; 
        transform.Translate(v * speed);

        Vector3 v2 = Camera.main.transform.right * hor;
        transform.Translate(v2 * speed);
    }
}
