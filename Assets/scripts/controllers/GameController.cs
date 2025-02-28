using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : MonoBehaviour
{
	#region Variables 

	// Unity Editor Settings
	[SerializeField] protected bool debugMode;
	[SerializeField] protected DebugLogLevel debugLevel;
	[SerializeField] protected bool debugDraw;

	[SerializeField] protected string[] scenes;

	// Private Static Variables
	private static GameController instance = null;
	protected static int sceneIndex = 0;

	// Public Properties
	public static bool InDebugMode { get { return (instance) ? instance.debugMode : false; } }
	public static bool InDebugDrawMode { get { return (instance) ? instance.debugDraw : false; } }
	public static DebugLogLevel DebugLevel { get { return (instance) ? instance.debugLevel : DebugLogLevel.Off; } }


	#endregion


	#region MonoBehaviour

	// Constructor
	protected void Awake()
	{
		instance = this;
		Assert.IsNotNull(instance);

		string activeSceneName = SceneManager.GetActiveScene().name;
		for (int i = 0; i < scenes.Length; i++)
		{
			if (scenes[i] == activeSceneName)
			{
				sceneIndex = i;
				break;
			}
		}
	}

	protected void OnDisable()
	{
		instance = null;
	}

	protected void NextScene()
	{
		sceneIndex = (sceneIndex + 1) % scenes.Length;
		SceneManager.LoadScene(scenes[sceneIndex]);
	}

	protected void PrevScene()
	{
		sceneIndex = ((sceneIndex - 1) < 0) ? (scenes.Length - 1) : (sceneIndex - 1);
		SceneManager.LoadScene(scenes[sceneIndex]);
	}

	#endregion


	#region Public Functions

	public static void LoadNextScene()
	{
		instance.NextScene();
	}

	public static void LoadPreviousScene()
	{
		instance.PrevScene();
	}

	#endregion
}
