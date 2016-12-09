using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera-Control/Mouse Look (Debugged Edition)")]
public class MouseLook : MonoBehaviour {
	
	public float sensitivityX = 15F;
	public float sensitivityY = 15F;
	
	public float minimumX = -360F;
	public float maximumX = 360F;
	
	public float minimumY = -60F;
	public float maximumY = 60F;
	
	
	float rotationY = 0F;
	float rotationX = 0F;

    void Update ()
	{
		rotationX += Input.GetAxis("Mouse X") * sensitivityX;
		rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
			    
		transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
	}

    public void SetTarget(Vector3 target)
    {
        target = Vector3.zero;
        rotationX = 0.0f;
        rotationY = 0.0f; 

        transform.localEulerAngles = target; 
    }
	

}
