using UnityEngine;
using System.Collections;

public class PositionMover : MonoBehaviour {

    public GameObject[] positions;
    public KeyCode moveLeft;
    public KeyCode moveRight;

    private MouseLook ml; 
    private int startPositionIndex;
    private int[,] camValues; 

	void Start () {
        Debug.Assert(positions.Length != 0, "User positions array is empty");
        ml = Camera.main.GetComponent<MouseLook>();

        camValues = new int[4, 2] { 
            { -30, 30 },   // South  
            { 60, 120 },   // West
            { 0, 0 },      // North 
            { -120, -60 }  // East
        };
    }
	
    //TODO: Make it possible to move around the points instead of using hardcoded keykodes
	void Update () {
        if (Input.GetKey(moveLeft))
            MoveCharacter(1);
        else if (Input.GetKey(moveRight))
            MoveCharacter(3);
    }

    private void MoveCharacter(int posIndex) {
        Camera.main.transform.position = positions[posIndex].transform.position;

        ml.minimumX = camValues[posIndex, 0];
        ml.maximumX = camValues[posIndex, 1]; 
    }
}
