using UnityEngine;
using System.Collections;

public class GameObjectSwitcher : MonoBehaviour
{
	// Unity Editor Variables
	[SerializeField] protected float speed;
	[SerializeField] protected GameObject[] objects;
	[SerializeField] protected bool shouldPingPong;
	[SerializeField] protected bool randomInterval;
	[SerializeField] protected float minRandomInterval;
	[SerializeField] protected float maxRandomInterval;

	// Protected Instance Variables
	protected int lastIndex = 0;
	protected int objIndex = 0;
	protected float timer = 0f;
	protected float currentInterval = 0f;

	// Use this for initialization
	void Start()
	{
		for(int i = 1; i < objects.Length; i++)
		{
			objects[i].SetActive(false);
		}

		if (randomInterval)
		{
			timer = Time.time;
			currentInterval = CalculateNewRandomInterval();
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (randomInterval)
		{
			if (Time.time - timer > currentInterval)
			{
				objects[lastIndex].SetActive(false);
				objects[objIndex].SetActive(true);
				lastIndex = objIndex;
				objIndex = (objIndex + 1) % objects.Length;

				timer = Time.time;
				currentInterval = CalculateNewRandomInterval();
			}
		}
		else if (shouldPingPong)
		{
			objIndex = PingPong((int)(Time.time * speed), 0, 4);
			if (objIndex != lastIndex)
			{
				objects[lastIndex].SetActive(false);
				objects[objIndex].SetActive(true);
				lastIndex = objIndex;
			}
		}
		else
		{
			objIndex = Mathf.FloorToInt(Time.time * speed) % objects.Length;

			if (objIndex != lastIndex)
			{
				objects[lastIndex].SetActive(false);
				objects[objIndex].SetActive(true);
				lastIndex = objIndex;
			}
		}
	}

	protected float CalculateNewRandomInterval()
	{
		return UnityEngine.Random.Range(minRandomInterval, maxRandomInterval);
	}

	protected int PingPong(int input, int min, int max)
	{
		int range = max - min ;
		return min + Mathf.Abs(((input + range) % (range * 2)) - range);
	}
}
