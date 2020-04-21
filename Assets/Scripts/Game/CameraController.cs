using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

	[Header("References")]
	public Transform player;

	[Header("Position parameters")]
	//public float movingSpeed = 10f;
	public Transform cameraAnchor;

	[SerializeField]
	private Transform[] routePoints;

	[SerializeField]
	[Range(0f, 1f)]
	private float lerpIndex = 0.5f;

	[Space]
	public Vector3 tempPos;
	private Vector3 gizmosPos;


	private void OnDrawGizmos()
	{
		for (float t = 0; t <= 1; t += 0.02f)
		{
			gizmosPos = CubicCurve(routePoints, t);

			// Draw curve
			Gizmos.DrawSphere(gizmosPos, 0.2f);
		}

		// Lines from anchor points to control points
		// https://youtu.be/RF04Fi9OCPc

		Gizmos.DrawLine(routePoints[0].position, routePoints[1].position);
		Gizmos.DrawLine(routePoints[2].position, routePoints[3].position);
	}


	private void Start()
	{
		tempPos = CubicCurve(routePoints, lerpIndex);
	}


	private void Update()
	{
		// Make routePoints to follow player
		//cameraAnchor.Translate((player.position - transform.position).normalized * movingSpeed * Time.deltaTime, Space.World);
		cameraAnchor.position = player.position;

		// Check input to move camenra
		if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K))
		{
			if (Input.GetKeyDown(KeyCode.J))
			{
				lerpIndex += 0.05f;
			}
			else
			{
				lerpIndex -= 0.05f;
			}

			lerpIndex = Mathf.Clamp01(lerpIndex);
		}

			tempPos = CubicCurve(routePoints, lerpIndex);
	}


	private void LateUpdate()
	{
		// MOVE camera
		//Vector3 step = (tempPos - transform.position).normalized * movingSpeed * Time.deltaTime;

		//// Needed to check step width in order to prevent vibration 
		//if (Vector3.Distance(transform.position, tempPos) > step.magnitude)
		//{
		//	transform.Translate(step, Space.World);
		//}
		transform.position = tempPos;

		// ROTATE camera
		Vector3 dir = player.position - this.transform.position;
		Quaternion rotation = Quaternion.LookRotation(dir, Vector3.up);
		this.transform.rotation = rotation;
	}


	private Vector3 CubicCurve(Transform[] points, float _lerpIndex)
	{
		Vector3 lerpPos;

		// Cubic Bezier Curve
		// https://en.wikipedia.org/wiki/B%C3%A9zier_curve#Quadratic_B%C3%A9zier_curves

		lerpPos = Mathf.Pow(1 - _lerpIndex, 3) * points[0].position
				+ 3 * Mathf.Pow(1 - _lerpIndex, 2) * _lerpIndex * points[1].position
				+ 3 * Mathf.Pow(1 - _lerpIndex, 1) * Mathf.Pow(_lerpIndex, 2) * points[2].position
				+ Mathf.Pow(_lerpIndex, 3) * points[3].position;

		return lerpPos;
	}
}
