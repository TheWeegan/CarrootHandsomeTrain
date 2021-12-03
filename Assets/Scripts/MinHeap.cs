using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class MinHeap
{
	List<AStarNode> data = new List<AStarNode>();


	public int size()
    {
		return (int)(data.Count);
	}

	public void enqueue(AStarNode element)
	{
		int current_node = (int)(data.Count);
		int parent_node = get_parent_index(current_node);
		data.Add(element);
		
		while (!Equals(current_node, parent_node) && 
			data[current_node].nodeDistanceToStart < data[parent_node].nodeDistanceToStart)
		{
			swap(current_node, parent_node);
			current_node = parent_node;
			parent_node = get_parent_index(parent_node);
		}
	}
	public AStarNode get_top()
	{
		return data[0];
	}

	public AStarNode dequeue()
	{
		AStarNode return_value = data[0];
		swap(0, (int)(data.Count - 1));
		data.RemoveAt((data.Count - 1));
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
		AStarNode temp_index = data[index];
		data[index] = data[another_index];
		data[another_index] = temp_index;
	}

	public void move_down(int first)
	{
		int smallest = get_left_child_index(first);
		int last = (int)(data.Count - 1);
		while (smallest <= last)
		{
			if (smallest < last && data[smallest + 1].nodeDistanceToStart < data[smallest].nodeDistanceToStart)
			{
				++smallest;
			}

			if ((data[smallest].nodeDistanceToStart < data[first].nodeDistanceToStart) || 
				(!(data[smallest].nodeDistanceToStart < data[first].nodeDistanceToStart) && 
				(!(data[first].nodeDistanceToStart < data[smallest].nodeDistanceToStart))))
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


