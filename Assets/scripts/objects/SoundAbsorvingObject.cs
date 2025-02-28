using UnityEngine;
using System.Collections;

public class SoundAbsorvingObject : WaveChangingObject
{
	#region Variables

	// Unity Editor Variables
	[SerializeField] protected float waveSpeedMultiplier;

	#endregion


	#region Public Functions

	public override void TakeWaveHit(SoundWaveObject wave, SoundWavePoint point)
	{
		point.speed *= waveSpeedMultiplier;
	}

	#endregion
}
