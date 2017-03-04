/* Shaun Yonkers
 * Theta* pathfinding
 * 
 * Graph class is used to read in the xml graph and translate the xml into a workable graph to find the shortest path
 *
 *
 * */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class Graph
{
    public int size;
    public char open;
    public char closed;

    public Vector3 start;
    public Vector3 end;

    public List <Node> nodes;
    public List<string> fullGraph = new List<string>(); //all 'ID' lines of the xml files stored here

    /* ******** Read Graph Function ***********
    *
    *Description: read the graph in from the xml file
    *
    *Inputs: fName: the filepath to the file
    *        isFile: bool check to see if the file exists
    */
    public void readGraph(string fName, bool isFile)
    {
        if (isFile)
        {
            using (XmlReader reader = XmlReader.Create(new StreamReader(fName)))
                parseXML(reader);
        }
        else {
            TextAsset data = Resources.Load(fName) as TextAsset;
            using (XmlReader reader = XmlReader.Create(new StringReader(data.text)))
                parseXML(reader);
        }
    }

    /* ******** Parse XML Function ***********
    *
    *Description: parse the xml file into a list 
    *
    *Inputs: reader: the xmlreader to get the information from the xml file
    */
    private void parseXML(XmlReader reader)
    {
        while (reader.Read())
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element:
                    switch (reader.Name)
                    {
                        case "City":
                            size = (int)System.Convert.ToDouble(reader.GetAttribute("Size"));

                            open = System.Convert.ToChar(reader.GetAttribute("Open"));
                            closed = System.Convert.ToChar(reader.GetAttribute("Closed"));

                            string temp = reader.GetAttribute("Start");
                            start = convertToVector(temp);

                            temp = reader.GetAttribute("End");
                            end = convertToVector(temp);

                            nodes = new List<Node>();
                            break;

                        case "Line":
                            int lineNumber = (int)System.Convert.ToDouble(reader.GetAttribute("Id")); // z value for node
                            int i = 0; //x value for node
                            string info = reader.ReadElementContentAsString();
                            fullGraph.Add(info);
							//find which spaces are open (.) in info string
                            foreach(char c in info)
                            {
                                if(c == open)
                                {
                                    Node newNode = new Node(new Vector3(i, 0, lineNumber));
                                    nodes.Add(newNode);
                                }
                                i++;
                            }
                            break;

                    }
                    break;
            }
        }
		//find the neighbors of each open node
        foreach(Node n in nodes)
        {
            getNeighbors(n);
        }
    }

	/* ******** Convert To Vector Function ***********
    *
    *Description: convert the string coordinates read from the xml file into a vector3
    *
    *Inputs: s is the string that contains the coordinates that are comma delimited
    */

    private Vector3 convertToVector(string s)
    {
        char[] delimiter = { ',' };
        string[] coordinates = s.Split(delimiter);
        float xValue = (float)System.Convert.ToDouble(coordinates[0]);
        float zvalue = (float)System.Convert.ToDouble(coordinates[1]);
        Vector3 newVector = new Vector3(xValue, 0, zvalue);
        return newVector;
    }

	/* ******** Get Neighbors Function ***********
    *
    *Description: find the neighbors of each node. Neighbors are considered open spaces that are one unit in the x or z direction
    *
    *Inputs: newNode - the node that we are finding the neighbors of.
    */
    private void getNeighbors(Node newNode)
    {
        foreach(Node n in nodes)
        {
            if(n.position == new Vector3(newNode.position.x + 1, newNode.position.y, newNode.position.z))
            {
                newNode.neighbors.Add(n);
            }
            if (n.position == new Vector3(newNode.position.x - 1, newNode.position.y, newNode.position.z))
            {
                newNode.neighbors.Add(n);
            }
            if (n.position == new Vector3(newNode.position.x, newNode.position.y, newNode.position.z + 1))
            {
                newNode.neighbors.Add(n);
            }
            if (n.position == new Vector3(newNode.position.x, newNode.position.y, newNode.position.z - 1))
            {
                newNode.neighbors.Add(n);
            }
        }
    }
}
