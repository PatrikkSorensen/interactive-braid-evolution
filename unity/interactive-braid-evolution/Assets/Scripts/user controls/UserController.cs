using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using APP_STATUS;

public class UserController : MonoBehaviour {

    public GameObject mainMenu, sceneCenter; 
    float speed;
    float sprintSpeed; 
    bool sprint; 

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
            if (UIStatusWindow.currentStatus == STATUS.SIMULATING)
                BraidSimulationManager.SetShouldBraidsEvaluate(false);

        if (Input.GetKeyDown(KeyCode.Q))
            Camera.main.GetComponent<MouseLook>().SetTarget(sceneCenter.transform.position);

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
            transform.Translate(v2 * speed * sprintSpeed);
            transform.Translate(v * speed * sprintSpeed);
        }
        else
        {
            transform.Translate(v2 * speed);
            transform.Translate(v * speed);
        }
    }
}
