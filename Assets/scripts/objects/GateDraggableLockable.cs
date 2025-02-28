using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

[System.Serializable]
public class GateParams
{
	public bool isLocked;
}

[RequireComponent(typeof(SignalReceiver))]
public class GateDraggableLockable : DraggableGate
{
	#region Variables

	// Unity Editor Variables
	[SerializeField] protected bool isLocked;
	[SerializeField] protected Renderer woodRenderer;

	// Protected Instance Variables
	protected SignalReceiver signalReceiver = null;
	protected Color startColor = Color.white;
	protected Material woodMaterial;

	#endregion


	#region MonoBehaviour

	// Constructor
	protected override void Awake()
	{
		signalReceiver = gameObject.GetComponent<SignalReceiver>();
		Assert.IsNotNull(signalReceiver);

		Assert.IsNotNull(woodRenderer);

		base.Awake();
	}

	// Use this for initialization
	protected virtual void Start()
	{
		signalReceiver.AddGateSignalReceivedCallback(ReceiveSignal);
		rBody.isKinematic = isLocked;

		woodMaterial = woodRenderer.material;
		startColor = woodMaterial.color;

		if (isLocked)
		{
			Lock();
		}
		else
		{
			Unlock();
		}
	}

	// Called when the behaviour becomes disabled () or inactive.
	protected virtual void OnDisable()
	{
		signalReceiver.RemoveGateSignalReceivedCallback(ReceiveSignal);
	}

	#endregion


	#region Protected Functions 

	protected virtual void ReceiveSignal(float signalStrength, GateParams gateParams)
	{
		if (gateParams.isLocked)
		{
			Lock();
		}
		else
		{
			Unlock();
		}
	}

	#endregion


	#region Public Functions 

	public override void OnClicked(Vector3 worldPos)
	{
		if (!isLocked)
		{
			base.OnClicked(worldPos);
		}
	}

	public override void OnDragged(Vector3 worldPos)
	{
		if (!isLocked)
		{
			base.OnDragged(worldPos);
		}
	}

	public override void OnReleased(Vector3 worldPos)
	{
		if (!isLocked)
		{
			base.OnReleased(worldPos);	
		}
	}

	public void Lock()
	{
		isLocked = true;
		rBody.isKinematic = true;
		woodMaterial.color = startColor * 0.25f;
	}

	public void Unlock()
	{
		isLocked = false;
		rBody.isKinematic = false;
		woodMaterial.color = startColor;
	}

	#endregion
}
