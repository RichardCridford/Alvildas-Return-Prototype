using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

public class JugObject : SoundEmitterTimer
{
	#region Variables 

	// Protected Instance Variables
	protected SignalReceiver signalReceiver = null;

	#endregion


	#region MonoBehaviour

	// Constructor
	protected override void Awake()
	{
		signalReceiver = gameObject.GetComponent<SignalReceiver>();
		Assert.IsNotNull(signalReceiver);

		base.Awake();
	}

	// Use this for initialization
	protected override void Start()
	{
		signalReceiver.AddSignalReceivedCallback(ReceiveSignal);
		timeParams.enableOnStart = false;

		base.Start();
	}

	// Called when the behaviour becomes disabled () or inactive.
	protected override void OnDisable()
	{
		signalReceiver.RemoveSignalReceivedCallback(ReceiveSignal);

		base.OnDisable();
	}

	#endregion


	#region Protected Functions

	protected void ReceiveSignal(float signalStrength)
	{
		if (signalStrength > 0.5f)
		{
			TryToEmit();
			TimerObj.StartTimer(timeParams);
		}
		else
		{
			TimerObj.StopTimer();
		}
	}

	#endregion
}

