using UnityEngine;
using System.Collections;

public sealed class SignalSender : MonoBehaviour
{
	#region Variables

	// Unity Editor Variables
	[Header("Normal Signal Receivers")]
	[SerializeField] private SignalReceiver[] signalReceivers;

	[Space]

	[Header("Gate Receivers")]
	[SerializeField] private GateParams gateParams;
	[SerializeField] private SignalReceiver[] gateSignalReceivers;

	#endregion


	#region Public Functions

	public void SendSignal()
	{
		for (int i = 0; i < signalReceivers.Length; i++)
		{
			if (signalReceivers[i] != null)
			{
				signalReceivers[i].ReceiveSignal(1f);
			}
		}

		for (int i = 0; i < gateSignalReceivers.Length; i++)
		{
			if (gateSignalReceivers[i] != null)
			{
				gateSignalReceivers[i].ReceiveSignal(1f, gateParams);	
			}
		}
	}

	#endregion
}
