using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

public class DraggableWall : DraggableObject
{
	#region Variables

	// Unity Editor Variables
	[SerializeField] protected Transform startPosObj = null;
	[SerializeField] protected Transform EndPosObject = null;

	// Protected Instance Variables
	protected float startAndEndPointsDistance = 0f;
	protected Vector3 startPos = Vector3.zero;
	protected Vector3 endPos = Vector3.zero;
	protected Vector3 endToStartDir = Vector3.zero; 
	#endregion


	#region Public Functions 

	public override void OnClicked(Vector3 worldPos)
	{
		this.Log("Clicked \"" + name + "\"", DebugLogLevel.MediumDetails);

		worldPos.y = GameConst.HEIGHT;
		startMouseWorldPos = worldPos;
		startTransPos = trans.position;
		startPos = startPosObj.position;
		endPos = EndPosObject.position;
		startAndEndPointsDistance = Vector3.Distance(startPosObj.position, EndPosObject.position);
		endToStartDir = (startPos - endPos).normalized;

		if (SoundsEnabled)
		{
			audioController.Play();
		}

		if (clickedCallbacks != null)
		{
			clickedCallbacks();
		}
	}

	public override void OnDragged(Vector3 worldPos)
	{
		this.Log("OnDragged \"" + name + "\" " + worldPos, DebugLogLevel.VeryDetailed);

		// Calculate the Mouse movement in world positions
		worldPos.y = GameConst.HEIGHT;
		mouseWorldMovement = (worldPos - startMouseWorldPos);
		mouseWorldMovement.y = 0f;

		// Project the object to the line between the two objects that set the path...
		Vector3 V1 = ((startTransPos + mouseWorldMovement) - startPos);
		Vector3 V2 = Vector3.Project(V1, endToStartDir);
		Vector3 newPos = startPos + V2;
		float newPosDist = Vector3.Distance(startPos, newPos);

		// Make sure we don't go farther than the two points...
		if (Vector3.Dot(endToStartDir, (startPos - newPos).normalized) <= 0)
		{
			newPos = startPos;
		}
		else if (newPosDist > startAndEndPointsDistance)
		{
			newPos = endPos;
		}

		trans.position = newPos;

		if (SoundsEnabled)
		{
			audioController.KeepLooping();
		}

		if (draggedCallbacks != null)
		{
			draggedCallbacks();
		}
	}

	// Called by the editor to draw gizmos that are also pickable
	protected void OnDrawGizmos()
	{
		if (startPosObj != null)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(startPosObj.position, 0.25f);
		}
		if (EndPosObject != null)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawSphere(EndPosObject.position, 0.25f);
		}
	}

	#endregion
}
