using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{

	private float transitionTime = 2f;
	private Animator animator;


	public float GetTransitionTime()
	{
		return transitionTime;
	}


	private void Start()
	{
		animator = GetComponent<Animator>();
	}


	public void OnTransition()
	{
		animator.SetTrigger("NextLvl");
	} 
}
