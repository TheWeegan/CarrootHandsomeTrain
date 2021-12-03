using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] GameObject _head;

    [SerializeField] GameObject _enemy;
    GridHandler _gridHandler;

    int _startIndex = -1;
    int _endIndex = -1;

    AStar _aStarPath;
    List<Vector2Int> _newPath = new List<Vector2Int>();
    PlayerController _playerController;

    // Start is called before the first frame update
    void Start()
    {
        _gridHandler = GetComponentInParent<GridHandler>();
        _playerController = GetComponent<PlayerController>();

        SpawnEnemy();
        _aStarPath = new AStar();
        _aStarPath.SetGrid = _gridHandler.GetGrid();
        _aStarPath.Set1DGrid = _gridHandler.Get1DGrid();
        _aStarPath.SetGridIDs = _gridHandler.GetGridIDs();
        _aStarPath.SetGridSize = _gridHandler.GetGridSize;

        _aStarPath._singlyLinkedList = _playerController.GetLinkedList;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Tail")
        {

        }
    }

    private void SpawnEnemy()
    {
        Vector3Int newPosition = new Vector3Int(Random.Range(1, _gridHandler.GetGridSize - 2), Random.Range(1, _gridHandler.GetGridSize - 2), -1);
        Vector3Int playerIntPosition = new Vector3Int((int)_playerController.transform.position.x, (int)_playerController.transform.position.y, -1);

        while(newPosition == playerIntPosition)
        {
            newPosition = new Vector3Int(Random.Range(1, _gridHandler.GetGridSize -2), Random.Range(1, _gridHandler.GetGridSize - 2), -1);
        }
        _enemy.gameObject.transform.position = newPosition;        
    }

    void HandleMovement()
    {
        if(_playerController.MovementTimer <= 0)
        {
            Vector2Int enemyPosition = new Vector2Int((int)_enemy.transform.position.x, (int)_enemy.transform.position.y);
            Vector2Int endPos = new Vector2Int((int)_head.transform.position.x, (int)_head.transform.position.y);

            if (InBorders(enemyPosition, endPos))
            {
                _startIndex = _gridHandler.GetGridID(enemyPosition.x, enemyPosition.y);
                _endIndex = _gridHandler.GetGridID(endPos.x, endPos.y);
                _newPath = _aStarPath.AStarPath(_startIndex, _endIndex);
                if(_newPath == null)
                {
                    return;
                }

                _gridHandler.GetGridID(enemyPosition.x, enemyPosition.y);

                for (int i = 0; i < _newPath.Count - 1; i++)
                {
                    if(enemyPosition.x == _newPath[i].x && enemyPosition.y == _newPath[i].y)
                    {
                        enemyPosition.x = _newPath[i + 1].x;
                        enemyPosition.y = _newPath[i + 1].y;
                        Vector3 tempPosition = new Vector3(enemyPosition.x, enemyPosition.y, -1);
                        _enemy.transform.position = tempPosition;
                        break;
                    }
                }

            }
        }
    }

    bool InBorders(Vector2Int startPos, Vector2Int endPos)
    {
        return ((startPos.x >= 0 && startPos.x < _gridHandler.GetGridSize && startPos.y >= 0 && startPos.y < _gridHandler.GetGridSize) &&
            (endPos.x >= 0 && endPos.x < _gridHandler.GetGridSize && endPos.y >= 0 && endPos.y < _gridHandler.GetGridSize));
    }
}
