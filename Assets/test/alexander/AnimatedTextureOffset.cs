using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

public class AnimatedTextureOffset : MonoBehaviour
{
	// Unity Editor Variables
	[SerializeField] protected Vector2 speed;
	[SerializeField] protected int texWidth;
	[SerializeField] protected int texHeight;
	[SerializeField] protected float startDelay;
	[SerializeField] protected float delay;

	// Protected Instance Variables
	public Vector2 offset = Vector2.zero;
	protected Vector2 tiling = Vector2.zero;

	protected bool isStartWaiting = true;
	protected bool isWaiting = false;
	protected float tileTimer = 0f;
	protected float waitTimer = 0f;
	protected Material mat = null;

	// Use this for initialization
	protected void Start()
	{
		waitTimer = Time.time;

		MeshRenderer r = GetComponent<MeshRenderer>();
		Assert.IsNotNull(r);
		mat = r.material;
		Assert.IsNotNull(mat);
//		mat = GetComponent<MeshRenderer>().sharedMaterial;
		if (mat == null)
		{
			enabled = false;
		}
	}
	
	// Update is called once per frame
	protected void Update() 
	{
		tiling = new Vector2(1f / texWidth, 1f / texHeight);

		if (isStartWaiting)
		{
			if (Time.time - waitTimer > startDelay)
			{
				isStartWaiting = false;
			}
			else
			{
				return;
			}
		}


		if (isWaiting)
		{
			//Debug.Log((Time.time - waitTimer) + " > " + delay);

			if (Time.time - waitTimer > delay)
			{
				//Debug.Log("-----------------------------");
				isWaiting = false;
				tileTimer = 0f;
			}
		}

		if (!isWaiting)
		{
			//mat.mainTextureScale = tiling;
			tileTimer += Time.deltaTime;
			offset.x = tiling.x * Mathf.CeilToInt(tileTimer * speed.x);
			offset.y = tiling.y * Mathf.CeilToInt(tileTimer * speed.y);

			offset.x %= 1f;
			offset.y %= 1f;

			//Debug.Log("offset.y = " + offset.y + " = " + tileTimer + " * " + speed.y);
			if (offset.y == 0f)
			{
				isWaiting = true;
				waitTimer = Time.time;
			}

			mat.mainTextureOffset = offset;
		}
	}
}
