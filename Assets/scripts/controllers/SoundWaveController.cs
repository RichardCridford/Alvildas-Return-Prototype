using UnityEngine;
using UnityEngine.Assertions;
using System;
using System.Collections;
using System.Collections.Generic;

public class SoundWaveParameters
{
	public float rotation = 0f;
	public float degrees = 0f;
	public float strength = 0f;
	public float attractionFactor = 0f;
	public Vector3 origin = Vector3.zero;
	public WaveType type = WaveType.Normal;
	public WaveColor color = WaveColor.White;
}

public class SoundWaveObject
{
	public int ID = 0;
	public float rotation = 0f;
	public float degrees = 0f;
	public Vector3 origin = Vector3.zero;
	public WaveType type = WaveType.Normal;
	public WaveColor color = WaveColor.White;
	public Material material = null;
	public GameObject soundObj = null;
	public GameObject waveObj = null;
	public LineRenderer lineRend = null;
	public SoundWavePoint[] points = null;

	public Mesh mesh = null;
	public int[] idx = null;
	//public Vector3[] points = null;
	//public Vector3[] pointsDir = null;
	public Vector3[] vtx = null;
	public Vector3[] nrm = null;
	public Vector2[] uv0 = null;
}

public class SoundWavePoint
{
	public bool IsDead { get { return !isMoving && !isAttracting; } }
	public float StrengthPercentage { get { return strength / startStrength; } }
	public Vector3 pos { get; set; }
	public Vector3 TargetPos { get { return targetHit.point; } }
	public GameObject TargetObj { get { return (targetHit.collider) ? targetHit.collider.gameObject : null; } }
	public WaveAffectedObject AffectedObj { get { return ( (targetHit.collider) ? targetHit.collider.gameObject.GetComponent<WaveAffectedObject>() : null); } }
	public WaveChangingObject ChangingObj { get { return ( (targetHit.collider) ? targetHit.collider.gameObject.GetComponent<WaveChangingObject>() : null); } }

	public Ray ray = new Ray();
	public RaycastHit targetHit = new RaycastHit();
	public bool isMoving = true;
	public bool isAttracting = true;
	public int numOfBounces = 0;
	public float speed = 0f;
	public float strength = 0;
	public float startStrength = 0f;
	public float attractionStrength = 1f;
	public Vector3 DirToTarget = Vector3.zero;
}

public class SoundWaveController : MonoBehaviour
{
	#region Variables

	// Unity Editor Variables
	[SerializeField] protected float radius;
	[SerializeField] protected float waveheight;
	[SerializeField] protected float waveSize;
	[SerializeField] protected float waveDistortion;
	[SerializeField] protected float waveColorBrightness;
	[SerializeField] protected Shader waveShader;
	[SerializeField] protected Texture2D normalTexture;
	[SerializeField] protected GameObject batchPrefab;

	// Protected Const Variables
	protected const float RAY_DISTANCE = 5000f;
	protected const float SPEED_OF_SOUND = 340.29f;
	protected const float WAVE_SPEED_OF_SOUND_MULTIPLIER = 0.075f;
	protected const float SOUND_PROXIMITY_DISTANCE = 1f;
	protected const float WAVEPOINTS_PER_DEGREE = 1f;

	// Protected Instance Variables
	protected List<SoundWaveObject> waveBatches = new List<SoundWaveObject>();
	protected List<SoundWaveObject> deadWaveBatches = new List<SoundWaveObject>();

	#endregion


	#region MonoBehaviour

	// FixedUpdate is called every fixed framerate frame
	protected void FixedUpdate()
	{
		for (int batchIndex = 0; batchIndex < waveBatches.Count; batchIndex++)
		{
			bool areAllPointsDead = true;
			for(int i = 0; i < waveBatches[batchIndex].points.Length; i++)
			{
				SoundWavePoint point = waveBatches[batchIndex].points[i];
				point.isAttracting &= point.attractionStrength > 0f;
				point.isMoving &= point.strength > 0f;

				areAllPointsDead &= point.IsDead;

				point.strength -= Time.fixedDeltaTime;
				point.attractionStrength -= Time.fixedDeltaTime;

				// Skip if the point isn't moving
				if (!point.isMoving)
				{
					continue;
				}

				float lastDist = Vector3.Distance(point.pos, point.TargetPos);
				Vector3 newPos = point.pos + point.DirToTarget * Time.fixedDeltaTime * point.speed;
				float newDist = Vector3.Distance(newPos, point.TargetPos);

				// If we've gone past the target...
				if (newDist > lastDist)
				{
					// Mark a bounce...
					point.numOfBounces++;

					// If we hit an object that is affected by the wave...
					if (point.AffectedObj != null)
					{
						point.AffectedObj.TakeWaveHit(waveBatches[batchIndex], point);
					}
					if (point.ChangingObj != null)
					{
						point.ChangingObj.TakeWaveHit(waveBatches[batchIndex], point);
					}

					CalculateNewTarget(point, (newDist - lastDist));
				}
				else
				{
					CheckTargetPath(point);

					point.pos = newPos;
				}
			}

			if (areAllPointsDead)
			{
				deadWaveBatches.Add(waveBatches[batchIndex]);
			}
		}

		// If we should remove a wave batch...
		if (deadWaveBatches.Count > 0)
		{
			for (int batchIndex = 0; batchIndex < deadWaveBatches.Count; batchIndex++)
			{
				//Destroy(deadWaveBatches[batchIndex].lineRend.gameObject);
				Destroy(deadWaveBatches[batchIndex].waveObj);
				waveBatches.Remove(deadWaveBatches[batchIndex]);
			}

			deadWaveBatches.Clear();
		}
	}

	// Update is called once per frame
	protected void Update()
	{
		// Draw...
		for (int batchIndex = 0; batchIndex < waveBatches.Count; batchIndex++)
		{
			waveBatches[batchIndex].material.SetFloat("_Distortion", waveDistortion);

			float lifeStatus = waveBatches[batchIndex].points[0].StrengthPercentage;
			Color c = waveBatches[batchIndex].color.RGBA() * waveColorBrightness;
			c.a = lifeStatus;
			waveBatches[batchIndex].material.SetColor("_Color", c);
			CalculateVertices(waveBatches[batchIndex]);
			//UpdateWaveRenderer(batchIndex);
		}
	}

	protected void UpdateWaveRenderer(int batchIndex)
	{
		if (waveBatches[batchIndex].lineRend && waveBatches[batchIndex].points != null)
		{
			SoundWaveObject wave = waveBatches[batchIndex];
			int numOfPoints = wave.points.Length;
			if (numOfPoints > 0)
			{
				for(int i = 0; i <= wave.points.Length; i++)
				{
					int index = i % numOfPoints;
					float lifeStatus = wave.points[index].StrengthPercentage;
					Color c = wave.color.RGBA();
					c.a = lifeStatus;

					wave.lineRend.SetColors(c, c);
					wave.lineRend.SetPosition(i, wave.points[index].pos);
				}	
			}
		}
	}



	#endregion


	#region Public Functions

	protected void CheckTargetPath(SoundWavePoint point)
	{
		// Calculate new target
		Vector3 oldTarget = point.TargetPos;
		RaycastHit hit = new RaycastHit();
		Ray ray = new Ray(point.pos, point.DirToTarget);
		if (Physics.Raycast(ray, out hit, RAY_DISTANCE))
		{
			if (hit.point != point.TargetPos)
			{
				point.ray = ray;
				point.targetHit = hit;
				point.DirToTarget = (point.TargetPos - point.pos).normalized;
				point.pos = oldTarget + (point.TargetPos - oldTarget).normalized * Time.fixedDeltaTime * point.speed;
			}
		}
	}

	protected void CalculateNewTarget(SoundWavePoint point, float startMultiplier)
	{
		// Calculate the next direction...
		Vector3 dir = Vector3.zero;
		if (point.TargetObj != null)
		{
			dir = Vector3.Reflect(point.DirToTarget, point.targetHit.normal).normalized;
		}
		else
		{
			dir = point.DirToTarget;
		}

		// Calculate new target
		Vector3 oldTarget = point.TargetPos;
		point.ray = new Ray(oldTarget, dir);
		if (Physics.Raycast(point.ray, out point.targetHit, RAY_DISTANCE))
		{
			point.DirToTarget = (point.TargetPos - point.pos).normalized;
			point.pos = oldTarget + (point.TargetPos - oldTarget).normalized * Time.fixedDeltaTime * point.speed * startMultiplier;
		}

		// If we're unable to find another target we kill the wavepoint
		else
		{
			point.isMoving = false;
		}
	}

	// Creates the wave and calculates the points on the wave
	protected void CreateWaveBatchAndPoints(SoundWaveParameters waveParams, GameObject soundObj)
	{
		SoundWaveObject wb = new SoundWaveObject();
		wb.soundObj = soundObj;
		wb.origin = waveParams.origin;
		wb.rotation = -waveParams.rotation;
		wb.degrees = waveParams.degrees;
		wb.color = waveParams.color;
		wb.type = waveParams.type;
		wb.material = new Material(waveShader);
		wb.material.SetTexture("_NormalMap", normalTexture);
		wb.material.hideFlags = HideFlags.DontSave;

		int numOfPoints = (int) (WAVEPOINTS_PER_DEGREE * wb.degrees);
		float theta = wb.rotation * Mathf.Deg2Rad;
		wb.points = new SoundWavePoint[numOfPoints];
		for(int i = 0; i < numOfPoints; i++)
		{
			wb.points[i] = new SoundWavePoint();
			wb.points[i].speed = SPEED_OF_SOUND * WAVE_SPEED_OF_SOUND_MULTIPLIER;
			wb.points[i].strength = waveParams.strength;
			wb.points[i].startStrength = waveParams.strength;
			wb.points[i].attractionStrength = waveParams.attractionFactor;

			theta += (wb.degrees / numOfPoints * Mathf.Deg2Rad);
			Vector3 pointPos = wb.origin + new Vector3(radius * Mathf.Cos(theta), 0f, radius * Mathf.Sin(theta));
			Vector3 pointDir = (pointPos - wb.origin).normalized;

			// Find the next target
			Ray ray = new Ray(pointPos, pointDir);
			RaycastHit rayHit = new RaycastHit();

			if (Physics.Raycast(ray, out rayHit, RAY_DISTANCE))
			{
				wb.points[i].pos = pointPos;
				wb.points[i].targetHit = rayHit;
				wb.points[i].DirToTarget = (wb.points[i].TargetPos - wb.points[i].pos).normalized;
			}
		}

		// Add a gameobject with a lineRenderer...
		wb.waveObj = Instantiate(batchPrefab) as GameObject;
		wb.waveObj.transform.position = wb.origin;
		wb.waveObj.transform.parent = transform;
		wb.waveObj.transform.Rotate(new Vector3(0f, 180f, 0f));
		wb.waveObj.GetComponent<Renderer>().material = wb.material;

		wb.ID = wb.waveObj.GetInstanceID();
		//wb.lineRend = wb.waveObj.GetComponent<LineRenderer>();
		//wb.lineRend.SetVertexCount(wb.points.Length + 1);





		wb.vtx = new Vector3[numOfPoints * 6];	// 
		wb.nrm = new Vector3[wb.vtx.Length];	// Each vertice needs a normal
		wb.uv0 = new Vector2[wb.vtx.Length];	// 
		wb.idx = new int[numOfPoints * 12];		// 

		int indTemp = 0;
		for (int i = 0; i < numOfPoints; i++)
		{
			wb.nrm[indTemp + 0] = Vector3.up;
			wb.nrm[indTemp + 1] = Vector3.up;
			wb.nrm[indTemp + 2] = Vector3.up;
			wb.nrm[indTemp + 3] = Vector3.up;
			wb.nrm[indTemp + 4] = Vector3.up;
			wb.nrm[indTemp + 5] = Vector3.up;

			wb.uv0[indTemp + 0] = new Vector2(((i + 0) / ((float) numOfPoints)), 0f);
			wb.uv0[indTemp + 1] = new Vector2(((i + 0) / ((float) numOfPoints)), 0.5f);
			wb.uv0[indTemp + 2] = new Vector2(((i + 0) / ((float) numOfPoints)), 1f);
			wb.uv0[indTemp + 3] = new Vector2(((i + 1) / ((float) numOfPoints)), 0f);
			wb.uv0[indTemp + 4] = new Vector2(((i + 1) / ((float) numOfPoints)), 0.5f);
			wb.uv0[indTemp + 5] = new Vector2(((i + 1) / ((float) numOfPoints)), 1f);

			indTemp += 6;
		}

		indTemp = 0;
		int k = 0;
		for (int i = 0; i < numOfPoints; i++)
		{
			wb.idx[indTemp + 0] = k + 0;
			wb.idx[indTemp + 1] = k + 1;
			wb.idx[indTemp + 2] = k + 3;

			wb.idx[indTemp + 3] = k + 1;
			wb.idx[indTemp + 4] = k + 4;
			wb.idx[indTemp + 5] = k + 3;

			wb.idx[indTemp + 6] = k + 1;
			wb.idx[indTemp + 7] = k + 2;
			wb.idx[indTemp + 8] = k + 4;

			wb.idx[indTemp + 9] = k + 2;
			wb.idx[indTemp +10] = k + 5;
			wb.idx[indTemp +11] = k + 4;

			indTemp += 12;
			k += 6;
		}

		wb.mesh = new Mesh();
		CalculateVertices(wb);
		wb.mesh.hideFlags = HideFlags.DontSave;
		wb.mesh.vertices = wb.vtx;
		wb.mesh.normals = wb.nrm;
		wb.mesh.uv = wb.uv0;
		wb.mesh.SetIndices(wb.idx, MeshTopology.Triangles, 0);
		wb.mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 1000);

		// Add the batch to the list
		waveBatches.Add(wb);

		UpdateWaveRenderer(waveBatches.Count - 1);
	}

	// Called by the editor to draw gizmos that are also pickable
	protected void OnDrawGizmos()
	{
		if (GameController.InDebugDrawMode)
		{
			for (int batchIndex = 0; batchIndex < waveBatches.Count; batchIndex++)
			{
				SoundWaveObject wave = waveBatches[batchIndex];
				if (wave.points != null)
				{
					for(int i = 0; i < wave.points.Length; i++)
					{
						float lifeStatus = wave.points[i].StrengthPercentage;
						Color c = wave.color.RGBA();
						c.a = 1f - lifeStatus;
						Gizmos.color = c;

						Gizmos.DrawSphere(wave.points[i].pos, 0.25f);
					}
				}
			}
		}
	}

	void CalculateVertices(SoundWaveObject wave)
	{
		int numOfPoints = wave.points.Length;
		int indTemp = 0;
		for (int i = 0; i < numOfPoints; i++)
		{
			// Center points...
			wave.vtx[indTemp + 0] = wave.points[(i + 0)              ].pos - wave.points[i + 0].DirToTarget * waveSize;
			wave.vtx[indTemp + 1] = wave.points[(i + 0)              ].pos + new Vector3(0f, waveheight, 0f);
			wave.vtx[indTemp + 2] = wave.points[(i + 0)              ].pos + wave.points[i + 0].DirToTarget * waveSize;

			wave.vtx[indTemp + 3] = wave.points[(i + 1) % numOfPoints].pos - wave.points[(i + 1) % numOfPoints].DirToTarget * waveSize;
			wave.vtx[indTemp + 4] = wave.points[(i + 1) % numOfPoints].pos + new Vector3(0f, waveheight, 0f);
			wave.vtx[indTemp + 5] = wave.points[(i + 1) % numOfPoints].pos + wave.points[(i + 1) % numOfPoints].DirToTarget * waveSize;

			if (Vector3.Distance(wave.vtx[indTemp + 1], wave.vtx[indTemp + 4]) > 3f)
			{
				Vector3 dir = (wave.vtx[indTemp + 4] - wave.vtx[indTemp + 1]).normalized * 0.5f;

				wave.vtx[indTemp + 3] = wave.vtx[indTemp + 0] + dir;
				wave.vtx[indTemp + 4] = wave.vtx[indTemp + 1] + dir;
				wave.vtx[indTemp + 5] = wave.vtx[indTemp + 2] + dir;
			}

			indTemp += 6;
		}

		wave.mesh.vertices = wave.vtx;
		Graphics.DrawMesh(wave.mesh, Vector3.zero, Quaternion.identity, wave.material, 0);
	}

	#endregion


	#region Public Functions

	public void CreateWave(SoundWaveParameters waveParams, GameObject soundObj)
	{
		this.Log("CreateWave(" + ")", DebugLogLevel.VeryDetailed);

		waveParams.origin.y = GameConst.HEIGHT;
		CreateWaveBatchAndPoints(waveParams, soundObj);
	}

	public float GetSoundStrengthInProximity(Vector3 worldPos)
	{
		worldPos.y = GameConst.HEIGHT;
		float totalStrength = 0f;
		for (int batchIndex = 0; batchIndex < waveBatches.Count; batchIndex++)
		{
			for(int i = 0; i < waveBatches[batchIndex].points.Length; i++)
			{
				SoundWavePoint point = waveBatches[batchIndex].points[i];
				if (Vector3.Distance(point.pos, worldPos) <= SOUND_PROXIMITY_DISTANCE)
				{
					totalStrength += point.strength;
				}
			}
		}

		return totalStrength;
	}

	#endregion

}
