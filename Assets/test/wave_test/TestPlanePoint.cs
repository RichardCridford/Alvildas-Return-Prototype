using UnityEngine;
using System.Collections;

public class TestPlanePoint : MonoBehaviour
{
	public TestPlanePoint Up { get; set; }
	public TestPlanePoint Down { get; set; }
	public TestPlanePoint Right { get; set; }
	public TestPlanePoint Left { get; set; }

	public bool HasUpdatedHeight { get; set; }

	const float SPEED = 5f;
	const float DAMPING = 0.01f;
	const float REST_HEIGHT = 0f;
	const float CLICKED_HEIGHT = 5f;
	const int NUM = 15;

	protected Vector3 velocity = Vector3.zero;


	// Use this for initialization
	void Start()
	{
		
	}

	void Update()
	{
		HasUpdatedHeight = false;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (transform.position.y > 0f)
		{
			transform.position += Vector3.down * Time.fixedDeltaTime * SPEED;
		}
	}

	public void UpdateHeight(float neighbourheight)
	{
		
		HasUpdatedHeight = true;

		float newHeight = neighbourheight;// - CLICKED_HEIGHT / (NUM);
		transform.position += new Vector3(0f, newHeight, 0f);
		UpdateNeighbours(transform.position.y);
	}

	public void Clicked()
	{
		this.Log("Clicked \"" + name + "\"", DebugLogLevel.MediumDetails);

		HasUpdatedHeight = true;
		velocity = Vector3.up * 3f;

		float newHeight = CLICKED_HEIGHT;
		transform.position += new Vector3(0f, newHeight, 0f);
		UpdateNeighbours(transform.position.y);
	}

	void UpdateNeighbours(float height)
	{
		if (Up && !Up.HasUpdatedHeight)
		{
			Up.HasUpdatedHeight = true;
			Up.UpdateHeight(height);
		}

		if (Down && !Down.HasUpdatedHeight)
		{
			Down.HasUpdatedHeight = true;
			Down.UpdateHeight(height);
		}

		if (Left && !Left.HasUpdatedHeight)
		{
			Left.HasUpdatedHeight = true;
			Left.UpdateHeight(height);
		}

		if (Right && !Right.HasUpdatedHeight)
		{
			Right.HasUpdatedHeight = true;
			Right.UpdateHeight(height);
		}
	}
}
