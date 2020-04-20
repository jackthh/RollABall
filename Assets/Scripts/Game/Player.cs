using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class Player : MonoBehaviour
{

	[Header("References")]
	public GameObject gameMasterContainer;
	private GameMaster gameMaster;

	[Header("Parameters for movement")]
	private Rigidbody rb;
	private bool doJump = false;
	public bool jumpable;
	public float verticalSpeed;
	public float horizontalSpeed;
	public float jumpForceMagnitude;

	[Header("Player status and stats")]
	private int health = 3;
	private bool isPaused = false;


	void Start()
	{
		gameMaster = gameMasterContainer.GetComponent<GameMaster>();
		rb = this.GetComponent<Rigidbody>();
	}


	private void Update()
	{
		if (isPaused)
		{
			return;
		}
		else
		{
			if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
			{
				doJump = true;
			}
		}
	}


	private void FixedUpdate()
	{
		if (isPaused)
		{
			rb.velocity = Vector3.zero;
			return;
		}
		else
		{
			float verticalInput = Input.GetAxis("Vertical");
			float horizontalInput = Input.GetAxis("Horizontal");

			if (jumpable && doJump)
			{
				rb.AddForce(Vector3.up * jumpForceMagnitude, ForceMode.Impulse);
				doJump = false;
			}

			float xVelocity = horizontalInput * horizontalSpeed;
			float yVelocity = rb.velocity.y;
			float zVelocity = verticalInput * verticalSpeed;
			rb.velocity = new Vector3(xVelocity, yVelocity, zVelocity);
		}
	}


	private void OnTriggerEnter(Collider other)
	{

		if (other.CompareTag("Diamond"))
		{
			gameMaster.OnCollectPickUps(other);
		}

		if (other.CompareTag("Enemy"))
		{
			this.health--;
			Renderer renderer = this.GetComponent<Renderer>();
			StartCoroutine(Flicker(renderer, 0.8f, 0.15f));

			gameMaster.OnTouchEnemies();
		}
	}


	public void SetIsPaused(bool value)
	{
		this.isPaused = value;
	}


	private bool IsGrounded()
	{
		SphereCollider playerCollider = transform.GetComponent<SphereCollider>();
		return Physics.Raycast(playerCollider.bounds.center, Vector3.down, playerCollider.bounds.extents.y + 0.1f);
	}


	public int GetHealth()
	{
		return this.health;
	}


	public bool IsDead()
	{
		if (this.health <= 0)
		{
			return true;
		}
		else
		{
			return false;
		}
	}


	private IEnumerator Flicker(Renderer objRenderer, float duration, float interval)
	{
		float endTime = Time.time + duration;
		while (Time.time < endTime)
		{
			objRenderer.enabled = false;
			yield return new WaitForSeconds(interval);
			objRenderer.enabled = true;
			yield return new WaitForSeconds(interval);
		}
	}


	public void OnReset()
	{
		this.health = 3;
		this.transform.position = new Vector3(0, 0.6f, 0);
	}
}

