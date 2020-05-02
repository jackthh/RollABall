using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameMaster : MonoBehaviour
{
	[Header("UI Refs")]
	public GameObject uIControllerContainer;
	private UI uIController;

	public GameObject startDialogContainer;
	private Animator startDialogAnimator;

	[Header("General Refs")]
	public GameObject transitionContainer;
	private SceneTransition transitionController;

	[Header("Game Objects Refs")]
	public GameObject playerContainer;
	private Player playerController;

	public GameObject diamondsContainer;
	private DiamondsController diamondsController;

	[Space]
	public GameObject movingEnemiesContainer;
	private MovingEnemies movingEnemiesController;

	[Header("Statistics Parameters")]
	public AnimationCurve scoreCurve;
	private float scoreTime;
	private int totalCoinsCount;
	private bool isRecording = false;

	private int lvlReached;
	private int totalRecordedScore;
	private int currentScore = 0;
	private int totalRecordedCoins;
	private int currentCoins = 0;
	private float totalRecordedTime;
	private float currentTime = 0f;
	private int totalRecordedDeaths;
	private int currentDeaths = 0;

	private float boostMult = 1f;


	public void SetBoostMult(float _boostMult)
	{
		this.boostMult = _boostMult;
	}


	private void Start()
	{
		// Get references
		uIController = uIControllerContainer.GetComponent<UI>();
		startDialogAnimator = startDialogContainer.GetComponent<Animator>();
		playerController = playerContainer.GetComponent<Player>();
		diamondsController = diamondsContainer.GetComponent<DiamondsController>();
		transitionController = transitionContainer.GetComponent<SceneTransition>();

		if (movingEnemiesContainer != null)
		{
			movingEnemiesController = movingEnemiesContainer.GetComponent<MovingEnemies>();
		}

		lvlReached = PlayerPrefs.GetInt("lvlReached", 1);
		totalRecordedScore = PlayerPrefs.GetInt("totalScore", 0);
		totalRecordedCoins = PlayerPrefs.GetInt("totalCoins", 0);
		totalRecordedTime = PlayerPrefs.GetFloat("totalTime", 0f);
		totalRecordedDeaths = PlayerPrefs.GetInt("totalDeaths", 0);

		// Init coins value
		totalCoinsCount = diamondsController.TotalCoinsCount();
		uIController.SetCoinsCount(totalCoinsCount);
		Debug.Log("master takes coins count = " + totalCoinsCount);

		// Setup environment
		Reset();
		PauseGame(true);

		// Show pop up UI
		StartCoroutine(ShowStartDialog());
	}


	IEnumerator ShowStartDialog()
	{
		yield return new WaitForSecondsRealtime(transitionController.GetTransitionTime());

		if (PlayerPrefs.GetInt(Utilities.SHOW_TUT_TAG, 1) == 1)
		{
			startDialogAnimator.SetTrigger("Start_ShowTut");
		}
		else
		{
			startDialogAnimator.SetTrigger("Start_NotShowTut");
		}
	}


	public void StartGame()
	{
		scoreTime = 0f;
		PauseGame(false);
	}


	public void OnPage2PlayClick()
	{
		StartGame();
	}


	public void OnMainMenuClick()
	{
		StartCoroutine(LoadLvl(0));
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


	private IEnumerator LoadLvl(int lvlIndex)
	{
		transitionController.OnTransition();
		yield return new WaitForSecondsRealtime(transitionController.GetTransitionTime());

		AsyncOperation async;
		if (lvlIndex > 5 || lvlIndex == 0) // Only 5/16 scenes already exist
		{
			async = SceneManager.LoadSceneAsync("MainMenu");
		}
		else
		{
			PlayerPrefs.SetInt(Utilities.SHOW_TUT_TAG, 0);
			async = SceneManager.LoadSceneAsync("Lvl" + lvlIndex);
			Debug.Log("Lvl moi = " + lvlIndex);
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
	}


	public void SaveGame(int nextLvl)
	{
		if (nextLvl > lvlReached)
		{
			PlayerPrefs.SetInt(Utilities.LVL_REACHED_TAG, nextLvl);
		}

		PlayerPrefs.SetInt(Utilities.TOTAL_SCORE_TAG, totalRecordedScore + currentScore);
		PlayerPrefs.SetInt(Utilities.LAST_LVL_SCORE_TAG, currentScore);

		PlayerPrefs.SetInt(Utilities.TOTAL_COINS_TAG, totalRecordedCoins + currentCoins);
		PlayerPrefs.SetInt(Utilities.LAST_LVL_COINS_TAG, currentCoins);

		PlayerPrefs.SetFloat(Utilities.TOTAL_TIME_TAG, totalRecordedTime + currentTime);
		PlayerPrefs.SetFloat(Utilities.LAST_LVL_TIME_TAG, currentTime);

		PlayerPrefs.SetInt(Utilities.TOTAL_DEATHS_TAG, totalRecordedDeaths + currentDeaths);
		PlayerPrefs.SetInt(Utilities.LAST_LVL_DEATHS_TAG, currentDeaths);
	}
}
