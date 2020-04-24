using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour {

	public Button playBtn;
	public AnimationCurve scaleCurve;

	private float curveTime = 0f;


	private void Update()
	{
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
