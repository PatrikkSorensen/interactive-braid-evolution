using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

    public float amount = 30.0f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * amount * Time.deltaTime);
    }
}
