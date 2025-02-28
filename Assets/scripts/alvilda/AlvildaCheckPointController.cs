using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;

public sealed class AlvildaCheckPointController : MonoBehaviour
{
	#region Variables

	// private Instance Variables
	private List<Vector3> checkPointPositions = new List<Vector3>();

	// Public Properties
	public Vector3 LastSavedPosition { get { return checkPointPositions[checkPointPositions.Count - 1]; } }

	#endregion


	#region MonoBehaviour 

	// Use this for initialization
	private void Start()
	{
		SaveCheckPoint(transform.position);
	}

	// Called by the editor to draw gizmos that are also pickable
	private void OnDrawGizmos()
	{
		if (Application.isPlaying)
		{
			Gizmos.DrawIcon(LastSavedPosition, "checkpoint_alvilda.png", true);
		}
		else
		{
			Gizmos.DrawIcon(transform.position, "checkpoint_alvilda.png", true);	
		}

	}

	#endregion


	#region Public Functions 

	// Called by the CheckPointObject class when Alvilda reaches a checkpoint
	public void SaveCheckPoint(Vector3 checkPointPos)
	{
		checkPointPos.y = GameConst.HEIGHT;
		checkPointPositions.Add(checkPointPos);
	}


	#endregion
}
