using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour {

	[Header("Play Btn")]
	public Button playBtn;
	public AnimationCurve scaleCurve;

	[Header("Transition")]
	public GameObject transitionContainer;
	private Animator transitionAnimator;
	private SceneTransition transitionController;

	private float curveTime = 0f;


	private void Start()
	{
		transitionAnimator = this.GetComponent<Animator>();
		transitionController = transitionContainer.GetComponent<SceneTransition>();
		StartCoroutine(ShowMainMenu());
		
	}


	IEnumerator ShowMainMenu()
	{
		yield return new WaitForSecondsRealtime(transitionController.GetTransitionTime());
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
