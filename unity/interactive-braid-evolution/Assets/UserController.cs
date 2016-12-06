using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class UserController : MonoBehaviour {

    public GameObject mainMenu; 
    float speed;
    float sprintSpeed; 
    bool sprint; 


	// Use this for initialization
	void Start () {
        mainMenu.GetComponent<MenuController>(); 
        speed = 0.25f;
        sprintSpeed = 2.0f;
	}

	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
            MenuController.ToggleMenu();

        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(3);

        if (Input.GetKeyDown(KeyCode.Space))
            FindObjectOfType<BraidSimulationManager>().SetFlagsFromButton(false); 
        AddTranslation(); 

    }

    private void AddTranslation()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            sprint = true;
        else if (Input.GetKeyUp(KeyCode.LeftShift))
            sprint = false;

        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        Vector3 v = Camera.main.transform.forward * ver;
        Vector3 v2 = Camera.main.transform.right * hor;

        if (sprint)
        {
            transform.Translate(v2 * speed * 2);
            transform.Translate(v * speed * 2);
        }
        else
        {
            transform.Translate(v2 * speed);
            transform.Translate(v * speed);
        }
    }
}
