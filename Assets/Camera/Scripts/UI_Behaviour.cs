using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Behaviour : MonoBehaviour {

	public Camera_Movement CameraMoving;
    
    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Zoom_In ()
    {
        Debug.Log("Zoom Camera In");
    }

    public void Zoom_Out()
    {
        Debug.Log("Zoom Camera Out");
    }

    public void Pivot_Left()
    {

        //Debug.Log("Pivot Camera Left");
        CameraMoving.Rotate_Left();
    }

    public void Pivot_Right()
    {
        CameraMoving.Rotate_Right();
        //Debug.Log("Pivot Camera Right");
    }
}
