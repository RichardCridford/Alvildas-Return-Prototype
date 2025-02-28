using UnityEngine;
using System.Collections;

public class DontDestroyOnLoad : MonoBehaviour
{
	#region MonoBehaviour 

	// Use this for initialization
	protected void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	#endregion
}
