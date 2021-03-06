﻿using UnityEngine;
using System.Collections;

public static class Utilities
{
	public static string BG_SOUND_MANAGER_TAG = "BgSound";
	public static string EFF_SOUND_MANAGER_TAG = "EffSound";
	public static string GAME_MASTER_TAG = "GameMaster";
	public static string JOYSTICK_TAG = "Joystick";

	public static string LVL_REACHED_TAG = "levelReached";

	public static string SHOW_TUT_TAG = "showTut";

	public static string TOTAL_SCORE_TAG = "totalScore";
	public static string LAST_LVL_SCORE_TAG = "lastLvlScore";

	public static string TOTAL_COINS_TAG = "totalCoins";
	public static string LAST_LVL_COINS_TAG = "lastLvlCoins";

	public static string TOTAL_TIME_TAG = "totalTime";
	public static string LAST_LVL_TIME_TAG = "lastLvlTime";

	public static string TOTAL_DEATHS_TAG = "totalDeaths";
	public static string LAST_LVL_DEATHS_TAG = "lastLvlDeaths";


	public static float DegreeToRad(float _degree)
	{
		return _degree * Mathf.PI / 180;
	}


	public static float RadToDegree(float _rad)
	{
		return 180 * _rad / Mathf.PI;
	}
}
