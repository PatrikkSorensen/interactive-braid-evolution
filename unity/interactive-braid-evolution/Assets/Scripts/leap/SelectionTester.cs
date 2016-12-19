using UnityEngine;
using System.Collections;

public class SelectionTester : MonoBehaviour {

	public void Selected()
    {
        Debug.Log(gameObject.name + " has been selected!"); 
    }
}
