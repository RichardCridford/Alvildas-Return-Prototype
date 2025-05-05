using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectIdentifier : MonoBehaviour
{

	// Alvilda = -1
	// Sound emitters = 1
	// Horns = 2
	// Train Car = 3
	// Bonfires = 4
	// Door Lever = 5
	// Draggable Wall = 6 
	//

	[SerializeField] int worldObjectNumber;

	public int GetWorldObjectNumber() { return worldObjectNumber; }


}
