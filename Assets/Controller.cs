/* Shaun Yonkers
 * Theta* pathfinding
 * 
 * Main script that creates the map and draws the path from start to end. Yellow nodes and edges are apart of the shortest path through the city.
 * */


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class Controller : MonoBehaviour
{

    Graph g = new Graph();

    private PriorityQueue openList;
	private List<Node> closedList;

    public Transform cube;
    public Transform sphere;
	public Transform connector;

    private Transform clone;

    // Use this for initialization
    void Start()
    {
        drawGraph();
        findPath();
    }


    /* ********  Draw Graph Function *********
    *
    * Description: reads in the xml city file and draws the city by instantiating the cube transform
    *               for closed spaces.
    ******************************************/
    private void drawGraph()
	{
		string filename = Application.dataPath + "/City.xml";
        g.readGraph(filename, File.Exists(filename));

		int x = 0;
		int z = 0;

        //create all the closed spaces as the cube prefab
        foreach (string s in g.fullGraph)
        {
            foreach (char c in s)
            {
                if (c == g.closed)
                {
                    clone = (Transform)Instantiate(cube, new Vector3(x, 0, z), Quaternion.identity);
                }
                x++;
            }
            x = 0;
            z++;
        }
    }


    /*  ***** find A* Path Function ******
    *
    * Description: find the shortest path between the start and end nodes created from the graph using the A* algorithm 
    *
    *
    ***************************************/

    private void findPath()
    {
        openList = new PriorityQueue();
        closedList = new List<Node>();
        Node current;

		//find and add ending node to open list (we are working backwards)
        foreach ( Node n in g.nodes)
        {
            if (n.position == g.end)
            {
                n.cost = 0;
                n.parent = n;
                openList.Enqueue(n);
            }
        }

        //while we havent found the shortest path
        while (!openList.isEmpty())
        {
            //find node with lowest cost in open list and add to closed & remove from open list
            current = openList.Dequeue();
			closedList.Add(current);

            //if we found our way back to the starting node
            if (current.position == g.start)
            {
                drawPath(current);
            }
			else
			{
            	foreach (Node nbr in current.neighbors)
            	{
                	if (!closedList.Contains(nbr))
                	{
                    	if (!openList.Contains(nbr))
                    	{
                        	nbr.cost = float.PositiveInfinity;
                        	nbr.parent = null;
                    	}
                    	updateVertex(current, nbr);
                	}
            	}
        	}
    	}
	}

	/*  ***** Update Vertex Function ******
    *
    * Description: Updates the priority queue with new costs, if smaller, to neighbors of current node
    *
    * Inputs: current - the current node that we are examining
    * 			  nbr - the neighbor we are computing the cost to get to
    *
    ***************************************/
    private void updateVertex(Node current, Node nbr)
    {
        float oldCost = nbr.cost;
        computeCost(current, nbr);
        if (nbr.cost < oldCost)
        {
            if (openList.Contains(nbr)){
                openList.Remove(nbr);
            }
            openList.Enqueue(nbr);
        }
    }

	/*  ***** Compute Cost Function ******
    *
    * Description: Compute the cost to get to the neighbor node
    * 
    * Inputs: current - the current node that we are examining
    * 			  nbr - the neighbor we are computing the cost to get to
    *
    ***************************************/
    private void computeCost(Node current, Node nbr)
    {
        float localCost = getLocalCost(current, nbr);
		//if the line cast doesnt hit any of the closed spaces from the parent of current to currents neighbor(nbr)
        if (!Physics.Linecast(current.parent.position, nbr.position))
        {
            if ((current.cost + localCost) < nbr.cost)
            {
                nbr.parent = current.parent;
                nbr.cost = current.cost + localCost;
            }
        }
        else
        {
            if ((current.cost + localCost) < nbr.cost)
            {
                nbr.parent = current;
                nbr.cost = current.cost + localCost;
            }
        }

    }

	/*  ***** Get Local Cost Function ******
    *
    * Description: Returns the distance between the current node and its neighbor
    * 
    * Inputs: current - the current node that we are examining
    * 			  nbr - the neighbor node that we are getting the distance to
    *
    ***************************************/
    private float getLocalCost(Node current, Node nbr)
    {
        return Vector3.Distance(current.position, nbr.position);
    }
		

    /*  ************** Draw Path Function ********************
    *
    *   Description: Changes the color of all nodes and edges from the ending node to the starting node
    *               to the color yellow to indicate the shortest path
    *
    *   Inputs: currentNode: the node found the be the end node
    *
    ***********************************************/

	private void drawPath(Node currentNode)
    {
        //while we haven't got back to the starting node
        while (currentNode.position != g.end)
        {
			Node parentnode = currentNode.parent;
			float temp = Vector3.Distance (currentNode.position, parentnode.position) / 2;

			clone = (Transform)Instantiate(sphere,currentNode.position,Quaternion.identity);
			if (currentNode.position == g.start) 
			{
				clone.GetComponent<MeshRenderer> ().material.color = Color.blue;
			}
			clone = (Transform)Instantiate (connector, new Vector3 (0, 0, 0), Quaternion.identity);

			clone.localScale = new Vector3 (clone.localScale.x, temp, clone.localScale.z);
			clone.position = Vector3.Lerp (currentNode.position, parentnode.position, 0.5f);
			clone.transform.up = parentnode.position - currentNode.position;

            currentNode = parentnode;
        }
		clone = (Transform)Instantiate(sphere,currentNode.position,Quaternion.identity);
		clone.GetComponent<MeshRenderer> ().material.color = Color.red;
    }
}
