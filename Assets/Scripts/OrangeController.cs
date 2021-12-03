using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static SinglyLinkedList;

public class OrangeController : MonoBehaviour
{
    [SerializeField] Text _scoreText;

    GridHandler _gridHandler;
    PlayerController _playerController;

    uint _score = 0;

    // Start is called before the first frame update
    void Start()
    {
        _gridHandler = GetComponentInParent<GridHandler>();
        _playerController = GetComponentInParent<PlayerController>();
        SpawnOrange();
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerController = other.GetComponentInParent<PlayerController>();
        if (playerController != null)
        {
            playerController.ExpandPlayer();
            SpawnOrange();
            AddScore(10);
        }
    }

    void SpawnOrange()
    {
        Vector3Int newPosition = new Vector3Int(Random.Range(0, _gridHandler.GetGridSize - 1), Random.Range(0, _gridHandler.GetGridSize - 1), -1);
        Vector3Int playerIntPosition = new Vector3Int((int)_playerController.transform.position.x, (int)_playerController.transform.position.y, -1);

        while (newPosition == playerIntPosition)
        {
            newPosition = new Vector3Int(Random.Range(0, _gridHandler.GetGridSize - 1), Random.Range(0, _gridHandler.GetGridSize - 1), -1);
        }
        gameObject.transform.position = newPosition;

    }

    void AddScore(uint scoreAmount)
    {
        _score += scoreAmount;
        _scoreText.text = "Score: " + _score.ToString();
    }
}
