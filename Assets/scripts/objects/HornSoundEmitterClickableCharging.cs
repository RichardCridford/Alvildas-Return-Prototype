using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

public class HornSoundEmitterClickableCharging : HornSoundEmitterClickable
{
	#region Variables 

	// Unity Editor Variables
	[SerializeField] protected ChargingParams chargeParams;

	// Protected Instance Variables
	protected Color startColor = Color.white;
	protected Material mat = null;
	protected Renderer rend = null;
	protected ChargingObject chargingObject = null;

	#endregion


	#region MonoBehaviour 

	// Constructor
	protected override void Awake()
	{
		Renderer rend = GetComponent<Renderer>();
		Assert.IsNotNull(rend);

		mat = rend.material;
		Assert.IsNotNull(mat);

		chargingObject = gameObject.AddComponent<ChargingObject>();
		Assert.IsNotNull(chargingObject);

		base.Awake();
	}

	//
	protected override void Start()
	{
		startColor = mat.color;

		chargingObject.AddChargeCompleteCallback(OnChargeComplete);

		base.Start();
	}

	// Update is called once per frame
	protected void Update()
	{
		if (chargingObject.IsCharging)
		{
			mat.color = startColor + startColor * chargingObject.ChargeStatus;
		}
	}

	// Called when the behaviour becomes disabled () or inactive.
	protected override void OnDisable()
	{
		chargingObject.RemoveChargeCompleteCallback(OnChargeComplete);
		base.OnDisable();
	}

	#endregion


	#region Protected Functions 

	protected void OnChargeComplete()
	{
		mat.color = startColor;
		TryToEmit();
	}

	protected override void OnClicked()
	{
		chargingObject.StartCharging(chargeParams);
	}

	#endregion
}
