using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDialog : MonoBehaviour
{
	[Header("Refs")]
	public GameObject gameMasterContainer;
	private GameMaster gameMaster;
	public GameObject videoPlayer1Container;
	public GameObject videoPlayer2Container;
	private Animator animator;


    void Start()
    {
		gameMaster = gameMasterContainer.GetComponent<GameMaster>();
		animator = GetComponent<Animator>();        
    }

	
	public void OnPage1Next()
	{
		animator.SetTrigger("Page1Next");
		videoPlayer1Container.GetComponentInChildren<UnityEngine.Video.VideoPlayer>().Stop();
		videoPlayer2Container.GetComponentInChildren<UnityEngine.Video.VideoPlayer>().Stop();
	}


	public void OnPage2Play()
	{
		animator.SetTrigger("Page2Play");
		StartCoroutine(TriggerPage2PlayClick());
	}


	IEnumerator TriggerPage2PlayClick()
	{
		// 1.5s = duration of clip "StartDialog_OnPage2Play" + 1s of buffer time 
		yield return new WaitForSecondsRealtime(2.5f);
		gameMaster.OnPage2PlayClick();
	}


	public void OnMainMenuClick()
	{
		gameMaster.OnMainMenuClick();
	}
}
