using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    //Parameters for moving
    private Rigidbody rb;
    private bool doJump = false;
    private bool movable = true;
    public bool jumpable;
    public float verticalSpeed;
    public float horizontalSpeed;
    public float jumpForceMagnitude;

    //Parameters to update scores
    private int score = 0;
    private List<Transform> pickUpsList = new List<Transform>();
    public TextMeshProUGUI scoreText;
    public GameObject pickUps;

    //Parameters to adjust health
    private int health = 3;
    public TextMeshProUGUI livesText;
    private bool invulnerable = false;

    //Parameters to show alert
    public TextMeshProUGUI alertText;

    // Use this for initialization
    void Start()
    {
        Time.timeScale = 1;
        alertText.GetComponent<CanvasRenderer>().SetAlpha(0);

        rb = this.GetComponent<Rigidbody>();

        //Get pick up objects list
        for(int i = 0; i < pickUps.transform.childCount; i++)
        {
            pickUpsList.Add(pickUps.transform.GetChild(i));
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            doJump = true;
        }
    }


    private void FixedUpdate()
    {
        //Move
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        //Use movePos in order to prevent player from falling way too slowly
        //Vector3 newPosition = new Vector3(transform.position.x + horizontalInput * horizontalSpeed * Time.deltaTime, transform.position.y, transform.position.z + verticalInput * verticalSpeed * Time.deltaTime);
        //rb.MovePosition(newPosition);

        if (movable)
        {
            // Jump
            if (jumpable && doJump)
            {
                rb.AddForce(Vector3.up * jumpForceMagnitude, ForceMode.Impulse);
                doJump = false;
            }

            //Test
            float xVelocity = horizontalInput * horizontalSpeed;
            float yVelocity = rb.velocity.y;
            float zVelocity = verticalInput * verticalSpeed;
            rb.velocity = new Vector3(xVelocity, yVelocity, zVelocity);

            //End Test

        }

    }


    private void OnTriggerEnter(Collider other)
    {
        
        //Detect diamonds collecting
        if(other.gameObject.CompareTag("Diamond"))
        {
            EventsManager.RaiseOnPickUp(other);

            //Wait to be handled later
            //other.gameObject.SetActive(false);
            //IncreaseScore();
        }

        //Detect obstacles touched
        if(other.transform.CompareTag("Enemy") && !invulnerable)
        {
            LoseOneLife();
        }
    }


    private bool IsGrounded()
    {
        SphereCollider playerCollider = transform.GetComponent<SphereCollider>();
        return Physics.Raycast(playerCollider.bounds.center, Vector3.down, playerCollider.bounds.extents.y + 0.1f);
    }

    private void IncreaseScore()
    {
        this.score++;
        scoreText.text = score + "/" + pickUpsList.Count;
        StartCoroutine(Flicker(scoreText.GetComponent<CanvasRenderer>(),1f, 0.2f, false, false));
        if(this.score == pickUpsList.Count)
        {
            Won();
        }
    }

    public void LoseOneLife()
    {
        this.health--;
        livesText.text = health.ToString();
        StartCoroutine(Flicker(GetComponent<Renderer>(), 0.8f, 0.15f));
        StartCoroutine(Flicker(livesText.GetComponent<CanvasRenderer>(), 1f, 0.2f, false, false));
        StartCoroutine("BeInvulnerableTemporarily");
        if (this.health <= 0)
        {
            Lost();
        }
    }


    // Overloading function: make flickering effect on given objects
    //For UI objects
    private IEnumerator Flicker(CanvasRenderer objRenderer, float duration, float interval, bool endGame, bool winning)
    {
        //Stop player temporarily (not using timeScale) to prevent runtime bugs
        if (endGame)
        {
            this.movable = false;
            this.rb.velocity = Vector3.zero; 
        }

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
            if (winning)
            {
                StartCoroutine(LoadNewLvl());
            }
            else
            {
                objRenderer.SetAlpha(0);
                Reset();
            }
        }
    }

    //For  non-UI objects
    private IEnumerator Flicker(Renderer objRenderer, float duration, float interval)
    {
        float endTime = Time.time + duration;
        while (Time.time < endTime)
        {
            objRenderer.enabled = false;
            yield return new WaitForSeconds(interval);
            objRenderer.enabled = true;
            yield return new WaitForSeconds(interval);
        }
    }

    private IEnumerator BeInvulnerableTemporarily()
    {
        invulnerable = true;
        yield return new WaitForSeconds(1);
        invulnerable = false;
    }

    private void Lost()
    {
        EndGame(false);
    }

    private void Won()
    {
        EndGame(true);
    }


    private void EndGame(bool winning)
    {

        if (winning)
        {
            alertText.text = "Congratulations!\nYou win!";
        }
        else
        {
            alertText.text = "You lose!\nRelax, try harder!";
        }
        CanvasRenderer renderer = alertText.GetComponent<CanvasRenderer>();
        renderer.SetAlpha(1);
        StartCoroutine(Flicker(renderer,1f, 0.2f, true, winning));
    }

    private void Reset()
    {
        score = 0;
        health = 3;
        livesText.text = health.ToString();
        scoreText.text = score + "/" + pickUpsList.Count;
        transform.position = new Vector3(0, 0.6f, 0);
        foreach(Transform item in pickUpsList)
        {
            item.gameObject.SetActive(true);
        }
        Time.timeScale = 1;
    }


    private IEnumerator LoadNewLvl()
    {
        Debug.Log("Chay vao phan load lvl");
        string currentSceneName = SceneManager.GetActiveScene().name;
        int currentLvl = int.Parse(currentSceneName.Substring(3));
        PlayerPrefs.SetInt("levelReached", currentLvl + 1);
        AsyncOperation async;

        if (currentLvl >= 16)
        {
            async = SceneManager.LoadSceneAsync("MainMenu");
        } else
        {
            Debug.Log("Lvl hien tai = " + currentLvl);
            int nextLvl = ++currentLvl;
            async = SceneManager.LoadSceneAsync("Lvl" + nextLvl);
            Debug.Log("Lvl moi = " + nextLvl);
        }

        while (!async.isDone)
        {
            yield return null;
        }
    }


    private IEnumerator WaitFor(float duration, bool returnVar)
    {
        yield return new WaitForSecondsRealtime(duration);
        returnVar = false;
    }
}


