using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SoundEmitter : MonoBehaviour
{
	#region Variables 

	// Unity Editor Variables
	[SerializeField] protected WaveColor waveColor;
	[SerializeField] protected float waveStrength;
	[SerializeField] protected float attractionStrength;
	[SerializeField] protected SoundEmitterBehaviour behaviour;
	[SerializeField] protected SoundEmitter[] otherEmitters;

	// Enums
	protected enum SoundEmitterBehaviour
	{
		TurnOnEmitter,
		TurnOnAllEmitters,
		SwitchBetweenEmitters
	}

	// Protected Instance Variables
	protected int emitterIndex = 0;
	protected PlaySoundOnClick audioController = null;

	#endregion

	#region MonoBehaviour 

	// Constructor
	protected virtual void Awake()
	{
		audioController = GetComponent<PlaySoundOnClick>();
		Assert.IsNotNull(audioController, "Error: Missing PlaySoundOnClick on \"" + name + "\"");

		//Assert.IsTrue(waveStrength > 0, "Error: waveStrength can not be 0 or lower on \"" + name + "\"");
		//Assert.IsTrue(attractionStrength > 0, "Error: attractionStrength can not be 0 or lower on \"" + name + "\"");
	}

	// Use this for initialization
	protected virtual void Start()
	{
		
	}

	// Called when the behaviour becomes disabled () or inactive.
	protected virtual void OnDisable()
	{
		
	}

	#endregion


	#region Protected Functions 

	protected void TryToEmit()
	{
		if (!LevelController.Alvilda.IsDead)
		{
			switch(behaviour)
			{
				case SoundEmitterBehaviour.TurnOnEmitter:
					EmitWave();
					break;

				case SoundEmitterBehaviour.TurnOnAllEmitters:
					EmitWave();
					for (int i = 0; i < otherEmitters.Length; i++)
					{
						otherEmitters[i].EmitWave();
					}
					break;

				case SoundEmitterBehaviour.SwitchBetweenEmitters:
					if (emitterIndex == otherEmitters.Length)
					{
						EmitWave();
					}
					else
					{
						otherEmitters[emitterIndex].EmitWave();
					}

					emitterIndex = (emitterIndex + 1) % (otherEmitters.Length + 1);
					break;
			}
		}
	}

	#endregion


	#region Protected Functions 

	public virtual void EmitWave()
	{
		audioController.PlayVarPitch(0.9f, 1.0f);

		LevelController.SoundWave.CreateWave
		(
			new SoundWaveParameters()
			{
				type = WaveType.Normal,
				origin = transform.position,
				rotation = transform.rotation.eulerAngles.y,
				color = waveColor,
				degrees = GameConst.CIRCLE_DEGREES,
				strength = waveStrength,
				attractionFactor = attractionStrength
			},
			gameObject
		);
	}

	#endregion
}
