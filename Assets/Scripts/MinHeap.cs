using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class MinHeap
{
	List<AStarNode> _data = new List<AStarNode>();

	public int size()
    {
		return (int)(_data.Count);
	}

	public void enqueue(AStarNode element)
	{
		int current_node = (int)(_data.Count);
		int parent_node = get_parent_index(current_node);
		_data.Add(element);
		
		while (!Equals(current_node, parent_node) && 
			_data[current_node].nodeDistanceToStart < _data[parent_node].nodeDistanceToStart)
		{
			swap(current_node, parent_node);
			current_node = parent_node;
			parent_node = get_parent_index(parent_node);
		}
	}
	public AStarNode get_top()
	{
		return _data[0];
	}

	public AStarNode dequeue()
	{
		AStarNode return_value = _data[0];
		swap(0, (int)(_data.Count - 1));
		_data.RemoveAt((_data.Count - 1));
		move_down(0);

		return return_value;
	}

	public int get_parent_index(int index)
	{
		return (index - 1) / 2;
	}

	public int get_left_child_index(int index) {
		return index * 2 + 1;
	}

	public int get_right_child_index(int index)
	{
		return index * 2 + 2;
	}

	public void swap(int index, int another_index)
	{
		AStarNode temp_index = _data[index];
		_data[index] = _data[another_index];
		_data[another_index] = temp_index;
	}

	public void move_down(int first)
	{
		int smallest = get_left_child_index(first);
		int last = (int)(_data.Count - 1);
		while (smallest <= last)
		{
			if (smallest < last && _data[smallest + 1].nodeDistanceToStart < _data[smallest].nodeDistanceToStart)
			{
				++smallest;
			}

			if ((_data[smallest].nodeDistanceToStart < _data[first].nodeDistanceToStart) || 
				(!(_data[smallest].nodeDistanceToStart < _data[first].nodeDistanceToStart) && 
				(!(_data[first].nodeDistanceToStart < _data[smallest].nodeDistanceToStart))))
			{
				swap(first, smallest);
				first = smallest;
				smallest = get_left_child_index(first);
			}
			else
			{
				smallest = last + 1;
			}
		}
	}
	
};


