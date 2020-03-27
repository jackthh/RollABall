using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManager : MonoBehaviour {

    public delegate void OnResetLvlHandler();
    public static event OnResetLvlHandler OnResetLvl;
    public static void RaiseOnResetLvl()
    {
            OnResetLvl();
    }


    public delegate void OnPickUpHandler(Collider pickUpCollider);
    public static event OnPickUpHandler OnPickUp;
    public static void RaiseOnPickUp(Collider pickUpCollider)
    {
        if (OnPickUp != null)
        {
            OnPickUp(pickUpCollider);
        }
    }
}
