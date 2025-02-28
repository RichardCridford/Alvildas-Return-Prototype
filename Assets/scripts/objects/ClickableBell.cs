using UnityEngine;
using System.Collections;

public class ClickableBell : SoundEmitterClickable
{
	#region Public Functions 

	protected override void OnClicked()
	{
		audioController.Play();

		LevelController.SoundWave.CreateWave
		(
			new SoundWaveParameters()
			{
				type = WaveType.Bell,
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
