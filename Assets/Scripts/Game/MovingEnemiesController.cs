using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemiesController : MonoBehaviour {

    public Material material1;
    public Material material2;
    public GameObject[] waypoints;
    public float movingSpeed;
    public int startPoint;
    int current = 0;
    float margin = 0.5f;

    private void Start()
    {
        StartCoroutine(ChangeColor(1.2f));
        current = startPoint;
    }

    private void Update()
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
