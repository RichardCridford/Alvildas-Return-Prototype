using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

[System.Serializable]
public class DraggableObjectParams
{
	
}

public class DraggableObject : ClickableObject
{
	#region Variables

	// Unity Editor Variables
	[SerializeField] protected bool freezePositionX;
	[SerializeField] protected bool freezePositionY;
	[SerializeField] protected bool freezePositionZ;

	// Protected Instance Variables
	protected Vector3 startMouseWorldPos = Vector3.zero;
	protected Vector3 startTransPos = Vector3.zero;
	protected Vector3 mouseWorldMovement = Vector3.zero;
	protected Vector3 lastMouseWorldPos = Vector3.zero;

	// Callbacks
	protected ClickableStateChange draggedCallbacks = null;

	#endregion


	#region MonoBehaviour

	protected virtual void FixedUpdate()
	{
		if (!isClicked)
		{
			return;
		}

		// Calculate the Mouse movement in world positions
		mouseWorldMovement = (lastMouseWorldPos - startMouseWorldPos);
		if (freezePositionX) mouseWorldMovement.x = 0f;
		if (freezePositionY) mouseWorldMovement.y = 0f;
		if (freezePositionZ) mouseWorldMovement.z = 0f;

		// Move the object
		rBody.velocity = Vector3.zero;
		rBody.angularVelocity = Vector3.zero;

		Vector3 move = (startTransPos + mouseWorldMovement) - rBody.position;
		move.y = 0f;
		if (!Physics.BoxCast(rBody.position, Vector3.one, move, Quaternion.identity, 2f))
		{
			rBody.MovePosition(rBody.position + move);
		}
	}

	#endregion


	#region Public Functions 

	public void AddDraggedCallback(ClickableStateChange newCallback) { draggedCallbacks += newCallback; }
	public void RemoveDraggedCallback(ClickableStateChange newCallback) { draggedCallbacks -= newCallback; }

	public override void OnClicked(Vector3 worldPos)
	{
		startMouseWorldPos = worldPos;
		lastMouseWorldPos = worldPos;
		startTransPos = trans.position;
		base.OnClicked(worldPos);
	}

	public virtual void OnDragged(Vector3 worldPos)
	{
		this.Log("OnDragged \"" + name + "\" " + lastMouseWorldPos, DebugLogLevel.VeryDetailed);

		lastMouseWorldPos = worldPos;

		if (!disableSounds)
		{
			audioController.KeepLooping();
		}

		if (draggedCallbacks != null)
		{
			draggedCallbacks();
		}
	}

	public override void OnReleased(Vector3 worldPos)
	{
		if (!disableSounds)
		{
			audioController.Stop();
		}

		base.OnReleased(worldPos);
	}

	#endregion
}
