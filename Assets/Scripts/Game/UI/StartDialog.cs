using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDialog : MonoBehaviour
{
	[Header("Refs")]
	public GameObject gameMasterContainer;
	private GameMaster gameMaster;
	private Animator animator;


    void Start()
    {
		gameMaster = gameMasterContainer.GetComponent<GameMaster>();
		animator = GetComponent<Animator>();        
    }

	
	public void OnPage1Next()
	{
		animator.SetTrigger("Page1Next");
	}


	public void OnPage2Play()
	{
		animator.SetTrigger("Page2Play");
	}


	IEnumerator TriggerPage2PlayClick()
	{
		// 1.5s = duration of clip "StartDialog_OnPage2Play"
		yield return new WaitForSecondsRealtime(1.5f);
		gameMaster.OnPage2PlayClick();
	}
}
