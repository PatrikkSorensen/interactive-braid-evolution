using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

    public float amount = 15.0f;

    void Update()
    {
        transform.Rotate(Vector3.up * amount * Time.deltaTime);
    }
}
