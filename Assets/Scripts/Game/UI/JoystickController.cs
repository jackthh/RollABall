using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickController : MonoBehaviour
{

	private Animator animator;


	private void Start()
	{
		animator = GetComponent<Animator>();
	}


	public void OnGameStart()
	{
		animator.SetTrigger("GameStart");
	}


	public void OnGameEnd()
	{
		animator.SetTrigger("GameEnd");
	}
}
