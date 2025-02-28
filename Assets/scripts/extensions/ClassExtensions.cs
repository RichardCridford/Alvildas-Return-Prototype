using UnityEngine;
using System.Collections;

public static class ClassExtensions
{
	public static Color RGBA(this WaveColor color)
	{
		Color c = Color.white;
		switch(color)
		{
			case WaveColor.White: c = Color.white * 5f; break;
			case WaveColor.Black: c = Color.black; break;
			case WaveColor.Blue:  c = Color.blue; break;
			case WaveColor.Cyan:  c = Color.cyan; break;
			case WaveColor.Grey:  c = Color.grey; break;
			case WaveColor.Green: c = Color.green; break;
			case WaveColor.Magenta: c = Color.magenta; break;
			case WaveColor.Red:   c = Color.red; break;
			case WaveColor.Yellow:c = Color.yellow; break;
			default:              c = Color.white; break;
		}

		return c;
	}

	#region GameObject Extensions

	public static int ID(this GameObject thisGameObject)
	{
		return thisGameObject.GetInstanceID();
	}


	public static bool Click(this GameObject thisGameObject, Vector3 worldPos)
	{
		ClickableObject co = thisGameObject.GetComponent<ClickableObject>();
		if (co)
		{
			co.OnClicked(worldPos);
			return true;
		}

		return false;
	}

	public static bool Drag(this GameObject thisGameObject, Vector3 worldPos)
	{
		DraggableObject co = thisGameObject.GetComponent<DraggableObject>();
		if (co)
		{
			co.OnDragged(worldPos);
			return true;
		}

		return false;
	}

	public static bool Release(this GameObject thisGameObject, Vector3 worldPos)
	{
		ClickableObject co = thisGameObject.GetComponent<ClickableObject>();
		if (co)
		{
			co.OnReleased(worldPos);
			return true;
		}

		return false;
	}

	#endregion

	#region Collision Extensions

	public static int GameObjID(this Collision thisCollision)
	{
		return thisCollision.gameObject.GetInstanceID();
	}

	public static InteractiveObject GetInteractiveObj(this Collision thisCollision)
	{
		return thisCollision.gameObject.GetComponent<InteractiveObject>();
	}

	#endregion

	#region Collider Extensions

	public static int ID(this Collider thisCollider)
	{
		return thisCollider.GetInstanceID();
	}

	public static int GameObjID(this Collider thisCollider)
	{
		return thisCollider.gameObject.GetInstanceID();
	}

	public static bool IsAlvilda(this Collider thisCollider)
	{
		return thisCollider.tag == GameConst.ALVILDA_TAG;
	}

	#endregion


	#region MonoBehaviour Extensions

	public static void Log(this MonoBehaviour thisMonoBehaviour, string logText, DebugLogLevel logLevel)
	{
		if (GameController.InDebugMode && (int) logLevel >= (int) GameController.DebugLevel)
		{
			UnityEngine.Debug.Log("\"" + thisMonoBehaviour.name + "\": " + logText + "\n" + thisMonoBehaviour.GetType() + "", thisMonoBehaviour);
		}
	}

	public static void LogWarning(this MonoBehaviour thisMonoBehaviour, string warningText, DebugLogLevel logLevel)
	{
		if (GameController.InDebugMode && (int) logLevel >= (int) GameController.DebugLevel)
		{
			UnityEngine.Debug.LogWarning("\"" + thisMonoBehaviour.name + "\": " + "Warning: " + warningText + "\n" + thisMonoBehaviour.GetType() + "", thisMonoBehaviour);
		}
	}

	public static void LogError(this MonoBehaviour thisMonoBehaviour, string errorText, DebugLogLevel logLevel)
	{
		if (GameController.InDebugMode && (int) logLevel >= (int) GameController.DebugLevel)
		{
			UnityEngine.Debug.LogError("\"" + thisMonoBehaviour.name + "\": " + "Error: " + errorText + "\n" + thisMonoBehaviour.GetType() + "", thisMonoBehaviour);
		}
	}

	#endregion


	#region Vector3 Extensions

	// Takes two 3D vectors and checks if they are in the same position
	// Calculating the squared magnitude instead of using the magnitude property is much faster 
	// http://docs.unity3d.com/ScriptReference/Vector3-sqrMagnitude.html
	public static bool InSamePosition(this Vector3 thisVector, Vector3 otherVector, float threshold, bool useSquareMagnitude)
	{
		if (useSquareMagnitude)
		{
			return ((thisVector - otherVector).sqrMagnitude < Mathf.Abs(threshold));
		}
		else
		{
			return ((thisVector - otherVector).magnitude < Mathf.Abs(threshold));
		}
	}

	#endregion
}
