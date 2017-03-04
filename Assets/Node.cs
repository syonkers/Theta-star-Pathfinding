/* Shaun Yonkers
 * Theta* pathfinding
 * 
 * Node class used to handle nodes in the XML graph
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node
{
    private Vector3 _position;
    private Node _parent;
    private float _cost;
    private List<Node> _neighbors;

    public Vector3 position
    {
        get
        {
            return _position;
        }
        set
        {
            _position = value;
        }
    }

    public Node parent
    {
        get
        {
            return _parent;
        }
        set
        {
            _parent = value;
        }
    }
    public List<Node> neighbors
    {
        get
        {
            return _neighbors;
        }
        set
        {
            _neighbors = value;
        }
    }
    public float cost
    {
        get
        {
            return _cost;
        }
        set
        {
            _cost = value;
        }
    }

    public Node(Vector3 where)
    {
        position = where;
        parent = null;
        neighbors = new List<Node>();
        cost = 0;
    }
}
