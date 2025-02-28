using UnityEngine;
using System.Collections;

public class WaveDestructableObject : WaveAffectedObject
{
	#region Variables 

	// Unity Editor Variables
	[SerializeField] protected float strength;
	[SerializeField] protected bool canAccumulateDamage;
	[SerializeField] protected WaveColor[] onlyTakesDamageFrom;

	// Protected Instance Variables
	protected float curStrength = 0;

	#endregion


	#region MonoBehaviour

	protected void Start()
	{
		curStrength = strength;
	}
	
	// Update is called once per frame
	protected void Update ()
	{
		if (curStrength <= 0f)
		{
			enabled = false;
			Destroy(gameObject);
		}
	}

	#endregion


	#region Protected Functions 

	protected void TakeDamage(float damage)
	{
		if (canAccumulateDamage)
		{
			curStrength -= damage;
		}
		else
		{
			if (LevelController.SoundWave.GetSoundStrengthInProximity(transform.position) >= curStrength)
			{
				curStrength = 0f;
			}
		}
	}

	#endregion


	#region Public Functions 

	public override void TakeWaveHit(SoundWaveObject wave, SoundWavePoint point)
	{
		if (onlyTakesDamageFrom.Length > 0)
		{
			for (int i = 0; i < onlyTakesDamageFrom.Length; i++)
			{
				if (onlyTakesDamageFrom[i] == wave.color)
				{
					TakeDamage(point.strength);
					break;
				}
			}	
		}
		else
		{
			TakeDamage(point.strength);
		}
	}

	#endregion
}
