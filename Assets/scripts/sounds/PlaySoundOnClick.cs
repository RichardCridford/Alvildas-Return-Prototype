using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

public class PlaySoundOnClick : MonoBehaviour
{
	#region Variables 

	// Unity Editor Variables
	[SerializeField] protected AudioClip[] clips;

	// Protected Instance Variables
	protected AudioSource audioSource;

	#endregion


	#region MonoBehaviour

	// Constructor
	protected virtual void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		Assert.IsNotNull(audioSource, "Error: Missing Audiosource on \"" + name + "\"");
	}

	#endregion


	#region Public Functions

	// Plays a random sound from the given soundclips array
	public void Play()
	{
		//if (!audioSource.isPlaying)
		{
			if (clips.Length > 0)
			{
				audioSource.clip = clips[Random.Range (0, clips.Length)];
				audioSource.Play();
			}
		}
	}

	// Plays a random sound from the given soundclips array and setting the pitch
	public void PlayVarPitch(float minPitch, float maxPitch)
	{
		//if (!audioSource.isPlaying)
		{
			if (clips.Length > 0)
			{
				audioSource.clip = clips[Random.Range (0, clips.Length)];
				audioSource.pitch = Random.Range (minPitch, maxPitch);
				audioSource.Play();
			}
		}
	}

	public void KeepLooping()
	{
		if (!audioSource.isPlaying)
		{
			if (clips.Length > 0)
			{
				audioSource.clip = clips[Random.Range (0, clips.Length)];
				audioSource.Play();
			}
		}
	}

	// Stops the sound playing
	public void Stop()
	{
		audioSource.Stop();
	} 

	#endregion
}
