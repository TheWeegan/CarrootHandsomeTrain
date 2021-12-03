using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CurrentKeyDown
{
    Up,
    Left,
    Right,
    Down,
    None
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject _head;
    [SerializeField] GameObject _tail;
    [SerializeField] GameObject _gameOverCanvas;
    [SerializeField] List<AudioClip> _audioClips;

    AudioSource _audioSource;
    GridHandler _gridHandler;

    SinglyLinkedList _singlyLinkedList = new SinglyLinkedList();

    CurrentKeyDown _currentKeyDown = CurrentKeyDown.None;
    CurrentKeyDown _latestMovement = CurrentKeyDown.None;

    Vector3 _formerPosition;
    Vector3 _lastPosition;

    bool _playerIsDead = false;

    float _gameSpeedTimer = 0.25f;
    float _movementTimer = 0;
    const float _gameSpeedEnhancer = 0.01f;

    public float GameSpeed { get { return _gameSpeedTimer; } set { _gameSpeedTimer = value; } }
    public float MovementTimer { get { return _movementTimer; } }
    public bool PlayerIsDead { get { return _playerIsDead; } set { _playerIsDead = value; } }
    public SinglyLinkedList GetLinkedList { get { return _singlyLinkedList; } }

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _gridHandler = GetComponentInParent<GridHandler>();

        Vector3 randomPosition = new Vector3(Random.Range(1, _gridHandler.GetGridSize - 2), Random.Range(1, _gridHandler.GetGridSize - 2), -1);

        _formerPosition = randomPosition;
        _lastPosition = randomPosition;
        _head.transform.position = randomPosition;
        _singlyLinkedList.Append(_head);

        _movementTimer = _gameSpeedTimer;
    }

    void Update()
    {
        if (!_playerIsDead)
        {
            HandleInput();
            PlayerMovement();
        }
        else
        {
            _gameOverCanvas.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            if(_latestMovement != CurrentKeyDown.Up && _latestMovement != CurrentKeyDown.Down)
            {
                _currentKeyDown = CurrentKeyDown.Up;
            }
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (_latestMovement != CurrentKeyDown.Left && _latestMovement != CurrentKeyDown.Right)
            {
                _currentKeyDown = CurrentKeyDown.Left;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (_latestMovement != CurrentKeyDown.Right && _latestMovement != CurrentKeyDown.Left)
            {
                _currentKeyDown = CurrentKeyDown.Right;
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            if (_latestMovement != CurrentKeyDown.Down && _latestMovement != CurrentKeyDown.Up)
            {
                _currentKeyDown = CurrentKeyDown.Down;
            }
        }
    }
    
    void PlayerMovement()
    {
        if (_movementTimer > 0 && _currentKeyDown != CurrentKeyDown.None)
        {
            _movementTimer -= Time.deltaTime;
            return;
        }
        _movementTimer = _gameSpeedTimer;

        SinglyLinkedList.Node node = _singlyLinkedList.First;
        _formerPosition = node._data.transform.position;
        Vector3 newPosition = node._data.transform.position;
        newPosition.z = -1;

        switch (_currentKeyDown)
        {
            case CurrentKeyDown.Up:
                {
                    newPosition.y += 1;
                    break;
                }
            case CurrentKeyDown.Left:
                {
                    newPosition.x -= 1;
                    break;
                }
            case CurrentKeyDown.Right:
                {
                    newPosition.x += 1;
                    break;
                }
            case CurrentKeyDown.Down:
                {
                    newPosition.y -= 1;
                    break;
                }
            default:
                {
                    break;
                }
        }
        _latestMovement = _currentKeyDown;

        while (node != null)
        {
            _formerPosition = node._data.transform.position;

            node._data.transform.position = newPosition;

            newPosition = _formerPosition;
            _lastPosition = newPosition;
            

            node = node._next;
        }
    }

    public void ExpandPlayer()
    {
        StartCoroutine(ExpandPlayerTimer());
    }

    private IEnumerator ExpandPlayerTimer()
    {
        yield return new WaitForSeconds(_gameSpeedTimer);
        GameObject newTail = Instantiate(_tail);
        newTail.transform.position = _lastPosition;
        _singlyLinkedList.Append(newTail);

        _audioSource.PlayOneShot(_audioClips[0]);

        if(_gameSpeedTimer > _gameSpeedEnhancer)
        {
            _gameSpeedTimer -= _gameSpeedEnhancer;
        }
    }

}
