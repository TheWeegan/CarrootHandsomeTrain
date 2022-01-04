using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    GridHandler _gridHandler;

    string _borderTag = "Border";
    string _enemyTag = "Enemy";
    string _tailTag = "Tail";

    private void Start()
    {
        _gridHandler = GetComponentInParent<GridHandler>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerController = gameObject.GetComponentInParent<PlayerController>();
        if(other.tag == _borderTag && playerController != null)
        {
            _gridHandler.TeleportPlayer(gameObject);
        }        
        else if((other.tag == _enemyTag || other.tag == _tailTag) && playerController != null)
        {
            playerController.PlayerIsDead = true;
        }
    }

}
