using UnityEngine;
using System.Collections;

public class RotateHardcoded : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    void Update()
    {
        transform.Rotate(Vector3.forward * 15 * Time.deltaTime);
    }
}
