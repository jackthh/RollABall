using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsManager : MonoBehaviour {

    public delegate void OnResetLvlHandler();
    public static event OnResetLvlHandler OnResetLvl;
    public static void RaiseOnResetLvl()
    {
        if (OnResetLvl != null)
        {
            OnResetLvl();
        }
    }

    public delegate void OnPickUpHandler(Collider pickUpCollider, int playerScore);
    public static event OnPickUpHandler OnPickUp;
    public static void RaiseOnPickUp(Collider pickUpCollider, int playerScore)
    {
        if (OnPickUp != null)
        {
            OnPickUp(pickUpCollider, playerScore);
        }
    }

    public delegate void OnLoseOneLifeHandler(int playerHealth);
    public static event OnLoseOneLifeHandler OnLoseOneLife;
    public static void RaiseLoseOneLife(int playerHealth)
    {
        if (OnLoseOneLife != null)
        {
            OnLoseOneLife(playerHealth);
        }
    }

    public delegate void OnEndGameHandler(bool winning);
    public static event OnEndGameHandler OnEndGame;
    public static void RaiseOnEndGame(bool winning)
    {
        if (OnEndGame != null)
        {
            OnEndGame(winning);
        }
    }

    public delegate void OnLoadNewLvlHandler();
    public static event OnLoadNewLvlHandler OnLoadNewLvl;
    public static void LoadNextLvl()
    {
        if (OnLoadNewLvl != null)
        {
            OnLoadNewLvl();
        }
    }

}
