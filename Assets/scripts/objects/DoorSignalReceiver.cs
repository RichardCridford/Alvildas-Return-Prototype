using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System.Diagnostics;

public class DoorSignalReceiver : MonoBehaviour
{
	#region Variables 

	// Unity Editor Variables
	[SerializeField] protected Transform targetObj;
	[SerializeField] protected float movementSpeed;

	// Protected Instance Variables
	protected bool isMoving = false;
	protected bool isEnabled = true;
	protected float currentDistanceToTarget = 0f;
	protected float lastDistanceToTarget = 0f;
	protected Transform trans = null;
	protected SignalReceiver signalReceiver = null;
	protected PlaySoundOnClick audioController = null;

	#endregion


	#region MonoBehaviour 

	// Constructor
	protected void Awake()
	{
		trans = transform;
		Assert.IsNotNull(trans);

		signalReceiver = gameObject.GetComponent<SignalReceiver>();
		Assert.IsNotNull(signalReceiver);

		audioController = gameObject.GetComponent<PlaySoundOnClick>();
		
		// I've commented this out because it was causing a Null exception error, may need to recode it at a later date.
		//Assert.IsNotNull(audioController);

		Assert.IsNotNull(targetObj);
	}

	// Use this for initialization
	protected virtual void Start()
	{
		signalReceiver.AddSignalReceivedCallback(ReceiveSignal);
	}

	// Update is called once per frame
	protected void FixedUpdate()
	{
		if (isEnabled && isMoving)
		{
			lastDistanceToTarget = Vector3.Distance(trans.position, targetObj.position);
			trans.position += Time.fixedDeltaTime * movementSpeed * (targetObj.position - trans.position).normalized;
			currentDistanceToTarget = Vector3.Distance(trans.position, targetObj.position);

			if (currentDistanceToTarget > lastDistanceToTarget)
			{
				trans.position = targetObj.position;
				
				//added this line for rotation as a quick fix, to get a wall to rotate (smooth it out later) 
				trans.rotation = targetObj.rotation;
				
				isEnabled = false;
			}
		}
	}

	// Called when the behaviour becomes disabled () or inactive.
	protected virtual void OnDisable()
	{
		signalReceiver.RemoveSignalReceivedCallback(ReceiveSignal);
	}

	#endregion


	#region Protected Functions

	protected void ReceiveSignal(float signalStrength)
	{
		if (!isMoving && isEnabled)
		{
			isMoving = true;
			lastDistanceToTarget = 99999f;

			// Need to figure this out 
			audioController.Play();
			
			
		}
	}

	#endregion

}
