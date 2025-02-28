using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

public class LightEffect : MonoBehaviour
{
	// Unity Editor Variables
	[Header("Intensity")]
	[SerializeField] protected float newIntensityDelay;
	[SerializeField] protected Vector2 intensityRandom;

	[Space]

	[Header("Transform")]
	[SerializeField] protected float transformNewPosDelay;
	[SerializeField] protected Vector2 transformXRandom;
	[SerializeField] protected Vector2 transformYRandom;
	[SerializeField] protected Vector2 transformZRandom;

	// Protected Instance Variables
	protected float transTimer = 0f;
	protected float lightTimer = 0f;
	protected float intensityStartVal = 0f;
	protected Light myLight = null;
	protected Vector3 startPos = Vector3.zero;
	protected Transform myTrans = null;

	// Constructor
	protected void Awake ()
	{
		myLight = GetComponent<Light>();
		Assert.IsNotNull(myLight);

		myTrans = transform;
		Assert.IsNotNull(myTrans);
	}

	// Use this for initialization
	protected void Start()
	{
		intensityStartVal = myLight.intensity;
		lightTimer = Time.time;

		transTimer = Time.time;
		startPos = myTrans.position;
	}
	
	// Update is called once per frame
	protected void Update()
	{
		// Intensity
		if (Time.time - lightTimer > newIntensityDelay)
		{
			myLight.intensity = intensityStartVal + Random.Range(intensityRandom.x, intensityRandom.y);
			lightTimer = Time.time;
		}

		// Transform
		if (Time.time - transTimer > transformNewPosDelay)
		{
			myTrans.position = startPos + 
				new Vector3(
					Random.Range(transformXRandom.x, transformXRandom.y),
					Random.Range(transformYRandom.x, transformYRandom.y),
					Random.Range(transformZRandom.x, transformZRandom.y)
				);

			transTimer = Time.time;
		}

	}
}
