using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour {

    private AudioSource source;
    public AudioClip collecting;
    public AudioClip dieing;
    public AudioClip losing;
    public AudioClip winning;


    public float GetVolume()
    {
        return this.source.volume;
    }


    public void SetVolume(float num)
    {
        if (num >= 0 && num <= 1)
        {
            this.source.volume = num;
        }
    }


    private void Awake()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("EffSound");
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
        source = GetComponent<AudioSource>();
    }

 
    public void PlayCollectingEffect(Collider dump1, int dump2)
    {
        source.PlayOneShot(collecting, this.source.volume);
    }


    public void PlayDieingEffect(int dump)
    {
        source.PlayOneShot(dieing, this.source.volume);
    }


    public void PlayEndGameEffect(bool isVictorious)
    {
        if (isVictorious)
        {
            source.PlayOneShot(winning, this.source.volume);
        }
        else
        {
            source.PlayOneShot(losing, this.source.volume);
        }
    }
}
