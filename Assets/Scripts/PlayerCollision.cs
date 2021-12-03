using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    GridHandler _gridHandler;

    private void Start()
    {
        _gridHandler = GetComponentInParent<GridHandler>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerController = gameObject.GetComponentInParent<PlayerController>();
        if(other.tag == "Border" && playerController != null)
        {
            TeleportPlayer();
        }        
        else if((other.tag == "Enemy" || other.tag == "Tail") && playerController != null)
        {
            playerController.PlayerIsDead = true;
        }
    }

    void TeleportPlayer()
    {
        Vector3Int newPosition = new Vector3Int(0, 0, -1);
        newPosition.x = (int)gameObject.transform.position.x;
        newPosition.y = (int)gameObject.transform.position.y;

        if (newPosition.x < 0)
        {
            newPosition.x += _gridHandler.GetGridSize;
        }
        else if(newPosition.x >= _gridHandler.GetGridSize)
        {
            newPosition.x -= _gridHandler.GetGridSize;
        }
        else if (newPosition.y < 0)
        {
            newPosition.y += _gridHandler.GetGridSize;
        }
        else if (newPosition.y >= _gridHandler.GetGridSize)
        {
            newPosition.y -= _gridHandler.GetGridSize;
        }
        gameObject.transform.position = newPosition;
    }
}
