using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

public class SoundEmitterTimer : SoundEmitter
{
	#region Variables 

	// Unity Editor Variables
	[SerializeField] protected TimerParams timeParams;

	// Protected Instance Variables
	public TimerObject TimerObj { get; set; }

	#endregion


	#region MonoBehaviour 

	// Constructor
	protected override void Awake()
	{
		TimerObj = gameObject.AddComponent<TimerObject>();
		Assert.IsNotNull(TimerObj);
		base.Awake();
	}

	// Use this for initialization
	protected override void Start()
	{
		TimerObj.AddTimerPassedCallback(TryToEmit);
		if (timeParams.enableOnStart)
		{
			TimerObj.StartTimer(timeParams);
		}
		base.Awake();
	}

	// Called when the behaviour becomes disabled () or inactive.
	protected override void OnDisable()
	{
		TimerObj.RemoveTimerPassedCallback(TryToEmit);
		base.OnDisable();
	}

	#endregion
}
