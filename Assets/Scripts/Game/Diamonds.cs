using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamonds: MonoBehaviour {

	private bool isPaused = false;


	public void SetIsPaused(bool value)
	{
		this.isPaused = value;
	}


	public void Update () {
		if (!isPaused)
		{
        transform.Rotate(new Vector3(0, 150, 0) * Time.deltaTime);
		}
	}
}
