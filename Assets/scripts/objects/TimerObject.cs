using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

[System.Serializable]
public class TimerParams
{
	public bool enableOnStart;
	public float interval;
}

public sealed class TimerObject : MonoBehaviour
{
	#region Variables 

	// Public Properties
	public bool IsEnabled { get { return isEnabled; } }
	public float Interval { get; private set; }
	public float TimeStatus { get { return (Time.time - startTime) / Interval; } }

	// Protected Instance Variables
	private bool isEnabled = false;
	private float startTime = 0;

	// Delegates
	public delegate void OnTimerPassed();

	// Callbacks
	private OnTimerPassed timerPassedCallbacks = null;

	#endregion


	#region MonoBehaviour 

	// Update is called once per frame
	private void Update()
	{
		if (isEnabled)
		{
			if (Time.time - startTime > Interval)
			{
				startTime = Time.time;

				if (timerPassedCallbacks != null)
				{
					timerPassedCallbacks();
				}
			}
		}
	}

	#endregion


	#region Public Functions

	public void AddTimerPassedCallback(OnTimerPassed newCallback)
	{
		timerPassedCallbacks += newCallback;
	}

	public void RemoveTimerPassedCallback(OnTimerPassed newCallback)
	{
		timerPassedCallbacks -= newCallback;
	}

	public void StartTimer(TimerParams timeParams)
	{
		Interval = timeParams.interval;
		startTime = Time.time;
		isEnabled = true;
	}

	public void StopTimer()
	{
		isEnabled = false;
	}

	#endregion
}
