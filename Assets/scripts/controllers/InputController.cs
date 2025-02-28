using UnityEngine;
using System;
using System.Collections;

public class InputController : MonoBehaviour
{
	// custom cursor script
	[SerializeField] mouseCursor mrArrow;
	[SerializeField] bool controllerControls;

	
	//For testing raycast cursor system
	private int NumberFromRaycast;
	public int GetNumberFromRaycast() { return NumberFromRaycast; }
	
	
	


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


	#region MonoBehaviour

	// Update is called once per frame
	protected void Update()
	{
		//if statement to only call the raycast every few frames
		if(Time.frameCount % 4 == 0)
        {
			autoRaycast();
		}

		//for testing controller options
		if (controllerControls == true)
		{
			controllerCasting();
			
		}

		else 
		{
			mouseCasting();
			

		}



	}

	private void autoRaycast()
	{
		// Check if we find a clickable object, if we do then click it
		Ray ray = Camera.main.ScreenPointToRay(mrArrow.forTheRaycast);

		Raycast(ray, out objFound, out rayHit);

		if (objFound != null)
		{
			Vector3 dir = (transform.position - objFound.transform.position).normalized;
			p = new Plane(dir, objFound.transform.position);

			if (p.Raycast(ray, out enter) == true)
			{

				
				//Stops null reference exception if the object doesn't have the Object Identifier script
				if (rayHit.collider.GetComponent<ObjectIdentifier> () == null)
				{

					NumberFromRaycast = 0;
					return;
				
				}


					

				//returns the object number hit by the raycast
				NumberFromRaycast = rayHit.collider.GetComponent<ObjectIdentifier>().GetWorldObjectNumber();
					
				//the testNumber stores the data from the raycast
				// the manager script on the cursor (where also the other sprites are stored, reaches in and grabs the testNumber)
				
				

					
				
			}
			
		}
	}


	private void mouseCasting()
	{

	#if UNITY_STANDALONE || UNITY_EDITOR
		
			if (Input.GetMouseButtonDown(0))
			{
				// Check if we find a clickable object, if we do then click it
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				Raycast(ray, out objFound, out rayHit);

				if (objFound != null)
				{
					Vector3 dir = (transform.position - objFound.transform.position).normalized;
					p = new Plane(dir, objFound.transform.position);

					if (p.Raycast(ray, out enter) == true)
					{
						objFound.Click(ray.GetPoint(enter));
						

					
					}
				}
			}
			else if (Input.GetMouseButtonUp(0))
			{
				// Check if we find a clickable object, if we do then click it
				GameObject obj = null;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				Raycast(ray, out obj, out rayHit);

				if (objFound != null)
				{
					if (p.Raycast(ray, out enter) == true)
					{
						objFound.Release(ray.GetPoint(enter));
						
				}
				}
			}
			else if (Input.GetMouseButton(0))
			{
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				GameObject obj = null;
				Raycast(ray, out obj, out rayHit);

				if (objFound != null)
				{
					Vector3 dir = (transform.position - objFound.transform.position).normalized;
					p = new Plane(dir, objFound.transform.position);
					if (p.Raycast(ray, out enter) == true)
					{
						objFound.Drag(ray.GetPoint(enter));
					}
				}
			}

			if (Input.GetKeyUp(KeyCode.D))
			{
				LevelController.Alvilda.Die();
			}
	#elif UNITY_IOS || UNITY_ANDROID
	//		if (Input.touches.Length > 0)
	//		{
	//			for (int i = 0; i < Input.touches.Length; i++)
	//			{
	//				if (Input.GetTouch(i).phase == TouchPhase.Ended)
	//				{
	//					// Check if we find a clickable object, if we do then click it
	//					Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
	//					Raycast(ray, out objFound, out rayHit);
	//
	//					if (objFound != null)
	//					{
	//						// Let everyone know who's listening that we clicked...
	//						if (clickCallbacks != null)
	//						{
	//							clickCallbacks(objFound, Input.mousePosition, rayHit.point, rayHit);	
	//						}
	//
	//						ClickableObject clickObj = objFound.GetComponent<ClickableObject>();
	//						if (clickObj != null)
	//						{
	//							objFound.GetComponent<ClickableObject>().OnClicked();
	//						}
	//					}
	//				}
	//			}
	//		}
	#endif

	}
	#endregion

	private void controllerCasting()
	{

	#if UNITY_STANDALONE || UNITY_EDITOR
			if (Input.GetButtonDown("Submit"))
			{
				// Check if we find a clickable object, if we do then click it
				Ray ray = Camera.main.ScreenPointToRay(mrArrow.forTheRaycast);

				Raycast(ray, out objFound, out rayHit);

				if (objFound != null)
				{
					Vector3 dir = (transform.position - objFound.transform.position).normalized;
					p = new Plane(dir, objFound.transform.position);

					if (p.Raycast(ray, out enter) == true)
					{
						objFound.Click(ray.GetPoint(enter));
					
					}
				}
			}
			else if (Input.GetButtonUp("Submit"))
			{
				// Check if we find a clickable object, if we do then click it
				GameObject obj = null;
				Ray ray = Camera.main.ScreenPointToRay(mrArrow.forTheRaycast);
				Raycast(ray, out obj, out rayHit);

				if (objFound != null)
				{
					if (p.Raycast(ray, out enter) == true)
					{
						objFound.Release(ray.GetPoint(enter));
					}
				}
			}
			else if (Input.GetButton("Submit"))
			{
				Ray ray = Camera.main.ScreenPointToRay(mrArrow.forTheRaycast);
				GameObject obj = null;
				Raycast(ray, out obj, out rayHit);

				if (objFound != null)
				{
					Vector3 dir = (transform.position - objFound.transform.position).normalized;
					p = new Plane(dir, objFound.transform.position);
					if (p.Raycast(ray, out enter) == true)
					{
						objFound.Drag(ray.GetPoint(enter));
						
					
					}
				}
			}

			if (Input.GetKeyUp(KeyCode.D))
			{
				LevelController.Alvilda.Die();
			}
	#elif UNITY_IOS || UNITY_ANDROID
		//		if (Input.touches.Length > 0)
		//		{
		//			for (int i = 0; i < Input.touches.Length; i++)
		//			{
		//				if (Input.GetTouch(i).phase == TouchPhase.Ended)
		//				{
		//					// Check if we find a clickable object, if we do then click it
		//					Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
		//					Raycast(ray, out objFound, out rayHit);
		//
		//					if (objFound != null)
		//					{
		//						// Let everyone know who's listening that we clicked...
		//						if (clickCallbacks != null)
		//						{
		//							clickCallbacks(objFound, Input.mousePosition, rayHit.point, rayHit);	
		//						}
		//
		//						ClickableObject clickObj = objFound.GetComponent<ClickableObject>();
		//						if (clickObj != null)
		//						{
		//							objFound.GetComponent<ClickableObject>().OnClicked();
		//						}
		//					}
		//				}
		//			}
		//		}
	#endif

	}
//#endregion




    #region Protected Functions

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

	/*// Converts a screen point to world pos
	public static bool ScreenPointToWorldPos(Vector3 screenHitPosition, out Vector3 worldPosition )
	{
		Ray ray = Camera.main.ScreenPointToRay(screenHitPosition);


		Plane rayHitPlane = new Plane(Vector3.up, Vector3.zero);	// 
		float enter = 10.0f;			// How far is the ray going to shoot?

		if (rayHitPlane.Raycast(ray, out enter) == true)
		{
			worldPosition = ray.GetPoint(enter);

			Collider[] colliders = new Collider[10];
			Physics.OverlapSphereNonAlloc(worldPosition, 15f, colliders);

			foreach(Collider c in colliders)
			{
				if (c != null)
				{
					
					Debug.Log(c.name);	
				}
			}

			return true;
		}

		worldPosition = Vector3.zero;
		return false;
	}*/

	#endregion


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
