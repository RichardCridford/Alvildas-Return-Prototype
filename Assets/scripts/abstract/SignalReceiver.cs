using UnityEngine;
using System.Collections;

public sealed class SignalReceiver : MonoBehaviour
{
	#region Variables 

	// Delegates
	public delegate void OnSignalReceived(float signalStrength);
	public delegate void OnSignalReceivedGate(float signalStrength, GateParams gp);

	// Callbacks
	private OnSignalReceived signalReceivedCallbacks = null;
	private OnSignalReceivedGate gateSignalReceivedCallbacks = null;

	#endregion


	#region Public Functions

	public void ReceiveSignal(float signalStrength)
	{
		if (signalReceivedCallbacks != null)
		{
			signalReceivedCallbacks(signalStrength);
		}
		else
		{
			this.LogWarning("No listener for normal Signal Received", DebugLogLevel.OnlyImportant);
		}
	}

	public void ReceiveSignal(float signalStrength, GateParams gateParams)
	{
		if (gateSignalReceivedCallbacks != null)
		{
			gateSignalReceivedCallbacks(signalStrength, gateParams);
		}

		else
		{
			this.LogWarning("No listener for gate Signal Received", DebugLogLevel.OnlyImportant);
		}
	}

	public void AddSignalReceivedCallback(OnSignalReceived newCallback)
	{
		signalReceivedCallbacks += newCallback;

	}

	public void RemoveSignalReceivedCallback(OnSignalReceived newCallback)
	{
		signalReceivedCallbacks -= newCallback;
	}

	public void AddGateSignalReceivedCallback(OnSignalReceivedGate newCallback)
	{
		gateSignalReceivedCallbacks += newCallback;
	}

	public void RemoveGateSignalReceivedCallback(OnSignalReceivedGate newCallback)
	{
		gateSignalReceivedCallbacks -= newCallback;
	}

	#endregion
}
