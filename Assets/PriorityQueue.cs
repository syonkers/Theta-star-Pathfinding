/* Shaun Yonkers
 * Theta* pathfinding
 * 
 * Priority Queue class is used to store the nodes that are in the open list of the Theta star implementation
 *
 *
 * */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PriorityQueue {
    List<Node> heap;

    public PriorityQueue()
    {
        heap = new List<Node>();
    }

	/* ******** Enqueue Function ***********
    *
    *Description: Add node to prioity queue
    *
    *Inputs: newNode: the node to be inserted into the PQ
    */
    public void Enqueue (Node newNode)
    {
        heap.Add(newNode);
        int i = heap.Count - 1;
        while (i != 0)
        {
            int p = (i - 1) / 2;
            if (heap[p].cost > heap[i].cost)
            {
                Swap(i, p);
                i = p;
            }
            else
                return;
        }
    }

	/* ******** Dequeue Function ***********
    *
    *Description: Remove first node (the priority) in prioity queue
    *
    *Return: Returns the node that was removed
    */
    public Node Dequeue()
    {
		int i = heap.Count - 1;
        Node first = heap[0];
        heap[0] = heap[i];
        heap.RemoveAt(i);
		--i;
		int p = 0;
	
		while (true) 
		{
			int left = p * 2 + 1;
			if (left > i)
				break;
			int right = left + 1;
			if ((right < p) && (heap[right].cost < heap[left].cost))
				left = right;
			if (heap[p].cost <= heap[left].cost)
				break;
			Swap (p, left);
			p = left;
		}
		return first;
	}

	/* ******** Contains Function ***********
    *
    *Description: Return whether the specified node is in the priority queue
    *
    */

    public bool Contains(Node n)
    {
        return heap.Contains(n);
    }

	/* ******** IsEmpty Function ***********
    *
    *Description: Return whether the priority queue is empty
    *
    */
    public bool isEmpty()
    {
        return heap.Count == 0;
    }

	/* ******** Remove Function ***********
    *
    *Description: Remove specified node in the priority queue
    *
    */
    public void Remove(Node n)
    {
        heap.Remove(n);
    }

	/* ******** Swap Function ***********
    *
    *Description: Swap places of the two node passed in
    *
    *Input: a & b are the indexes of the two nodes wanting to be swapped
    */
    private void Swap (int a, int b)
    {
        Node temp = heap[a];
        heap[a] = heap[b];
        heap[b] = temp;
    }
}