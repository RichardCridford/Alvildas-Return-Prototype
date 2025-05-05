using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseCursorStory : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	

	void Update () 
	{
		Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		transform.position = cursorPos;

		Debug.Log(Input.mousePosition);

	}
}
