using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

public class DeathSpikes : MonoBehaviour
{
	#region Variables

	// Protected Instance Variables
	protected PlaySoundOnClick audioController = null;

	#endregion


	#region MonoBehaviour 

	// Constructor
	protected virtual void Awake()
	{
		audioController = GetComponent<PlaySoundOnClick>();
		Assert.IsNotNull(audioController, "Error: Missing PlaySoundOnClick on \"" + name + "\"");
	}


	// OnTriggerEnter is called when the Collider other enters the trigger
	protected void OnTriggerEnter(Collider other)
	{
		if (other.IsAlvilda())
		{
			audioController.Play();
			LevelController.Alvilda.Die();
		}
	}

	#endregion
}

