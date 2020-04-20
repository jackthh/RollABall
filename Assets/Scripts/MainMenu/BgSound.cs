using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgSound : MonoBehaviour {

    private AudioSource source;

    private void Awake()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("BgSound");
        if (gameObjects.Length > 1)
        {
            Object.Destroy(this.gameObject);
            Debug.Log("Destroy on load");
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            Debug.Log("Don't destroy on load");
        }
    }

    private void Start()
    {
        source = this.GetComponent<AudioSource>();
    }


    public float GetVolume()
    {
        return this.source.volume;
    }

    public void SetVolume(float num)
    {
        if (0 <= num && num <= 1)
        {
            source.volume = num;
        }
    }
}
