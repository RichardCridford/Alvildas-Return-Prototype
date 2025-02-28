using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

public class AlvildaMovement : MonoBehaviour
{
	#region Variables 

	// Unity Editor Variables
	[SerializeField] protected float defaultSpeed;
	[SerializeField] protected float radius;

	// Protected Enums
	public enum MovementState
	{
		Stopped,
		MovingInDirection,
		MovingToPosition
	}

	// Public Properties
	public MovementState State { get { return state; } }
	public Vector3 TargetPosition { get { return targetPosition; } }
	public Vector3 TargetDirection { get { return targetDirection; } }

	// Protected Instance Variables
	protected float speed = 1f;
	protected Transform trans = null;
	protected Rigidbody rBody = null;
	protected Vector3 targetPosition = Vector3.zero;
	protected Vector3 targetDirection = Vector3.zero;
	protected MovementState state = MovementState.Stopped;

	// Delegates
	public delegate void ReachedDestination();
	public delegate void ReachedSoundOrigin();

	// Callbacks
	protected ReachedDestination onReachedDestinationCallbacks = null;
	protected ReachedSoundOrigin onReachedSoundOriginCallbacks = null;

	#endregion


	#region MonoBehaviour 

	// Constructor
	protected void Awake()
	{
		trans = transform;
		Assert.IsNotNull(trans);

		rBody = GetComponent<Rigidbody>();
		Assert.IsNotNull(rBody);
	}

	// Use this for initialization
	protected void Start()
	{
		speed = defaultSpeed;
	}

	// FixedUpdate is called every fixed framerate frame
	protected void FixedUpdate()
	{
		rBody.velocity = Vector3.zero;

		if (state == MovementState.MovingInDirection)
		{
			Vector3 newPos = rBody.position + targetDirection * Time.fixedDeltaTime * speed;
			rBody.MovePosition(newPos);

			Debug.DrawLine(trans.position, trans.position + targetDirection * 2.5f, Color.red);
		}
		else if (state == MovementState.MovingToPosition)
		{
			float lastDist = Vector3.Distance(rBody.position, targetPosition);
			Vector3 newPos = rBody.position + (targetPosition - rBody.position).normalized * Time.fixedDeltaTime * speed;
			float newDist = Vector3.Distance(newPos, targetPosition);

			// If we've gone past the target...
			if (newDist > lastDist)
			{
				state = MovementState.Stopped;

				if (onReachedDestinationCallbacks != null)
				{
					onReachedDestinationCallbacks();	
				}
			}
			else
			{
				rBody.MovePosition(newPos);
				Debug.DrawLine(trans.position, targetPosition, Color.red);
			}
		}
	}

	#endregion


	#region Public Functions 

	public void SetSpeed(float newSpeed)
	{
		speed = newSpeed;
	}

	public void MoveTo(Vector3 worldPos)
	{
		state = MovementState.MovingToPosition;

		targetPosition = worldPos;
		targetDirection = (targetPosition - trans.position).normalized;
	}

	public void MoveDirection(Vector3 direction)
	{
		state = MovementState.MovingInDirection;
		targetDirection = direction;
	}

	public void Stop()
	{
		state = MovementState.Stopped;
	}

	public void AddReachedDestinationCallback(ReachedDestination newCallback)
	{
		onReachedDestinationCallbacks += newCallback;
	}

	public void RemoveReachedDestinationCallback(ReachedDestination newCallback)
	{
		onReachedDestinationCallbacks -= newCallback;
	}

	public void AddReachedSoundOriginCallback(ReachedSoundOrigin newCallback)
	{
		onReachedSoundOriginCallbacks += newCallback;
	}

	public void RemoveReachedSoundOriginCallback(ReachedSoundOrigin newCallback)
	{
		onReachedSoundOriginCallbacks -= newCallback;
	}

	#endregion
}
