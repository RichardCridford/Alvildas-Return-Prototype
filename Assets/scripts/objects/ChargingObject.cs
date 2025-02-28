using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

[System.Serializable]
public class ChargingParams
{
	public float chargeDuration;
}

public class ChargingObject : MonoBehaviour
{
	#region Variables 

	// Public Properties
	public bool IsCharging { get { return isCharging; } }
	public float ChargeStatus { get { return timerObj.TimeStatus; } }

	// Protected Instance Variables
	protected bool isCharging = false;
	protected float startChargeTime = 0f;
	protected float duration = 0f;
	protected TimerObject timerObj = null; 

	// Delegates
	public delegate void OnChargeComplete();

	// Callbacks
	protected OnChargeComplete chargeCompleteCallbacks = null;

	#endregion


	#region MonoBehaviour 

	// Constructor
	protected void Awake()
	{
		Renderer rend = GetComponent<Renderer>();
		Assert.IsNotNull(rend);

		timerObj = gameObject.AddComponent<TimerObject>();
		Assert.IsNotNull(timerObj);
	}

	//
	protected void Start()
	{
		timerObj.AddTimerPassedCallback(OnTimerPassed);
	}

	// Called when the behaviour becomes disabled () or inactive.
	protected void OnDisable()
	{
		timerObj.RemoveTimerPassedCallback(OnTimerPassed);
	}

	protected void OnTimerPassed()
	{
		timerObj.StopTimer();
		isCharging = false;

		if (chargeCompleteCallbacks != null)
		{
			chargeCompleteCallbacks();
		}
	}

	#endregion


	#region Public Functions

	public void AddChargeCompleteCallback(OnChargeComplete newCallback)
	{
		chargeCompleteCallbacks += newCallback;
	}

	public void RemoveChargeCompleteCallback(OnChargeComplete newCallback)
	{
		chargeCompleteCallbacks -= newCallback;
	}

	public virtual void StartCharging(ChargingParams chargeParams)
	{
		timerObj.StartTimer(
			new TimerParams()
			{
				interval = chargeParams.chargeDuration
			}
		);

		isCharging = true;
	}

	public virtual void StopCharging()
	{
		isCharging = false;
	}

	#endregion
}
