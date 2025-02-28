using UnityEngine;
using System.Collections;

public class TestPlane : MonoBehaviour
{
	[System.Serializable]
	protected class Particle
	{
		public float accel = 0f;
		public float vel = 0f;
		public float pos = 0f;
		public GameObject obj = null;
	}

	// Protected Const Variables
	public float frequency;
	public float damping;
	public float mass;
	public float equilibriumSpringK;
	public float neighbourSpringK;
	public float POWER;
	public int PLANE_SIDE_LENGTH;

	// Protected Instance Variables
	protected Particle[,] particles = null;


	// Use this for initialization
	protected void Start()
	{
		particles = new Particle[PLANE_SIDE_LENGTH, PLANE_SIDE_LENGTH];
		for (int x = 0; x < PLANE_SIDE_LENGTH; x++)
		{
			for (int z = 0; z < PLANE_SIDE_LENGTH; z++)
			{
				Particle newParticle = new Particle();
				newParticle.obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				newParticle.obj.name = x + "_" + z;
				newParticle.obj.transform.position = new Vector3(x, 0, z);
				newParticle.obj.transform.parent = transform;
				particles[x,z] = newParticle;
			}	
		}

		LevelController.UserInput.AddClickCallback(OnClicked);
	}

	Particle clickedParticle = null;
	protected void OnClicked(GameObject objClicked, Vector3 mousePos, Vector3 worldPos, RaycastHit rayHit)
	{
		string[] indices = objClicked.name.Split('_');
		int x = 0;
		int z = 0;
		if (int.TryParse(indices[0], out x))
		{
			if (int.TryParse(indices[1], out z))
			{
				clickedParticle = particles[x,z];//.extraForce = Mathf.Sin(Time.time) * POWER;
			}
		}
	}

	// FixedUpdate is called every fixed framerate frame
	protected void FixedUpdate()
	{
		// Spring => F = - k * X
		// Acce ===> a = ∑ Force * Mass
		// Speed ==> v = v0 + accel * time
		// Pos  ===> s = v0 * time - 1/2 * velocity * time^2

		// foreach neighbour
		// F = pos - n.pos * x √ 

		// on click
		// p.f += Math.sin(Time.time) * Power

		float time = Time.fixedDeltaTime;

		if (Input.GetMouseButton(0))
		{
			if (clickedParticle != null)
			{
				clickedParticle.accel = Mathf.Sin(Time.time * frequency) * POWER * mass;
			}
		}

		for (int x = 0; x < PLANE_SIDE_LENGTH; x++)
		{
			for (int z = 0; z < PLANE_SIDE_LENGTH; z++)
			{
				Particle curParticle = particles[x,z];

				// Calculate Forces....

				// 
				float equilibriumForce = - equilibriumSpringK * curParticle.pos;
				curParticle.accel += equilibriumForce * mass;

				// Check the neighbours in X pos...
				for (int u = x - 1; u <= x + 1; u += 2)
				{
					if (u < 0) continue;
					if (u > PLANE_SIDE_LENGTH - 1) continue;

					// X = pos - n.pos * x
					// Spring => Force = - k * X
					// Acce ===> a = ∑ Force * Mass
					Particle neigbourParticle = particles[u,z];
					float XVar = curParticle.pos - neigbourParticle.pos;
					float force = - neighbourSpringK * XVar;
					curParticle.accel += force * mass;
				}

				// Check the neighbours in Z pos...
				for (int v = z - 1; v <= z + 1; v += 2)
				{
					if (v < 0) continue;
					if (v > PLANE_SIDE_LENGTH - 1) continue;

					// X = pos - n.pos
					// Spring => Force = - k * X
					// Acce ===> a = ∑ Force * Mass
					Particle neigbourParticle = particles[x,v];
					float XVar = curParticle.pos - neigbourParticle.pos;
					float force = - neighbourSpringK * XVar;
					curParticle.accel += force * mass;
				}

				// Speed ==> v = v0 + accel * time (Plus we add damping)
				curParticle.vel *= damping;
				float lastVel = curParticle.vel;
				curParticle.vel = lastVel + curParticle.accel * time;

				// Pos  ===> s = v0 * time - 1/2 * velocity * time^2
				curParticle.pos += (lastVel * time) - (0.5f * curParticle.vel * time * time);

				// Move the Gameobject...
				curParticle.obj.transform.position = new Vector3(curParticle.obj.transform.position.x, curParticle.pos, curParticle.obj.transform.position.z);

				// Cleanup
				curParticle.accel = 0f;
			}
		}
	}

	// Called when the behaviour becomes disabled () or inactive.
	protected void OnDisable()
	{
		LevelController.UserInput.RemoveClickCallback(OnClicked);
	}
}
