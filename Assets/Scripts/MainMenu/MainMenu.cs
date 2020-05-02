using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour {

	[Header("Player ref")]
	public GameObject playerContainer;
	private MainMenuPlayer player;

	[Header("Transition")]
	public GameObject transitionContainer;
	private SceneTransition transitionController;
	private Animator transitionAnimator;

	[Header("Play Btn")]
	public Button playBtn;
	public AnimationCurve scaleCurve;

	private float curveTime = 0f;

	private bool initRun = true;


	private void Start()
	{
		transitionAnimator = this.GetComponent<Animator>();
	}


	private void OnEnable()
	{
		// Needed to get references now cause OnEnable() is called before Start()
		transitionController = transitionContainer.GetComponent<SceneTransition>();
		player = playerContainer.GetComponent<MainMenuPlayer>();

		// These functions will be called everytime MainMenu is activated
		StartCoroutine(ShowMainMenu());
		player.Reset();

	}


	IEnumerator ShowMainMenu()
	{
		// Make sure we dont have to wait for "2s" if it's not the game start
		if (this.initRun)
		{
			// Wait for the scene transition
			yield return new WaitForSecondsRealtime(transitionController.GetTransitionTime());
			this.initRun = false;
		}

		transitionAnimator.SetTrigger("ShowMainMenu");
	}


	private void Update()
	{
		// Animate Play Btn
		if (curveTime <= 1.6f)
		{
			curveTime += Time.deltaTime;
		}
		else
		{
			curveTime = 0;
		}

		float scale = scaleCurve.Evaluate(curveTime);
		playBtn.transform.localScale = new Vector3(scale, scale, scale);
	}


	public void QuitApp()
    {
        Debug.Log("Quitted!");
        Application.Quit();
    }
}
