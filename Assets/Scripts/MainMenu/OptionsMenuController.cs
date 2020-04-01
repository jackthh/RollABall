using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuController : MonoBehaviour {

    public Slider bgSlider;
    public Slider effSlider;
    public GameObject bgManager;
    public GameObject effManager;

    private void Start()
    {
        bgSlider.value = bgManager.GetComponent<AudioSource>().volume;
        effSlider.value = effManager.GetComponent<AudioSource>().volume;
    }
}
