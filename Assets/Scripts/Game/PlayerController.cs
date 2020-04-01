using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class PlayerController : MonoBehaviour
{
    // Parameters for moving
    private int health = 3;
    private int score = 0;
    private Rigidbody rb;
    private bool doJump = false;
    private bool movable = true;
    private bool invulnerable = false;
    private List<Transform> pickUpsList = new List<Transform>();
    private int backCount = 0;
    public bool jumpable;
    public float verticalSpeed;
    public float horizontalSpeed;
    public float jumpForceMagnitude;
    public GameObject pickUps;

    private void OnEnable()
    {
        EventsManager.OnLoadNewLvl += LoadNextLvl;
        EventsManager.OnResetLvl += Reset;
    }
    private void OnDisable()
    {
        EventsManager.OnLoadNewLvl -= LoadNextLvl;
        EventsManager.OnResetLvl -= Reset;
    }

    // Use this for initialization
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        //Get pick up objects list
        for (int i = 0; i < pickUps.transform.childCount; i++)
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
        if (Input.GetKey(KeyCode.Escape) && Application.isMobilePlatform)
        {
            backCount++;
            StartCoroutine(FlagBack());
            if (backCount > 1)
            {
                StartCoroutine(LoadBack());
            }
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
            // Test
            float xVelocity = horizontalInput * horizontalSpeed;
            float yVelocity = rb.velocity.y;
            float zVelocity = verticalInput * verticalSpeed;
            rb.velocity = new Vector3(xVelocity, yVelocity, zVelocity);

            // End Test

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detect diamonds collecting
        if(other.CompareTag("Diamond"))
        {
            EventsManager.RaiseOnPickUp(other, ++this.score);
            if (this.score == pickUpsList.Count)
            {
                Pause();
                EventsManager.RaiseOnEndGame(true);
            }
        }

        // Detect obstacles touched
        if(other.CompareTag("Enemy") && !invulnerable)
        {
            Debug.Log("Touched enemy");
            this.health--;
            if (IsDead())
            {
                Debug.Log("Dead");
                Pause();
                EventsManager.RaiseOnEndGame(false);
            }
            else
            {
                Debug.Log("Lose 1 life");
                EventsManager.RaiseLoseOneLife(health);
                Renderer renderer = this.GetComponent<Renderer>();
                StartCoroutine(Flicker(renderer, 0.8f, 0.15f));
            }

        }
    }

    private bool IsGrounded()
    {
        SphereCollider playerCollider = transform.GetComponent<SphereCollider>();
        return Physics.Raycast(playerCollider.bounds.center, Vector3.down, playerCollider.bounds.extents.y + 0.1f);
    }

    private bool IsDead()
    {
        if (this.health <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    } 
     
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
        this.invulnerable = true;
        yield return new WaitForSeconds(1f);
        this.invulnerable = false;
    }

    private void Pause()
    {
        invulnerable = true;
        movable = false;
        rb.velocity = Vector3.zero;
    }

    private void Reset()
    {
        this.health = 3;
        this.score = 0;
        this.movable = true;
        this.invulnerable = false;
        this.transform.position = new Vector3(0, 0.6f, 0);
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
    
    private IEnumerator LoadBack()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync("MainMenu");
        while (!async.isDone)
        {
            yield return null;
        }
    }

    private IEnumerator FlagBack()
    {
        yield return new WaitForSeconds(1f);
        backCount = 0;
    }
}

