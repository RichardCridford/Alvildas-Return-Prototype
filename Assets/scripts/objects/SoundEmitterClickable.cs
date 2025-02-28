using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SoundEmitterClickable : SoundEmitter
{
	#region Variables 

	// Unity Editor Variables
	[SerializeField] ClickableObjectParams clickableParams;

	// Protected Instance Variables
	protected ClickableObject clickableObj = null;

	#endregion


	#region MonoBehaviour 

	// Constructor
	protected override void Awake()
	{
		clickableObj = gameObject.AddComponent<ClickableObject>();
		base.Awake();
	}

	// Use this for initialization
	protected override void Start()
	{
		clickableObj.AddClickCallback(OnClicked);
		clickableObj.AddReleasedCallback(OnReleased);
		base.Start();
	}

	// Called when the behaviour becomes disabled () or inactive.
	protected override void OnDisable()
	{
		clickableObj.RemoveClickCallback(OnClicked);
		clickableObj.RemoveReleasedCallback(OnReleased);
		base.OnDisable();
	}

	#endregion


	#region Protected Functions 

	protected virtual void OnClicked()
	{
		TryToEmit();
	}

	protected virtual void OnReleased()
	{
		
	}

	#endregion
}
