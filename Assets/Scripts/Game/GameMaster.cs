using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{

	private void OnEnable()
	{
		EventsManager.OnLoadNewLvl += LoadNextLvl;

	}

	
	private void LoadNextLvl()
	{
		Debug.Log("Chay vao phan load lvl");
		string currentSceneName = SceneManager.GetActiveScene().name;
		int currentLvl = int.Parse(currentSceneName.Substring(3));
		int nextLvl = ++currentLvl;
		PlayerPrefs.SetInt("levelReached", nextLvl);
		StartCoroutine(LoadLvl(nextLvl));
	}


	private IEnumerator LoadLvl(int nextLvl)
	{
		AsyncOperation async;
		if (nextLvl > 5)
		{
			async = SceneManager.LoadSceneAsync("MainMenu");
		}
		else
		{
			async = SceneManager.LoadSceneAsync("Lvl" + nextLvl);
			Debug.Log("Lvl moi = " + nextLvl);
		}
		while (!async.isDone)
		{
			yield return null;
		}
	}


	//private IEnumerator LoadBack()
	//{
	//    AsyncOperation async = SceneManager.LoadSceneAsync("MainMenu");
	//    while (!async.isDone)
	//    {
	//        yield return null;
	//    }
	//}


	private void OnDisable()
	{
        EventsManager.OnLoadNewLvl -= LoadNextLvl;

	}

}
