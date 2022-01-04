using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHandler : MonoBehaviour
{
    [SerializeField] GameObject _gridTile;
    [SerializeField] GameObject _borderPiece;
    [SerializeField] int _gridSize = 10;
    [SerializeField] int _tileSize = 1;

    List<GameObject> _border;

    GameObject[,] _grid;
    GameObject[] _1Dgrid;

    int[,] _gridIDs;

    public GameObject[] Get1DGrid { get { return _1Dgrid; } }
    public GameObject[,] GetGrid { get { return _grid; } }
    public int[,] GetGridIDs { get { return _gridIDs; } }
    public int GetGridSize { get { return _gridSize; } }
    public int GetTileSize { get { return _tileSize; } }

    public int GetGridID(int x, int y)
    {
        return _gridIDs[x, y];
    }

    private void Awake()
    {
        CreateGrid();
        CreateBorder();
        Camera mainCamera = Camera.main;
        Vector3 cameraPosition = new Vector3(_gridSize * 0.5f, _gridSize * 0.5f, -(_gridSize + 1));
        mainCamera.transform.position = cameraPosition;
    }

    void CreateGrid()
    {        
        _grid = new GameObject[_gridSize, _gridSize];
        _1Dgrid = new GameObject[_gridSize * _gridSize];
        _gridIDs = new int[_gridSize, _gridSize];

        int i = 0;
        for (int x = 0; x < _grid.GetLength(0); ++x)
        {
            for (int y = 0; y < _grid.GetLength(1); ++y)
            {
                _grid[x, y] = Instantiate(_gridTile);
                _grid[x, y].transform.position = new Vector2(x, y);

                _1Dgrid[i] = _grid[x, y];
                _gridIDs[x, y] = i;
                ++i;
            }
        }
    }

    void CreateBorder()
    {
        _border = new List<GameObject>();
        Vector3 piecePosition = new Vector3(-1, -1, -1);
        _borderPiece.transform.position = piecePosition;

        int index = 0;
        int borderOffset = _tileSize;

        int borderSizeY = (_grid.GetLength(1) * 2) + _tileSize;
        for (; index < borderSizeY; index++)
        {
            piecePosition.y += _tileSize;
            _border.Add(Instantiate(_borderPiece));
            _border[index].transform.position = piecePosition;
            if(index == _grid.GetLength(1) - borderOffset)
            {
                piecePosition.x += _grid.GetLength(0) + borderOffset;
                piecePosition.y -= _grid.GetLength(1);
            }
        }

        int borderSizeX = (_grid.GetLength(0) * 2) + _tileSize * 2;
        piecePosition.x -= _grid.GetLength(0) + _tileSize * 2;

        for (int i = 0; i < borderSizeX; i++)
        {
            piecePosition.x += borderOffset;
            _border.Add(Instantiate(_borderPiece));
            _border[index].transform.position = piecePosition;

            if (i == _grid.GetLength(0))
            {
                piecePosition.x -= _grid.GetLength(0);
                piecePosition.y -= _grid.GetLength(1) + borderOffset;
            }
            index++;
        }
    }

    public void TeleportPlayer(GameObject playerGameObject)
    {
        Vector3Int newPosition = new Vector3Int(0, 0, -1);
        newPosition.x = (int)playerGameObject.transform.position.x;
        newPosition.y = (int)playerGameObject.transform.position.y;

        if (newPosition.x < 0)
        {
            newPosition.x += _gridSize;
        }
        else if (newPosition.x >= _gridSize)
        {
            newPosition.x -= _gridSize;
        }
        else if (newPosition.y < 0)
        {
            newPosition.y += _gridSize;
        }
        else if (newPosition.y >= _gridSize)
        {
            newPosition.y -= _gridSize;
        }
        playerGameObject.transform.position = newPosition;

    }
}
