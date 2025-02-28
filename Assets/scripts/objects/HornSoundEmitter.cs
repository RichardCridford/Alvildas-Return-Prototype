using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class HornSoundEmitter : SoundEmitter
{
	#region Variables 

	// Unity Editor Variables
	[SerializeField] protected float waveDegrees;

	#endregion


	#region MonoBehaviour 

	// Constructor
	protected override void Awake()
	{
		Assert.IsTrue(waveDegrees > 0, "Error: waveDegrees can not be 0 or lower!");
		base.Awake();
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
				type = WaveType.Horn,
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
