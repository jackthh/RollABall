using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LvlSelectionController : MonoBehaviour {

    public GameObject levelButtons;
    private Button[] levelButtonsList;
    // Use this for initialization
    void Start()
    {
        int levelReached = PlayerPrefs.GetInt("levelReached", 1);
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
