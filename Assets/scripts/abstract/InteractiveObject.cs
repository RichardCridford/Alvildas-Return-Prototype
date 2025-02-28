using UnityEngine;
using System.Collections;

public sealed class InteractiveObject : MonoBehaviour
{
	#region Variables 

	// Delegates
	public delegate void OnInteraction();

	// Callbacks
	private OnInteraction interactionCallbacks = null;

	#endregion


	#region Public Functions

	public void OnAlvildaInteraction()
	{
		if (interactionCallbacks != null)
		{
			interactionCallbacks();
		}
	}

	public void AddInteractionCallback(OnInteraction newCallback)
	{
		interactionCallbacks += newCallback;
	}

	public void RemoveInteractionCallback(OnInteraction newCallback)
	{
		interactionCallbacks -= newCallback;
	}

	#endregion
}
