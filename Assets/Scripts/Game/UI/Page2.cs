using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Page2 : MonoBehaviour
{

	[Header("Refs")]
	private GameMaster gameMaster;
	public Text subBonusText;
	private Animator animator;
	public Button playBtn;

	[Header("Spin parameters")]
	public Transform rotor;
	[SerializeField]
	private float spinSpeed = 1000f;
	// TODO: Delete later
	private float startToEndTime = 0f;

	private bool doSpin = false;
	private float currentAngle = 0;

	[SerializeField]
	private float speedThreshold = 80f;
	[SerializeField]
	private float acceleration = -150f;

	private float finalAcceleration = 0f;

	// Parameters to pick result
	private readonly float[] results = new float[] { 1.1f, 2f, 2.5f, 1f, 4f, 3f, 1.5f };
	private int resultIndex;
	private readonly int[] normalResult = new int[] { 3, 0, 6, 1 };
	private readonly int[] rareResult = new int[] { 2, 5 };
	private readonly int[] superRareResult = new int[] { 4 };


	private void Start()
	{
		playBtn.interactable = false;
		gameMaster = GameObject.FindGameObjectWithTag(Utilities.GAME_MASTER_TAG).GetComponent<GameMaster>();
		animator = GetComponent<Animator>();
	}


	public void OnRoll()
	{
		resultIndex = PickRandomResult();
		Debug.Log("Result = " + resultIndex);
		Rotate();
		animator.SetTrigger("WheelClick");
	}


	private int PickRandomResult()
	{
		string randomType = PickRandomType();
		Debug.Log("Random Type = " + randomType);

		int index;
		int chosenResult;

		switch (randomType)
		{
			case "Common":
				index = Random.Range(0, normalResult.Length);
				chosenResult = normalResult[index];
				return chosenResult;

			case "Rare":
				index = Random.Range(0, rareResult.Length);
				chosenResult = rareResult[index];
				return chosenResult;

			default:
				return superRareResult[0];
		}
	}


	private string PickRandomType()
	{
		float temp = Random.value;
		// Probability: normal = 80%, rare = 15%, super rare = 5%

		if (temp <= 0.8f)
		{
			return "Common";
		}
		if (temp <= 0.95f)
		{
			return "Rare";
		}
		return "Super Rare";
	}


	private void Rotate()
	{
		doSpin = true;

	}


	private void Update()
	{
		if (doSpin)
		{
			// Stop the wheel
			if (Mathf.Abs(spinSpeed) <= 2)
			{
				doSpin = false;
				subBonusText.text = "x " + results[resultIndex];
				gameMaster.SetBoostMult(results[resultIndex]);
				animator.SetTrigger("WheelGotResult");
				playBtn.interactable = true;
				Debug.Log("Play Btn Interactable = " + playBtn.IsInteractable());
				Debug.Log("Duration = " + (Time.realtimeSinceStartup - startToEndTime));
			}

			rotor.Rotate(new Vector3(0, 0, spinSpeed * Time.deltaTime));

			// Needed a variable to store current angle of the wheel cause if we 
			// call transform.rotation.z later, it will just return 0 as the initalized
			// z-axis rotation value of the transform
			currentAngle += spinSpeed * Time.deltaTime;

			// Slow down angle speed according to acceleration
			spinSpeed += (acceleration * Time.deltaTime);

			// Assure we only call this once
			if (spinSpeed <= speedThreshold && finalAcceleration == 0)
			{
				startToEndTime = Time.realtimeSinceStartup;
				Debug.Log("Start Point Time = " + startToEndTime);
				Debug.Log("Spin speed = " + spinSpeed);
				CalculateFinalAcceleration();
				acceleration = finalAcceleration;
				Debug.Log("Acceleration = " + acceleration);
			}
		}
	}


	private void CalculateFinalAcceleration()
	{
		// w ^ 2 - w0 ^ 2 = 2 * a * Phi 
		// (w = 0)
		// Phi tinh bang degree, vi van toc bi anh huong boi gia toc duoi dang degree/sec

		float Phi0 = currentAngle % 360;
		Debug.Log("Phi0 = " + Phi0);
		float Phi = PickResultAngle();
		Debug.Log("Phi = " + Phi);
		float deltaPhi;

		if (Phi - Phi0 <= 0)
		{
			deltaPhi = Phi + 360 - Phi0;
		}
		else
		{
			deltaPhi = Phi - Phi0;
		}
		Debug.Log("Delta Phi = " + deltaPhi);

		this.finalAcceleration = (-Mathf.Pow(spinSpeed, 2)) / (2 * deltaPhi);
	}


	private float PickResultAngle()
	{
		float cellWidth = 360 / results.Length;
		float minAngle;
		float maxAngle;
		float resultAngle;

		minAngle = (resultIndex - 0.5f) * cellWidth;
		Debug.Log("Min Angle = " + minAngle);
		maxAngle = (resultIndex + 0.5f) * cellWidth;
		Debug.Log("Max Angle = " + maxAngle);

		resultAngle = Random.Range(minAngle, maxAngle);

		return resultAngle;
	}
}