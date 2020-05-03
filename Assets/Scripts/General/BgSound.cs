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
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
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


	public void Pause()
	{
		source.Pause();
	}


	public void UnPause()
	{
		source.UnPause();
	}
}
