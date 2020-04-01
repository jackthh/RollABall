using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour {

    /* Parameters to update scores */
    private List<Transform> pickUpsList = new List<Transform>();
    public GameObject pickUps;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI lvlText;
    public TextMeshProUGUI alertText;

    private void OnEnable()
    {
        EventsManager.OnPickUp += IncreaseOneScore;
        EventsManager.OnEndGame += OnEndGame;
        EventsManager.OnLoseOneLife += LoseOneLife;
    }
    private void OnDisable()
    {
        EventsManager.OnPickUp -= IncreaseOneScore;
        EventsManager.OnEndGame -= OnEndGame;
        EventsManager.OnLoseOneLife -= LoseOneLife;

    }

    private void Start()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        int currentLvl = int.Parse(currentSceneName.Substring(3));
        lvlText.text = "Level " + currentLvl;
        alertText.GetComponent<CanvasRenderer>().SetAlpha(0);
        //Get pick up objects list
        for (int i = 0; i < pickUps.transform.childCount; i++)
        {
            pickUpsList.Add(pickUps.transform.GetChild(i));
        }
        scoreText.text = 0 + "/" + pickUpsList.Count;
    }

    // Needless to use given Collider argument cause we just wanna change value of Score
    private void IncreaseOneScore(Collider tmp, int playerScore)
    {
        scoreText.text = playerScore + "/" + pickUpsList.Count;
        CanvasRenderer renderer = scoreText.GetComponent<CanvasRenderer>();
        StartCoroutine(Flicker(renderer, 1f, 0.2f, false, false));
    }

    private void LoseOneLife(int playerHealth)
    {
        livesText.text = playerHealth.ToString();
        CanvasRenderer renderer = livesText.GetComponent<CanvasRenderer>();
        StartCoroutine(Flicker(renderer, 1f, 0.2f,false, false));
    }

    private void OnEndGame(bool winning)
    {
        CanvasRenderer renderer = alertText.GetComponent<CanvasRenderer>();
        renderer.SetAlpha(1);
        if (winning)
        {
            alertText.text = "Congratulations!\nYou win!";
            StartCoroutine(Flicker(renderer, 1f, 0.2f, true, false));
        }
        else
        {
            alertText.text = "You lose!\nRelax, try harder!";
            StartCoroutine(Flicker(renderer, 1f, 0.2f,true, true));
        }
        renderer.SetAlpha(0);
    }

    // For UI objects
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
                EventsManager.RaiseOnResetLvl();
                Reset();
            }
            else
            {
                EventsManager.LoadNextLvl();
            }
        }
    }

    private void Reset()
    {
        scoreText.text = 0 + "/" + pickUpsList.Count;
        livesText.text = "3";
        alertText.text = "";
        alertText.canvasRenderer.SetAlpha(0);
    }
}
