using System;
using UnityEngine;

public class PlayerRoomChecker : MonoBehaviour
{
    public event Action OnPlayerEntered = delegate { };
    private Collider checker;
    private bool firstTimeEntered;

    private void Awake()
    {
        checker = GetComponent<Collider>();
        checker.isTrigger = true;
        firstTimeEntered = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() && firstTimeEntered)
        {
            print("Player entered a new room!");
            OnPlayerEntered();
            checker.enabled = false;
            firstTimeEntered = false;
        }
    }
}