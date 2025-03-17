using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

[System.Serializable]
public class ClickableObjectParams
{
	public AudioClip clickedSound;
}

[RequireComponent(typeof(AudioSource))]
public class ClickableObject : MonoBehaviour
{
	#region Variables 

	[SerializeField] protected bool disableSounds;

	// Protected Instance Variables
	protected bool isClicked = false;
	protected Transform trans = null;
	protected Rigidbody rBody = null;
	protected PlaySoundOnClick audioController = null;

	// Public Properties
	public bool SoundsEnabled { get { return !disableSounds; } set { disableSounds = !value; } }

	// Delegates
	public delegate void ClickableStateChange();

	// Callbacks
	protected ClickableStateChange clickedCallbacks = null;
	protected ClickableStateChange releasedCallbacks = null;

	#endregion


	#region MonoBehaviour 

	// Constructor
	protected virtual void Awake()
	{
		audioController = GetComponent<PlaySoundOnClick>();;
		Assert.IsNotNull(audioController);

		// Not having a rigid boy is causing a null reference exception
		
		//rBody = GetComponent<Rigidbody>();
		//Assert.IsNotNull(rBody);

		trans = transform;
		Assert.IsNotNull(trans);
	}

	#endregion


	#region Public Functions 

	public void AddClickCallback(ClickableStateChange newCallback) { clickedCallbacks += newCallback; }
	public void RemoveClickCallback(ClickableStateChange newCallback) { clickedCallbacks -= newCallback; }

	public void AddReleasedCallback(ClickableStateChange newCallback) { releasedCallbacks += newCallback; }
	public void RemoveReleasedCallback(ClickableStateChange newCallback) { releasedCallbacks -= newCallback; }

	public virtual void OnClicked(Vector3 worldPos)
	{
		this.Log("Clicked", DebugLogLevel.MediumDetails);

		isClicked = true;

		if (SoundsEnabled)
		{
			audioController.Play();	
		}

		if (clickedCallbacks != null)
		{
			clickedCallbacks();
		}
	}

	public virtual void OnReleased(Vector3 worldPos)
	{
		this.Log("OnReleased", DebugLogLevel.VeryDetailed);

		isClicked = false;

		if (releasedCallbacks != null)
		{
			releasedCallbacks();
		}
	}

	#endregion
}
