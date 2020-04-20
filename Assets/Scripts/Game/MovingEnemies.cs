using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemies : MonoBehaviour {

    public Material material1;
    public Material material2;
    public GameObject[] waypoints;
    public float movingSpeed;
    public int startPoint;
    private int current;
	private float margin = 0.5f;

	private bool isPaused = false;


	public void SetIsPaused(bool value)
	{
		this.isPaused = value;
	}


    private void Start()
    {
        StartCoroutine(ChangeColor(1.2f));
        current = startPoint;
    }


    private void Update()
    {
		if (isPaused)
		{
			return;
		}
		else
		{
			if (Vector3.Distance(waypoints[current].transform.position, this.transform.position) < margin)
			{
				current++;
				if (current >= waypoints.Length)
				{
					current = 0;
				}
			}
			transform.position = Vector3.MoveTowards(transform.position, waypoints[current].transform.position, movingSpeed * Time.deltaTime);
		}
    }


    private IEnumerator ChangeColor(float interval)
    {
        while (true)
        {
            this.GetComponent<Renderer>().material = material1;
            yield return new WaitForSeconds(interval);
            this.GetComponent<Renderer>().material = material2;
            yield return new WaitForSeconds(interval);
        }
    }
}
