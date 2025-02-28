using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mouseCursor : MonoBehaviour 
{
	InputController input;
	
	[SerializeField] private int objectNumber;
	private string worldObjectName;


	//variables for the new mouse cursor
	private Image rend;
	[SerializeField] private Sprite blueArrow;
	[SerializeField] private Sprite greenArrow;
	[SerializeField] private Sprite redArrow;
	[SerializeField] private Sprite greenFlame;
	[SerializeField] private Sprite blueHand;

	
	[SerializeField] float moveSpeed = 10f;
	[SerializeField] bool removepointer = true;
	
	// variable that the input controller can call on for the custom cursor position
	public Vector3 forTheRaycast;

	// Use this for initialization
	void Start () 
	{
		rend = GetComponent<Image>();
		input = FindObjectOfType<InputController>();
		
		
		if (removepointer)
		{

			Cursor.visible = false;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		Move();

		ManageArrowSprites();
	}

	private void Move()
	{
		var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
		var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

		var newXPos = transform.position.x + deltaX;
		var newYPos = transform.position.y + deltaY;
		forTheRaycast = transform.position = new Vector3(newXPos, newYPos, 0);

	}


	private void ManageArrowSprites()
	{
		objectNumber = input.GetNumberFromRaycast();


		if (objectNumber == 0)
		{
			rend.sprite = redArrow;
		
		
		}
		
		if (objectNumber == 1)
		{

			worldObjectName = "Sound Emitter";
			Debug.Log(worldObjectName);
			rend.sprite = greenArrow;

		}

		if (objectNumber == 2)
		{
			worldObjectName = "Horn";
			Debug.Log(worldObjectName);
			rend.sprite = greenArrow;


		}


		if (objectNumber == 3)
		{

			worldObjectName = "Train Car";
			Debug.Log(worldObjectName);
			rend.sprite = blueHand;

		}

		
		if (objectNumber == 4)
		{

			worldObjectName = "Bonfire";
			Debug.Log(worldObjectName);
			rend.sprite = greenFlame;

		}


		if (objectNumber == 5)
		{
			worldObjectName = "Door Lever";
			Debug.Log(worldObjectName);
			rend.sprite = greenArrow;
		
		
		}

		else if (objectNumber == 6)
		{
			worldObjectName = "Draggable Wall";
			Debug.Log(worldObjectName);
			rend.sprite = blueArrow;


		}

	}


}



