using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour {

	[Header("References")]
	public GameObject gameMasterContainer;
	private GameMaster gameMaster;

	[Header("Stats Update")]
	private int coinsCount;

	public Text lvlText;
	public Text scoreText;
    public Text coinsText;
    public Text livesText;

	[Header("Display Notifications")]
	public Text alertText;


    private void Start()
    {
		gameMaster = gameMasterContainer.GetComponent<GameMaster>();

        string currentSceneName = SceneManager.GetActiveScene().name;
        int currentLvl = int.Parse(currentSceneName.Substring(3));
        lvlText.text = "Level " + currentLvl;
        alertText.GetComponent<CanvasRenderer>().SetAlpha(0);
	}


	public void SetCoinsCount(int _coinsCount)
	{
		this.coinsCount = _coinsCount;
	}


	// This func will be called from GameMaster
	public void UpdateScore(float _score)
	{
		scoreText.text = _score.ToString();
		CanvasRenderer canvasRenderer = scoreText.GetComponent<CanvasRenderer>();
		StartCoroutine(Flicker(canvasRenderer, 1.2f, 0.2f, false, false));
	}


    // This func will be called from GameMaster
    public void UpdateCoins(int currentCoins)
    {
        coinsText.text = currentCoins + "/" + this.coinsCount;
        CanvasRenderer canvasRenderer = coinsText.GetComponent<CanvasRenderer>();
        StartCoroutine(Flicker(canvasRenderer, 0.8f, 0.2f, false, false));
    }


    public void UpdateHealth(int playerHealth)
    {
        livesText.text = playerHealth.ToString();
        CanvasRenderer renderer = livesText.GetComponent<CanvasRenderer>();
        StartCoroutine(Flicker(renderer, 1f, 0.2f,false, false));
    }


    public void OnEndGame(bool winning)
    {
        CanvasRenderer renderer = alertText.GetComponent<CanvasRenderer>();
        renderer.SetAlpha(1);
        if (winning)
        {
            alertText.text = "Congratulations!\nYou win!";
            StartCoroutine(Flicker(renderer, 1.2f, 0.2f, true, false));
        }
        else
        {
            alertText.text = "You lose!\nRelax, try harder!";
            StartCoroutine(Flicker(renderer, 1.2f, 0.2f,true, true));
        }
        renderer.SetAlpha(0);
    }


    private IEnumerator Flicker(CanvasRenderer objRenderer, float duration, float interval, bool endGame, bool reset)
    {
        float endTime = Time.unscaledTime + duration;

        while (Time.unscaledTime < endTime)
        {
            objRenderer.SetAlpha(0f);
            yield return new WaitForSeconds(interval);
            objRenderer.SetAlpha(1f);
            yield return new WaitForSeconds(interval);
        }

        objRenderer.SetAlpha(1);

        if (endGame)
        {
            if (reset)
            {
				gameMaster.Reset();
            }
            else
            {
                gameMaster.LoadNextLvl();
            }
        }
    }


    public void OnReset()
    {
		scoreText.text = "0";
		coinsText.text = "0/" + coinsCount.ToString();
        livesText.text = "3";
        alertText.canvasRenderer.SetAlpha(0);
    }
}
