using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountsData
{

	public int lvlReached = 0;
	public float totalScore = 0;
	public float lastLvlScore = 0;
	public float totalTime = 0;
	public float lastLvlTime = 0;

	public AccountsData(int lvlReached, float totalScore, float lastLvlScore, float totalTime, float lastLvlTime)
	{
		this.lvlReached = lvlReached;
		this.totalScore = totalScore;
		this.lastLvlScore = lastLvlScore;
		this.totalTime = totalTime;
		this.lastLvlTime = lastLvlTime;
	}
}
