using UnityEngine;
using System.Collections;

public abstract class WaveChangingObject : MonoBehaviour
{
	// Abstract Functions
	public abstract void TakeWaveHit(SoundWaveObject wave, SoundWavePoint point);
}
