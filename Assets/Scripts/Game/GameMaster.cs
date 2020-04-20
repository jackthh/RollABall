using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameMaster : MonoBehaviour
{
	[Header("References")]
	public GameObject uIControllerContainer;
	private UI uIController;

	public GameObject playerContainer;
	private Player playerController;

	public GameObject diamondsContainer;
	private DiamondsController diamondsController;

	[Space]
	public GameObject movingEnemiesContainer;
	private MovingEnemies movingEnemiesController;

	[Header("Statistics")]
	public AnimationCurve scoreCurve;
	private float scoreTime;
	private int totalCoinsCount = 0;
	private bool isRecording = false;

	private int lvlReached;
	private int totalScore;
	private int currentScore = 0;
	private int totalCoins;
	private int currentCoins = 0;
	private float totalTime;
	private float currentTime = 0f;
	private int totalDeaths;
	private int currentDeaths = 0;


	private void Start()
	{
		uIController = uIControllerContainer.GetComponent<UI>();
		playerController = playerContainer.GetComponent<Player>();
		diamondsController = diamondsContainer.GetComponent<DiamondsController>();
		if (movingEnemiesContainer != null)
		{
			movingEnemiesController = movingEnemiesContainer.GetComponent<MovingEnemies>();
		}

		lvlReached = PlayerPrefs.GetInt("lvlReached", 1);
		totalScore = PlayerPrefs.GetInt("totalScore", 0);
		totalCoins = PlayerPrefs.GetInt("totalCoins", 0);
		totalTime = PlayerPrefs.GetFloat("totalTime", 0f);
		totalDeaths = PlayerPrefs.GetInt("totalDeaths", 0);

		totalCoinsCount = diamondsController.TotalCoinsCount();

		Reset();
		StartGame();
	}


	public void StartGame()
	{
		scoreTime = 0f;
		PauseGame(false);
	}


	public void PauseGame(bool isPaused)
	{
		isRecording = !isPaused;

		playerController.SetIsPaused(isPaused);
		diamondsController.SetIsPaused(isPaused);
		if (movingEnemiesController != null)
		{
			movingEnemiesController.SetIsPaused(isPaused);
		}
	}


	private void Update()
	{
		if (isRecording)
		{
		currentTime += Time.deltaTime;
		scoreTime += Time.deltaTime;
		}
	}


	public void OnCollectPickUps(Collider _collider)
	{
		diamondsController.OnTouchPlayer(_collider);

		int score = CalculateScore();
		currentScore += score;
		currentCoins++;

		uIController.UpdateScore(this.currentScore);
		uIController.UpdateCoins(this.currentCoins);

		if (this.currentCoins == this.totalCoinsCount)
		{
			PauseGame(true);
			uIController.OnEndGame(true);
		}
	}


	public void OnTouchEnemies()
	{
		uIController.UpdateHealth(playerController.GetHealth());

		if (playerController.IsDead())
		{
			currentDeaths++;
			PauseGame(true);
			uIController.OnEndGame(false);
		}
	}


	private int CalculateScore()
	{
		Debug.Log("Score time = " + scoreTime);
		int score = (int) Mathf.Round(scoreCurve.Evaluate(scoreTime));
		Debug.Log("Curve cal = " + scoreCurve.Evaluate(scoreTime));
		scoreTime = 0f;
		return score;
	}


	public void LoadNextLvl()
	{
		Debug.Log("Running loading level");
		string currentSceneName = SceneManager.GetActiveScene().name;
		int currentLvl = int.Parse(currentSceneName.Substring(3));
		int nextLvl = ++currentLvl;
		SaveGame(nextLvl);
		StartCoroutine(LoadLvl(nextLvl));
	}


	private IEnumerator LoadLvl(int nextLvl)
	{
		AsyncOperation async;
		if (nextLvl > 5) // Only 5/16 scenes exist
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


	public void Reset()
	{
		uIController.SetCoinsCount(totalCoinsCount);
		uIController.OnReset();
		playerController.OnReset();
		diamondsController.OnReset();

		StartGame();
	}


	public void SaveGame(int nextLvl)
	{
		if (nextLvl > lvlReached)
		{
			PlayerPrefs.SetInt("levelReached", nextLvl);
		}
		PlayerPrefs.SetInt("totalScore", totalScore + currentScore);
		PlayerPrefs.SetInt("lastLvlScore", currentScore);
		PlayerPrefs.SetInt("totalCoins", totalCoins + currentCoins);
		PlayerPrefs.SetInt("lastLvlCoins", currentCoins);
		PlayerPrefs.SetFloat("totalTime", totalTime + currentTime);
		PlayerPrefs.SetFloat("lastLvlTime", currentTime);
		PlayerPrefs.SetInt("totalDeaths", totalDeaths + currentDeaths);
		PlayerPrefs.SetInt("lastLvlDeaths", currentDeaths);
	}
}
