using UnityEngine;
using System.Collections;
using DG.Tweening; 

public class InspectObjectScript : MonoBehaviour {

    public GameObject objectToInspect;
    private Vector3 originalPosition;
    private float offset;
    private float duration;
    private bool selected;
    private bool clicked; 

    void Start()
    {
        offset = 3.75f;
        duration = 2.0f;
        selected = false;
        clicked = false; 
        originalPosition = objectToInspect.transform.position; 
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0))
            clicked = true;
        else if (Input.GetKeyUp(KeyCode.Mouse0))
            clicked = false;

        if (Input.GetKeyDown(KeyCode.X))
        {
            Vector3 endPosition = Camera.main.transform.position;
            endPosition.z = Camera.main.transform.position.z + offset; 
            objectToInspect.transform.DOMove(endPosition, duration);
            selected = true; 
        } else if(Input.GetKeyDown(KeyCode.Y))
        {
            selected = false; 
            objectToInspect.transform.DOMove(originalPosition, duration);
        }

        if (selected && clicked)
        {
            objectToInspect.transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * Time.deltaTime * 100.0f);
            Debug.Log("Rotating object: " + Input.GetAxis("Mouse Y") + ", " + Input.GetAxis("Mouse Y")); 
        }
            
    }
}
