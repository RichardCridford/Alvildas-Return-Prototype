using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Movement : MonoBehaviour {

	public Camera Viewer;
    
    // Use this for initialization
	void Start ()
    {
        //Viewer.transform.rotation = Quaternion.Euler(36, 45, 1.65f);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Rotate_Right()
    {
        
        Viewer.transform.Rotate(0, 1, 0);
        
    }

    public void Rotate_Left()
    {
        
        Viewer.transform.Rotate(0, -1, 0);

    }
}
