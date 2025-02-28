using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

public class AlvildaCuriousMarker : MonoBehaviour
{
	// Unity Editor Variables
	[SerializeField] AudioClip turnOnSound; 

	// Protected Instance Variables
	protected MeshRenderer rend = null;
	protected AudioSource audioSource = null;

	// Constructor
	protected void Awake()
	{
		rend = GetComponent<MeshRenderer>();
		Assert.IsNotNull(rend);

		audioSource = GetComponent<AudioSource>();
		Assert.IsNotNull(audioSource);
	}

	// Use this for initialization
	protected void Start()
	{
		TurnOff();
	}

	#region Public Functions

	public void TurnOn()
	{
		rend.enabled = true;

		if (turnOnSound != null)
		{
			audioSource.PlayOneShot(turnOnSound);
		}
	}

	public void TurnOff()
	{
		rend.enabled = false;
	}

	#endregion
}
