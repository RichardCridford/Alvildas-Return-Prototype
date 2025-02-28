using UnityEngine;
using System.Collections;

public class WindArea : MonoBehaviour
{
	#region MonoBehaviour 

	protected void OnTriggerEnter(Collider collider)
	{
		SignalReceiver sr = collider.GetComponent<SignalReceiver>();
		if (sr != null)
		{
			sr.ReceiveSignal(1f);
		}
	}

	protected void OnTriggerExit(Collider collider)
	{
		SignalReceiver sr = collider.GetComponent<SignalReceiver>();
		if (sr != null)
		{
			sr.ReceiveSignal(0f);
		}
	}

	#endregion

}
