using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

public class HornSoundEmitterClickableLimited : HornSoundEmitterClickable
{
	#region Variables 

	// Unity Editor Variables
	[SerializeField] int numOfUses;
	[SerializeField] GameObject[] hornModels;

	// Protected Instance Variables
	protected int useCounter = 0;

	#endregion


	#region MonoBehaviour

	// Use this for initialization
	protected override void Start()
	{
		clickableObj.SoundsEnabled = false;
		LevelController.Alvilda.AddAlvildaRespawnCallback(OnAlvildaRespawned);
		Initialize();
		base.Start();
	}

	// Called when the behaviour becomes disabled () or inactive.
	protected override void OnDisable()
	{
		LevelController.Alvilda.RemoveAlvildaRespawnCallback(OnAlvildaRespawned);
		base.OnDisable();
	}

	#endregion


	#region Protected Functions

	protected override void OnClicked()
	{
		if (useCounter < numOfUses)
		{
			useCounter += 1;

			if (hornModels.Length > numOfUses - useCounter)
			{
				hornModels[numOfUses - useCounter].SetActive(false);
			}

			base.OnClicked();	
		}
	}

	protected void OnAlvildaRespawned()
	{
		Initialize();
	}

	protected void Initialize()
	{
		Debug.Log("!!!!");
		if (numOfUses < hornModels.Length)
		{
			for (int i = 0; i < numOfUses; i++)
			{
				hornModels[i].SetActive(true);
			}

			for (int i = numOfUses; i < hornModels.Length; i++)
			{
				hornModels[i].SetActive(false);
			}
		}
		useCounter = 0;
	}

	#endregion
}
