using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglyLinkedList
{
    public class Node
    {
        public Node _next;
        public GameObject _data;
    }

    Node _root = null;
    int _size = 0;

    public Node First { get { return _root; } }

    public Node Last
    {
        get
        {
            Node currentNode = _root;
            if (currentNode == null)
            {
                return null;
            }
            while (currentNode._next != null)
            {
                currentNode = currentNode._next;
            }
            return currentNode;
        }
    }

    public int Size()
    {
        return _size;
    }

    public Node GetNodeFromIndex(int index)
    {
        Node currentNode = _root;
        
        for (int i = 0; i < index; ++i)
        {
            currentNode = currentNode._next;
        }
        return currentNode;
    }

    public void Append(GameObject value)
    {
        Node currentNode = new Node { _data = value };
        ++_size;

        if (_root == null)
        {
            _root = currentNode;
        }
        else
        {
            Last._next = currentNode;
        }
    }

    public void Delete(Node node)
    {
        if (_root == node)
        {
            _root = node._next;
            node._next = null;
        }
        else
        {
            Node currentNode = _root;
            while (currentNode._next != null)
            {
                if (currentNode._next == node)
                {
                    currentNode._next = node._next;
                    node._next = null;
                    break;
                }
                currentNode = currentNode._next;
            }
        }
    }
}
