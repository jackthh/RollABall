using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{

	private Animator animator;

	public Text scoreEffText; 


	private void Start()
	{
		animator = GetComponent<Animator>();
	}


	public void OnGameStart()
	{
		animator.SetTrigger("GameStart");
	}


	public void OnGetScore(float recentScore)
	{
		scoreEffText.text = "+ " + recentScore.ToString();
		animator.SetTrigger("GetScore");
	}


	public void OnGameEnd()
	{
		animator.SetTrigger("GameEnd");
	}
}
