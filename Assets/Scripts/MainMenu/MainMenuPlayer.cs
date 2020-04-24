using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPlayer : MonoBehaviour
{
	[Header("References")]
	public Transform diamond;
	public Transform startPoint;
	public Transform endPoint;

	[Header("Stats")]
	public float jumpForceMagnitude = 30f;
	public AnimationCurve cringingSpeedCurve;
	private float playerSpeed = 40f;

	private Rigidbody rb;

	private bool moving = true;
	private bool jumping = false;

	private bool checkingNeeded = true;
	private float cringingTime = 0f;


	void Start()
    {
		rb = this.GetComponent<Rigidbody>();
	}


	private void Update()
	{
		if (checkingNeeded && Mathf.Abs(transform.position.x - diamond.position.x) <= 0.2f)
		{
			checkingNeeded = false;
			moving = false;
			StartCoroutine(CringingMove());
		}

		if (!moving && !jumping)
		{
			cringingTime += Time.deltaTime;
		}

		if (Vector3.Distance(endPoint.position, transform.position) <= 0.5f)
		{
			transform.position = startPoint.position;
		}
	}


	private void FixedUpdate()
	{
		if (moving)
		{
			rb.velocity = new Vector3(40, 0, 0) * playerSpeed * Time.deltaTime;
		}
		else
		{
			if (!jumping)
			{
				rb.velocity = new Vector3(40, 0, 0) * playerSpeed * cringingSpeedCurve.Evaluate(cringingTime) * Time.deltaTime;
			}
		}
	}


	IEnumerator CringingMove()
	{
		yield return new WaitForSeconds(2.2f);
		jumping = true;
		cringingTime = 0f;

		StartCoroutine(Jump());
	}


	IEnumerator Jump()
	{
		rb.velocity = Vector3.zero;
		rb.AddForce(Vector3.up * jumpForceMagnitude, ForceMode.Impulse);

		yield return new WaitForSeconds(4f);
		jumping = false;
		moving = true;

		yield return new WaitForSeconds(1f);
		checkingNeeded = true;
	}


	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Diamond"))
		{
			StartCoroutine(hideTemporarily(other));
		}
	}


	IEnumerator hideTemporarily(Collider _collider)
	{
		_collider.transform.localScale = Vector3.zero;
		yield return new WaitForSeconds(9f);
		_collider.transform.localScale = new Vector3(5, 5, 5);
	}
}
