using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

public class AlvildaController : WaveAffectedObject
{
    #region Variables 



    public Transform pppppp;

    // I wrote this 
    private GameObject Alvilda;
    private Transform TrainSpace;
    //
	
    
    
    // Unity Editor Variables
	[Header("Curious State")]
	[SerializeField] protected float curiousToIdleDelay;

	[Header("Idle State")]
	[SerializeField] protected float idleToWanderingDelay;

	[Header("Wandering State")]
	[SerializeField] protected float newWanderingTargetDelay;
	[SerializeField] protected float minDistanceToWanderingTarget;

	[Space]

	[SerializeField] protected Transform modelTransform;
	[SerializeField] protected Material mat = null;

	[Space]

	[Header("****************************")]
	[Header("Info Variables")]
	[Header("****************************")]
	[SerializeField] protected AlvildaState currentState;


	// Public Properties
	public bool IsDead { get { return state == AlvildaState.Dead; } }
	public Vector3 Position { get { return trans.position; } }
	public AlvildaState State { get { return state; } }
	public AlvildaCheckPointController CheckPointController { get { return checkPointController; } }

	// Protected Const Variables
	protected float RAYCAST_LENGTH = 100f;

	// Protected Instance Variables
	protected bool isAtWanderingTarget = false;
	protected float timer = 0f;
	protected Vector3 wanderingTarget = Vector3.zero;
	protected Color col = Color.white;
	protected Transform trans = null;
	protected GameObject wanderingTargetObj = null;
	protected AlvildaState state = AlvildaState.Idle;
	protected MeshRenderer rend = null;
	protected SoundWavePoint lastHitPoint = null;
	protected SoundWaveObject lastWave = null;
	protected ClickableObject clickObj = null;
	protected AlvildaMovement movement = null;
	protected AlvildaCuriousMarker curiousMarker = null;
	protected AlvildaCheckPointController checkPointController = null;

	// Delegates
	public delegate void AlvildaChangeState(AlvildaState newState);
	public delegate void AlvildaDeath();
	public delegate void AlvildaRespawn();

	// Callbacks
	protected AlvildaDeath onDeathCallbacks = null;
	protected AlvildaRespawn onRespawnCallbacks = null;
	protected AlvildaChangeState onStateChangeCallbacks = null;

	#endregion


	#region MonoBehaviour 

	// Constructor
	protected void Awake()
	{
		trans = transform;
		Assert.IsNotNull(trans);
		Assert.IsNotNull(mat);
		col = mat.color;

		movement = GetComponent<AlvildaMovement>();
		Assert.IsNotNull(movement);

		curiousMarker = GetComponentInChildren<AlvildaCuriousMarker>();
		Assert.IsNotNull(curiousMarker);

		checkPointController = GetComponent<AlvildaCheckPointController>();
		Assert.IsNotNull(checkPointController);

		clickObj = gameObject.AddComponent<ClickableObject>();
		Assert.IsNotNull(clickObj);
	}

	// Use this for initialization
	protected void Start()
	{
		movement.AddReachedDestinationCallback(OnReachedDestination);
		movement.AddReachedSoundOriginCallback(OnReachedSoundOrigin);
		clickObj.AddReleasedCallback(ReceiveMouseClick);

		ChangeState(AlvildaState.Idle);

        //This is for the traincart  
        Alvilda = GameObject.Find("alvilda");
       
	}


///////////////////Methods for parenting/unparenting Alvilda to the Train cart object.

    void ChangeParent()
    {
        Alvilda.transform.parent = TrainSpace.transform;


    }

    void RevertParent()
    {
        Alvilda.transform.parent = null;


    }

 //////////////////////////////////////////////////////////////////////////////////////////
  


    // Update is called once per frame
    protected void Update()
	{
		modelTransform.LookAt(trans.position + movement.TargetDirection * 10f);
		//Debug.DrawLine(trans.position, trans.position + movement.TargetDirection, Color.blue);
		if (state == AlvildaState.Idle)
		{
			if (Time.time - timer > idleToWanderingDelay)
			{
				ChangeState(AlvildaState.Wandering);
			}
		}
		else if (state == AlvildaState.Wandering)
		{
			if (isAtWanderingTarget)
			{
				if (Time.time - timer > newWanderingTargetDelay)
				{
					UpdateWanderingTarget();
				}
			}
		}
		else if (state == AlvildaState.Curious)
		{
			if (Time.time - timer > curiousToIdleDelay)
			{
				ChangeState(AlvildaState.Idle);
			}
		}
		else if (state == AlvildaState.MovingTowardsSound)
		{
			// If the signal is dead...
			if (lastHitPoint.attractionStrength <= 0f
				|| Vector3.Distance(lastWave.origin, trans.position) < 1f)
			{
				ChangeState(AlvildaState.Idle);
			}
		}

		// Update info variables
		currentState = state;
	}

	protected void OnCollisionEnter(Collision collision)
	{
		InteractiveObject iObj = collision.GetInteractiveObj();
		if (iObj != null)
		{
			iObj.OnAlvildaInteraction();
		}

    }

    // Called when this collider/rigidbody has begun touching another rigidbody/collider.
    protected void OnCollisionStay(Collision collision)
	{
		if (collision.collider.name == "ground") return;

		if (state == AlvildaState.Wandering)
		{
			// Reached the wandering target...
			if (collision.GameObjID() == wanderingTargetObj.ID())
			{
				if (!isAtWanderingTarget)
				{
					timer = Time.time;
					isAtWanderingTarget = true;
					movement.Stop();
				}
			}

			// Check if we can still see the target...
			else
			{
				Ray ray = new Ray(trans.position, (wanderingTarget - trans.position).normalized);
				RaycastHit hit = new RaycastHit();

//				if (Physics.Raycast(ray, out hit, RAYCAST_LENGTH))
				if (Physics.SphereCast(ray, 1f, out hit, RAYCAST_LENGTH))
				{
					// If we see a new target then let's wander somwhere else...
					if (hit.collider.GameObjID() != wanderingTargetObj.ID())
					{
						UpdateWanderingTarget();
					}
				}

				// If we can't see it then find a new target...
				else
				{
					UpdateWanderingTarget();
				}
			}
		}
		else if (lastWave != null)
		{
			if (lastWave.type == WaveType.Bell && collision.collider.tag == GameConst.BONFIRE_TAG)
			{
				if (state != AlvildaState.Occupied)
				{
					ChangeState(AlvildaState.Occupied);	
				}
			}

//////////////////////////////////////// The train carts object has its own soundwave name and its own Tag
            if (lastWave.type == WaveType.Traincart && collision.collider.tag == GameConst.TRAINCART_TAG)
            {

                //Alvilda's state changes
                if (state != AlvildaState.Occupied)
                {
                    ChangeState(AlvildaState.Occupied);

                    // Find the transform of the traincart
                    TrainSpace = collision.collider.GetComponent<Transform>();
                   

                    //She is parented to the traincart 
                    ChangeParent();

                }
            }
/////////////////////////////////////////////////////////////////////////////////
          


   

            else if (collision.GameObjID() == lastWave.soundObj.ID())
			{
				ChangeState(AlvildaState.Idle);	
			}
		}
	}

	// Called when the behaviour becomes disabled () or inactive.
	protected void OnDisable()
	{
		mat.color = col;
		movement.RemoveReachedDestinationCallback(OnReachedDestination);
		movement.RemoveReachedSoundOriginCallback(OnReachedSoundOrigin);
		clickObj.RemoveReleasedCallback(ReceiveMouseClick);
	}

	#endregion


	#region Protected Functions

	protected void OnReachedDestination()
	{
		ChangeState(AlvildaState.Idle);	
	}

	protected void OnReachedSoundOrigin()
	{
		ChangeState(AlvildaState.Idle);
	}

	protected void UpdateWanderingTarget()
	{
		bool foundATarget = false;
		while (!foundATarget)
		{
			Vector3 dir = new Vector3(Random.Range(-100f, 100f), 0f, Random.Range(-100f, 100f)).normalized;
			Ray ray = new Ray(trans.position, dir);
			RaycastHit hit = new RaycastHit();
			if (Physics.Raycast(ray, out hit, RAYCAST_LENGTH))
			{
				if (Vector3.Distance(trans.position, hit.point) > minDistanceToWanderingTarget)
				{
					foundATarget = true;
					isAtWanderingTarget  = false;
					wanderingTarget = hit.point;
					wanderingTargetObj = hit.collider.gameObject;
					movement.MoveTo(wanderingTarget);
				}
			}	
		}
	}

	// Changes Alvilda's state
	// info on the default keyword: https://msdn.microsoft.com/en-us/library/xwth0h0d.aspx
	public void ChangeState(AlvildaState newState, Vector3 direction = default(Vector3))
	{
		this.Log("New state: " + newState.ToString(), DebugLogLevel.MediumDetails);

		timer = Time.time;
		state = newState;

		switch(newState)
		{
			case AlvildaState.Idle:
				movement.Stop();
				curiousMarker.TurnOff();
				mat.color = col;
				lastWave = null;
				lastHitPoint = null;
				break;

			case AlvildaState.Occupied:
				movement.Stop();
				curiousMarker.TurnOff();
				mat.color = Color.red;
				lastWave = null;
				lastHitPoint = null;
				break;

			case AlvildaState.Wandering:
				curiousMarker.TurnOff();
				UpdateWanderingTarget();
				lastWave = null;
				lastHitPoint = null;
				break;

			case AlvildaState.Curious:
				movement.Stop();
				curiousMarker.TurnOn();
				break;

			case AlvildaState.MovingTowardsSound:
				curiousMarker.TurnOff();
				movement.MoveDirection(direction);
				break;

			case AlvildaState.Dead:
				movement.Stop();
				curiousMarker.TurnOff();
				lastWave = null;
				lastHitPoint = null;
				break;

			default:
				this.LogError("NOT IMPLEMENTED FOR: " + newState.ToString(), DebugLogLevel.OnlyImportant);
				break;
		}

		// Let those who are listening know of the change...
		if (onStateChangeCallbacks != null)
		{
			onStateChangeCallbacks(state);
		}
	}

	#endregion


	#region Public Functions

	public void Respawn()
	{
		trans.position = checkPointController.LastSavedPosition;

		Renderer[] renderers = modelTransform.GetComponentsInChildren<Renderer>();
		for (int i = 0; i < renderers.Length; i++)
		{
			renderers[i].enabled = true;
		}

		ChangeState(AlvildaState.Idle);

		if (onRespawnCallbacks != null)
		{
			onRespawnCallbacks();
		}
	}

	public void Die()
	{
		Renderer[] renderers = modelTransform.GetComponentsInChildren<Renderer>();
		for (int i = 0; i < renderers.Length; i++)
		{
			renderers[i].enabled = false;	
		}

		ChangeState(AlvildaState.Dead);

		if (onDeathCallbacks != null)
		{
			onDeathCallbacks();
		}
	}

	// Called when the user clicks on the Alvilda game object
	public void ReceiveMouseClick()
	{
		switch(state)
		{
			case AlvildaState.Occupied:
				ChangeState(AlvildaState.Idle);
                
 ////////////////If Alvilda is occupied and currently parented to the traincart, she will be unparented 
                RevertParent();
 /////////////////////////////////////////////////
                break;

			default:
				this.Log("Ignoring mouse click on Alvilda because state is: " + state.ToString(), DebugLogLevel.VeryDetailed);
				break;
		}
	}

	// Called when Alvilda receives a sound signal
	public override void TakeWaveHit(SoundWaveObject wave, SoundWavePoint point)
	{
		this.Log("TakeWaveHit(" + wave.origin + ", " + -point.DirToTarget + ", " + point.attractionStrength + ")", DebugLogLevel.VeryDetailed);

		if (state == AlvildaState.Occupied || state == AlvildaState.Dead)
		{
			return;
		}

		// Stop if we've been hit by the same wave or if the new wave is weaker than the one we're chasing...
		int lastSoundObjID = -1;
		if (lastWave != null && lastHitPoint != null)
		{
			if (lastWave.ID == wave.ID)
			{
				return;
			}
			else if (lastHitPoint.attractionStrength > point.attractionStrength)
			{
				return;	
			}
			lastSoundObjID = lastWave.soundObj.ID();
		}

		lastWave = wave;
		lastHitPoint = point;
		Vector3 waveOrigin = new Vector3(wave.origin.x, trans.position.y, wave.origin.z);
		Vector3 direction = new Vector3(-lastHitPoint.DirToTarget.x, 0f, -lastHitPoint.DirToTarget.z);
		WaveType waveType = wave.type;

		if (waveType == WaveType.Bell 
			|| waveType == WaveType.Horn
			|| waveType == WaveType.Lever)
		{
			ChangeState(AlvildaState.MovingTowardsSound, direction);
		}
		else
		{
			switch(state)
			{
				case AlvildaState.Dead:
					break;

				case AlvildaState.Idle:
				case AlvildaState.Occupied:
				case AlvildaState.Wandering:
					if (Vector3.Distance(waveOrigin, Position) > 1)
					{
						ChangeState(AlvildaState.Curious);
					}
					break;

				case AlvildaState.MovingTowardsSound:
					if (Vector3.Distance(waveOrigin, Position) > 1f)
					{
						if (lastWave != null && lastWave.soundObj.ID() != lastSoundObjID)
						{
							ChangeState(AlvildaState.Curious);
						}
					}
					break;

				case AlvildaState.Curious:
					if (Vector3.Distance(waveOrigin, Position) > 1f)
					{
						ChangeState(AlvildaState.MovingTowardsSound, direction);
					}
					break;
			}
		}
	}

	public void AddStateChangeCallback(AlvildaChangeState newCallback)
	{
		onStateChangeCallbacks += newCallback;
	}

	public void RemoveStateChangeCallback(AlvildaChangeState newCallback)
	{
		onStateChangeCallbacks -= newCallback;
	}

	public void AddAlvildaDeathCallback(AlvildaDeath newCallback)
	{
		onDeathCallbacks += newCallback;
	}

	public void RemoveAlvildaDeathCallback(AlvildaDeath newCallback)
	{
		onDeathCallbacks -= newCallback;
	}

	public void AddAlvildaRespawnCallback(AlvildaRespawn newCallback)
	{
		onRespawnCallbacks += newCallback;
	}

	public void RemoveAlvildaRespawnCallback(AlvildaRespawn newCallback)
	{
		onRespawnCallbacks -= newCallback;
	}

	#endregion

	// Made this to stop Alvilda moving in certain situations.
	public void AlvildaMovementOverride()
	{
		ChangeState(AlvildaState.Idle); 
	
	}
}
