using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/** Holds the graph data structure formed by street intersections,
 * Determines the graph dynamically */

public class NodeGraph : MonoBehaviour {


    //public PathfindingInitializer pathfindingInitializer;

    public GameObject line;
    private NavNode[] allNodes;
    private Intersection[] allIntersections;

	// Use this for initialization
	void Awake () {
        allNodes = GetComponentsInChildren<NavNode>();
        allIntersections = GetComponentsInChildren<Intersection>();
        Debug.Log(allNodes.Length + " nodes present");
        //pathfindingInitializer = new PathfindingInitializer();
	}

    public NavNode[] getAllNodes()
    {
        return allNodes;
    }

    /*public PathfindingInitializer getPathfindingInitializer(){
        return pathfindingInitializer;
    }*/

	// Update is called once per frame
	void Update () {

	}
}


/*public class PathfindingInitializer
{
    private Dictionary<NavNode, bool> nodeMap;
    private int numInitializedNodes;
    private bool allNodesInitialized = false;

    public PathfindingInitializer()
    {
        nodeMap = new Dictionary<NavNode, bool>();
    }

    public void registerNode(NavNode n)
    {
        nodeMap.Add(n, false);
    }

    public bool nodeMarked(NavNode n)
    {
        return nodeMap[n];
    }

    public int getNumInitializedNodes()
    {
        return numInitializedNodes;
    }

    public void markRoadChecked(NavNode n)
    {
        if (!nodeMap[n])
        {
            numInitializedNodes++;
            nodeMap[n] = true;
            if (numInitializedNodes == nodeMap.Keys.Count)
            {
                allNodesInitialized = true;
            }
        }
    }

    public bool areAllNodesInitialized()
    {
        return allNodesInitialized;
    }
}*/