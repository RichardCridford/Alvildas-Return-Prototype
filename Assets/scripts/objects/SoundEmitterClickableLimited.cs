using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

public class SoundEmitterClickableLimited : SoundEmitterClickable
{
	#region Variables 

	// Unity Editor Variables
	[SerializeField] int numOfUses;
	[SerializeField] GameObject[] models;

	// Protected Instance Variables
	protected int useCounter = 0;
	protected Color col = Color.white;
	protected Material mat = null;
	protected Renderer rend = null;

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

			if (models.Length > numOfUses - useCounter)
			{
				models[numOfUses - useCounter].SetActive(false);
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
		if (numOfUses < models.Length)
		{
			for (int i = 0; i < numOfUses; i++)
			{
				models[i].SetActive(true);
			}

			for (int i = numOfUses; i < models.Length; i++)
			{
				models[i].SetActive(false);
			}
		}
		useCounter = 0;
	}

	#endregion
}
