using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopAlvilda : MonoBehaviour {

	public AlvildaController alvildaController;	
	
	private void OnTriggerEnter(Collider collision)
	{


		if (collision.tag == "alvilda")
		{

			Debug.Log("Alvilda stepped in");
			alvildaController.AlvildaMovementOverride();


		}
	}



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
