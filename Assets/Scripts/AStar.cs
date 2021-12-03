using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AStarNode
{
    public float nodeDistanceToStart;

    public int nodePredecessorId;
    public int nodeId;

    public int[] neighborsID = { -1, -1, -1, -1, -1, -1, -1, -1};


    public AStarNode()
    {
        nodeDistanceToStart = float.MaxValue;
        nodePredecessorId = -1;
        nodeId = -1;
    }


    public AStarNode(float distanceToStart, int id, int predecessorId)
    {
        nodeDistanceToStart = distanceToStart;
        nodePredecessorId = predecessorId;
        nodeId = id;
    }

    bool CloserThanPreviousDistance(AStarNode node)
    {
        return nodeDistanceToStart < node.nodeDistanceToStart;
    }

    bool Unvisited()
    {
        return nodeDistanceToStart == float.MaxValue;
    }
};

public class AStar
{
    GameObject[,] _grid;
    GameObject[] _1Dgrid;
    int[,] _gridIDs;

    int _gridSize;
    bool _playerAtNode = false;

    public GameObject[,] SetGrid { set { _grid = value;  } }
    public GameObject[] Set1DGrid { set { _1Dgrid = value;  } }
    public int[,] SetGridIDs { set { _gridIDs = value;  } }
    public int SetGridSize { set { _gridSize = value;  } }


    public SinglyLinkedList _singlyLinkedList;

    public List<Vector2Int> AStarPath(int startIndex, int endIndex)
    {
        if(startIndex < 0 || endIndex < 0)
        {
            return null;
        }

        MinHeap nodesToBeChecked = new MinHeap();
        Dictionary<int, AStarNode> visitedNodes = new Dictionary<int, AStarNode>();

        AStarNode startNode = new AStarNode(0, startIndex, endIndex);
        GetNeighbors(ref startNode);
        nodesToBeChecked.enqueue(startNode);


        while (nodesToBeChecked.size() > 0)
        {
            AStarNode currentNode = nodesToBeChecked.dequeue();
            visitedNodes.TryGetValue(currentNode.nodeId, out AStarNode visitedNode);
            _playerAtNode = false;
            if(currentNode.nodeId != startIndex && currentNode.nodeId != endIndex)
            {
                for (int i = 0; i < _singlyLinkedList.Size(); i++)
                {
                    Vector2Int position =  new Vector2Int((int)_singlyLinkedList.GetNodeFromIndex(i)._data.transform.position.x,
                        (int)_singlyLinkedList.GetNodeFromIndex(i)._data.transform.position.y);
                    Vector2Int currentNodePos = new Vector2Int((int)_1Dgrid[currentNode.nodeId].transform.position.x, 
                        (int)_1Dgrid[currentNode.nodeId].transform.position.y);
                    
                    if(position.x == currentNodePos.x && position .y == currentNodePos.y)
                    {
                        _playerAtNode = true;
                        break;
                    }
                }
            }


            if (visitedNode == null && !_playerAtNode)
            {
                visitedNodes.Add(currentNode.nodeId, currentNode);
            }
            else
            {
                continue;
            }



            if (currentNode.nodeId == endIndex)
            {
                return GetFinalPath(visitedNodes, startIndex);
            }

            for (int i = 0; i < currentNode.neighborsID.Length; ++i)
            {
                if (currentNode.neighborsID[i] >= 0)
                {
                    float F = CalculateDistanceToStart(currentNode.nodeDistanceToStart,
                        _1Dgrid[currentNode.neighborsID[i]].transform.position, _1Dgrid[currentNode.nodeId].transform.position);
                    
                    AStarNode node = new AStarNode(F, currentNode.neighborsID[i], currentNode.nodeId);
                    GetNeighbors(ref node);
                    nodesToBeChecked.enqueue(node);
                }
            }
        }
        return null;
    }

    bool TileInsideArray(Vector2Int neighborPosition)
    {
        return (neighborPosition.x >= 0 && neighborPosition.x < _gridSize && 
            neighborPosition.y >= 0 && neighborPosition.y < _gridSize);
    }

    void GetNeighbors(ref AStarNode aStarNode)
    {
        Vector2Int currentTile = new Vector2Int();
        Vector2Int[] neighborPositions = new Vector2Int[8];

        currentTile.x = (int)_1Dgrid[aStarNode.nodeId].transform.position.x;
        currentTile.y = (int)_1Dgrid[aStarNode.nodeId].transform.position.y;

        neighborPositions[0] = new Vector2Int(currentTile.x - 1, currentTile.y + 1);
        neighborPositions[1] = new Vector2Int(currentTile.x, currentTile.y + 1);
        neighborPositions[2] = new Vector2Int(currentTile.x + 1, currentTile.y + 1);

        neighborPositions[3] = new Vector2Int(currentTile.x - 1, currentTile.y);
        neighborPositions[4] = new Vector2Int(currentTile.x + 1, currentTile.y);
        
        neighborPositions[5] = new Vector2Int(currentTile.x - 1, currentTile.y - 1);
        neighborPositions[6] = new Vector2Int(currentTile.x, currentTile.y - 1);
        neighborPositions[7] = new Vector2Int(currentTile.x + 1, currentTile.y - 1);

        for (int i = 0; i < neighborPositions.Length; i++)
        {
            if (TileInsideArray(neighborPositions[i]))
            {
                aStarNode.neighborsID[i] = _gridIDs[neighborPositions[i].x, neighborPositions[i].y];
            }
        }
    }

    List<Vector2Int> GetFinalPath(Dictionary<int, AStarNode> visitedNodes, int startNodeID)
    {
        List<Vector2Int> finalPath = new List<Vector2Int>();

        int currentIndex = visitedNodes.Count - 1;

        AStarNode currentNode = visitedNodes.ElementAt(currentIndex).Value; ;

        Vector2Int actualPosition = new Vector2Int();

        while (currentNode.nodeId != startNodeID)
        {
            actualPosition.x = (int)_1Dgrid[currentNode.nodeId].transform.position.x;
            actualPosition.y = (int)_1Dgrid[currentNode.nodeId].transform.position.y;
            
            finalPath.Insert(0, actualPosition);

            for (int i = 0; i < visitedNodes.Count; i++)
            {                
                if (visitedNodes.ElementAt(i).Key == currentNode.nodePredecessorId)
                {
                    currentNode = visitedNodes.ElementAt(i).Value;
                    break;
                }
            }
        }
        actualPosition.x = (int)_1Dgrid[currentNode.nodeId].transform.position.x;
        actualPosition.y = (int)_1Dgrid[currentNode.nodeId].transform.position.y;

        finalPath.Insert(0, actualPosition);
        return finalPath;
    }

    float CalculateDistanceToStart(float distance, Vector2 aPosition1, Vector2 aPosition2)
    {        
	    float calculatedDistance = Mathf.Sqrt(Mathf.Pow(aPosition2.x - aPosition1.x, 2) + Mathf.Pow(aPosition2.y - aPosition1.y, 2));
	    return calculatedDistance + distance;
    }
}
