using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondsController : MonoBehaviour {

    // Add methods to handle events
    private void OnEnable()
    {
        EventsManager.OnResetLvl += Reset;
        EventsManager.OnPickUp += TouchPlayer;
    }

    private void OnDisable()
    {
        EventsManager.OnResetLvl -= Reset;
        EventsManager.OnPickUp -= TouchPlayer;
    }

    // Update is called once per frame
    void Update () {
        transform.Rotate(new Vector3(0, 150, 0) * Time.deltaTime);
	}

    private void TouchPlayer(Collider collider, int var)
    {
        if (collider.gameObject.name == this.gameObject.name)
        {
            this.transform.localScale = new Vector3(0, 0, 0);
            Debug.Log(this.gameObject.name + "Touch Player");
        }
    }

    private void Reset()
    {
        this.transform.localScale = new Vector3(1, 1, 1);
    }
}
