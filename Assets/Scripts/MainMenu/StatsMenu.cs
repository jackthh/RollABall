using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsMenu : MonoBehaviour
{

	public Text totalScore;
	public Text lastScore;

	public Text totalCoins;
	public Text lastCoins;

	public Text totalTime;
	public Text lastTime;

	public Text totalDeaths;
	public Text lastDeaths;


	private void Start()
	{
		totalScore.text = PlayerPrefs.GetInt(Utilities.TOTAL_SCORE_TAG, 0).ToString();
		lastScore.text = PlayerPrefs.GetInt(Utilities.LAST_LVL_SCORE_TAG, 0).ToString();

		totalCoins.text = PlayerPrefs.GetInt(Utilities.TOTAL_COINS_TAG, 0).ToString();
		lastCoins.text = PlayerPrefs.GetInt(Utilities.LAST_LVL_COINS_TAG, 0).ToString();

		totalTime.text = PlayerPrefs.GetFloat(Utilities.TOTAL_TIME_TAG, 0f).ToString();
		lastTime.text = PlayerPrefs.GetFloat(Utilities.LAST_LVL_TIME_TAG, 0f).ToString();

		totalDeaths.text = PlayerPrefs.GetInt(Utilities.TOTAL_DEATHS_TAG, 0).ToString();
		lastDeaths.text = PlayerPrefs.GetInt(Utilities.LAST_LVL_DEATHS_TAG, 0).ToString();
	}

}

