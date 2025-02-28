using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

public class RotateAround : MonoBehaviour
{
	#region Variables

	// Unity Editor Variables
	[SerializeField] protected Transform objToRotate;
	[SerializeField] protected float speed;

	// Protected Instance Variables
	protected Transform trans;
	protected PlaySoundOnClick audioController = null;

	#endregion


	#region MonoBehaviour

	// Constructor
	protected void Awake()
	{
		trans = transform;
		Assert.IsNotNull(trans);
		Assert.IsNotNull(objToRotate);
	}

	// FixedUpdate is called every fixed framerate frame
	protected void FixedUpdate()
	{  
		objToRotate.RotateAround(trans.position, Vector3.up, speed * Time.fixedDeltaTime);
	}

	#endregion
}
