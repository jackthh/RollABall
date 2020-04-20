using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour {

    public void QuitApp()
    {
        Debug.Log("Quitted!");
        Application.Quit();
    }
}
