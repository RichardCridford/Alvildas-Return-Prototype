using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class LeverObject : HornSoundEmitterClickable
{
	#region Variables 

	// Protected Instance Variables
	InteractiveObject intObject = null;
	SignalSender signalSender = null;

	#endregion


	#region MonoBehaviour

	// Constructor
	protected override void Awake()
	{
		intObject = gameObject.AddComponent<InteractiveObject>();
		Assert.IsNotNull(intObject);

		signalSender = gameObject.GetComponent<SignalSender>();
		Assert.IsNotNull(signalSender);

		base.Awake();
	}

	// Use this for initialization
	protected override void Start()
	{
		intObject.AddInteractionCallback(OnAlvildaInteraction);
		base.Start();
	}

	// Called when the behaviour becomes disabled () or inactive.
	protected override void OnDisable()
	{
		intObject.RemoveInteractionCallback(OnAlvildaInteraction);
		base.OnDisable();
	}

	#endregion


	#region Protected Functions

	protected void OnAlvildaInteraction()
	{
		this.Log("OnAlvildaInteraction() received.", DebugLogLevel.OnlyImportant);

		LevelController.Alvilda.ChangeState(AlvildaState.Occupied);
		signalSender.SendSignal();
	}

	#endregion


	#region Public Functions 

	public override void EmitWave()
	{
		audioController.Play();

		LevelController.SoundWave.CreateWave
		(
			new SoundWaveParameters()
			{
				type = WaveType.Lever,
				origin = transform.position,
				rotation = transform.rotation.eulerAngles.y,
				color = waveColor,
				degrees = waveDegrees,
				strength = waveStrength,
				attractionFactor = attractionStrength
			},
			gameObject
		);
	}

	#endregion
}
