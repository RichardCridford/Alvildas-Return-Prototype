using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundColorChanger : WaveChangingObject
{
	#region Variables 

	// Unity Editor Variables
	[SerializeField] protected WaveColor waveColor;

	#endregion


	#region Public Functions

	// Abstract Functions
	public override void TakeWaveHit(SoundWaveObject wave, SoundWavePoint point)
	{
		wave.color = waveColor;	
	}

	#endregion
}
