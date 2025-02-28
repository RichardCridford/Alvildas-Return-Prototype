using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

public class HornSoundEmitterTimer : HornSoundEmitter
{
	#region Variables 

	// Unity Editor Variables 
	[SerializeField] protected TimerParams timeParams;

	// Public Properties
	public TimerObject TimerObj { get; protected set; }

	#endregion


	#region MonoBehaviour 

	// Constructor
	protected override void Awake()
	{
		base.Awake();
		TimerObj = gameObject.AddComponent<TimerObject>();
	}

	// Use this for initialization
	protected void Start()
	{
		TimerObj.AddTimerPassedCallback(TryToEmit);
		if (timeParams.enableOnStart)
		{
			TimerObj.StartTimer(timeParams);
		}

		emitterIndex = otherEmitters.Length;
	}

	// Called when the behaviour becomes disabled () or inactive.
	protected void OnDisable()
	{
		TimerObj.RemoveTimerPassedCallback(TryToEmit);
	}

	#endregion
}
