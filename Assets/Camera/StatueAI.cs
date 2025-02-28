using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueAI : MonoBehaviour {

	[SerializeField] private float rotationSpeed;
	[SerializeField] private float distance;

	[SerializeField] private LineRenderer lineOfSight;
	[SerializeField] private Gradient redColor;
	[SerializeField] private Gradient greenColor;
	[SerializeField] private Gradient purpleColor;

	[SerializeField] private bool canStatueTriggerObjects;

	// testing raycast hits sound objects
	public int NumberFromRaycast;


	//Needed for making calls to Sound Objects
	#region Variables 

	// Delegates
	public delegate void Click(GameObject objClicked, Vector3 mousePos, Vector3 worldPos, RaycastHit rayHit);

	// Callbacks
	protected Click clickCallbacks = null;

	//
	protected GameObject objFound = null;
	protected RaycastHit rayHit = new RaycastHit();

	Plane p = new Plane(Vector3.up, Vector3.zero);
	float enter = 50.0f;

	#endregion


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		//EnemyRaycast();
		AlternativeRaycast();

	}


	private void EnemyRaycast()
	{
		transform.Rotate(Vector3.down * rotationSpeed * Time.deltaTime);

		var ray = new Ray(this.transform.position, this.transform.forward);
		RaycastHit hit;
		Physics.Raycast(ray);

		if (Physics.Raycast(ray, out hit, distance))
		{

			lineOfSight.SetPosition(1, hit.point);
			lineOfSight.colorGradient = redColor;

			NumberFromRaycast = hit.collider.GetComponent<ObjectIdentifier>().GetWorldObjectNumber();


			if (hit.collider.CompareTag("alvilda"))
			{
				LevelController.Alvilda.Die();

			}
		}
		else
		{

			lineOfSight.SetPosition(1, transform.position + transform.forward * distance);
			lineOfSight.colorGradient = greenColor;

		}

		lineOfSight.SetPosition(0, transform.position);


	}

	private void AlternativeRaycast()
	{
		transform.Rotate(Vector3.down * rotationSpeed * Time.deltaTime);



		var ray = new Ray(this.transform.position, this.transform.forward);
		Raycast(ray, out objFound, out rayHit);

	

		if (objFound != null)
		{
			//To render the line of the Raycast in the game view
			lineOfSight.SetPosition(1, rayHit.point);
			lineOfSight.colorGradient = redColor;

			//Stops null reference exception if the object doesn't have the Object Identifier script
			if (rayHit.collider.GetComponent<ObjectIdentifier>() == null)
			{

				NumberFromRaycast = 0;
				return;

			}

		

			//Polls the object for its identifying number
			NumberFromRaycast = rayHit.collider.GetComponent<ObjectIdentifier>().GetWorldObjectNumber();



			Vector3 dir = (transform.position - objFound.transform.position).normalized;
			p = new Plane(dir, objFound.transform.position);


			if (canStatueTriggerObjects)
			{
				//Using the identifying number can set which objects are affected by the Ray 
				if (p.Raycast(ray, out enter) == true && NumberFromRaycast == 1)
				{
					objFound.Click(ray.GetPoint(enter));



				}

				if (p.Raycast(ray, out enter) == true && NumberFromRaycast == 2)
				{
					objFound.Click(ray.GetPoint(enter));


				}

			}
			
			
			if (p.Raycast(ray, out enter) == true && NumberFromRaycast == -1)
			{
				
				LevelController.Alvilda.Die();

				StartCoroutine("LaserHitsAlvilda");
			
			}


		}

		else
		{

			lineOfSight.SetPosition(1, transform.position + transform.forward * distance);
			lineOfSight.colorGradient = greenColor;

		}

		lineOfSight.SetPosition(0, transform.position);
	}

	IEnumerator LaserHitsAlvilda ()
	{
		//may need to change rotationSpeed
		lineOfSight.colorGradient = purpleColor;
		rotationSpeed = 0;

		yield return new WaitForSeconds(3);

		rotationSpeed = 25;
	}


	protected void Raycast(Ray ray, out GameObject objFound, out RaycastHit rayHit)
	{
		float enter = 1000;

		//Debug.DrawRay(ray.origin, ray.direction, Color.red, 5f);
		if (Physics.Raycast(ray, out rayHit, enter))
		{
			objFound = rayHit.collider.gameObject;
			return;
		}

		objFound = null;

	}


	#region Public Functions

	public void AddClickCallback(Click newCallback)
	{
		clickCallbacks += newCallback;
	}

	public void RemoveClickCallback(Click newCallback)
	{
		clickCallbacks -= newCallback;
	}

	#endregion
}
