using UnityEngine;
using System.Collections;

public class CheckPointObject : MonoBehaviour
{
	#region MonoBehaviour 

	// OnTriggerEnter is called when the Collider other enters the trigger
	protected void OnTriggerEnter(Collider other)
	{
		if (other.IsAlvilda())
		{
			LevelController.Alvilda.CheckPointController.SaveCheckPoint(transform.position);
			gameObject.SetActive(false);
		}
	}

	// Called by the editor to draw gizmos that are also pickable
	protected void OnDrawGizmos()
	{
		Gizmos.DrawIcon(transform.position, "checkpoint_object.png", true);
	}

	#endregion
}
