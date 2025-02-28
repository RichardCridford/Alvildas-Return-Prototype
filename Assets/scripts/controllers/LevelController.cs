using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

public class LevelController : MonoBehaviour
{
	#region Variables 

	// Unity Editor Variables
	[Header("Alvilda")]
	[SerializeField] protected float respawnDelay;

	[Header("GUI Buttons")]
	[SerializeField] protected float width = 120;
	[SerializeField] protected float height = 60;

	// Protected Instance Variables
	protected bool isWaitingToRespawn = false;

	// Private Static Variables
	private static LevelController instance = null;

	// Public Properties
	public static InputController UserInput { get; protected set; }
	public static SoundWaveController SoundWave { get; protected set; }
	public static AlvildaController Alvilda { get; protected set; }
	public static LevelController Level { get; protected set; }


	#endregion


	#region MonoBehaviour

	// Constructor
	protected void Awake()
	{
		instance = this;
		Assert.IsNotNull(instance);

		UserInput = FindObjectOfType<InputController>();
		Assert.IsNotNull(UserInput);

		SoundWave = FindObjectOfType<SoundWaveController>();
		Assert.IsNotNull(SoundWave);

		Alvilda = FindObjectOfType<AlvildaController>();
		Assert.IsNotNull(Alvilda);

		Level = FindObjectOfType<LevelController>();
		Assert.IsNotNull(Level);
	}

	// Use this for initialization
	protected void Start()
	{
		Alvilda.AddAlvildaDeathCallback(OnAlvildaDeath);
	}

	protected void OnGUI()
	{
		
		GUILayout.BeginArea(new Rect(0, 0, width, height));
		if (GUILayout.Button("Previous Level", GUILayout.Width(width), GUILayout.Height(height)))
		{
			GameController.LoadNextScene();
		}
		GUILayout.EndArea();

		GUILayout.BeginArea(new Rect(Screen.width - width - 10, 0, width, height));
		if (GUILayout.Button("Next Level", GUILayout.Width(width), GUILayout.Height(height)))
		{
			GameController.LoadPreviousScene();
		}
		GUILayout.EndArea();
	}

	protected void OnDisable()
	{
		instance = null;
		Alvilda.RemoveAlvildaDeathCallback(OnAlvildaDeath);
	}

	#endregion


	#region Protected Functions

	protected void OnAlvildaDeath()
	{
		StartCoroutine(RespawnAlvildaRoutine());
	}

	protected IEnumerator RespawnAlvildaRoutine()
	{
		isWaitingToRespawn = true;

		yield return new WaitForSeconds(respawnDelay);

		Alvilda.Respawn();
		isWaitingToRespawn = false;
	}

	#endregion
}
