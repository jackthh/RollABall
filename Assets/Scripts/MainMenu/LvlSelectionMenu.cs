using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LvlSelectionMenu : MonoBehaviour {

	public GameObject transitionContainer;
	private SceneTransition transitionController;

    public GameObject levelButtons;
    private Button[] levelButtonsList;


    void Start()
    {
		transitionController = transitionContainer.GetComponent<SceneTransition>();

        int levelReached = PlayerPrefs.GetInt(Utilities.LVL_REACHED_TAG, 1);
        levelButtonsList = levelButtons.GetComponentsInChildren<Button>();
        for (int i = 0; i < levelButtonsList.Length; i++)
        {
            if (i + 1 > levelReached)
            {
                levelButtonsList[i].interactable = false;
            }
        }

    }


    public void LoadLvl(int level)
    {
        StartCoroutine(LoadAsyncScene(level.ToString()));
    }


    IEnumerator LoadAsyncScene(string level)
    {
		PlayerPrefs.SetInt(Utilities.SHOW_TUT_TAG, 1);
		transitionController.OnTransition();
		yield return new WaitForSecondsRealtime(transitionController.GetTransitionTime());

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Lvl" + level);
        while(!asyncOperation.isDone)
        {
            yield return null;
        }
    }


    public void QuitApp()
    {
        Debug.Log("Quitted!");
        Application.Quit();
    }

}
