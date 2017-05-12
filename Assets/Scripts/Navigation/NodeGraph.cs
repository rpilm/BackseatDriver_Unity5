using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/** Holds the graph data structure formed by street intersections,
 * Determines the graph dynamically */

public class NodeGraph : MonoBehaviour
{
    
    public bool drawWholeNetwork = false;

    private NavNode[] allNodes;
    private Intersection[] allIntersections;

    // Use this for initialization
    void Awake()
    {
        allNodes = GetComponentsInChildren<NavNode>();
        allIntersections = GetComponentsInChildren<Intersection>();
        Debug.Log(allNodes.Length + " nodes present");

        setRandomDestination();
    }

    public NavNode[] getAllNodes()
    {
        return allNodes;
    }

    public void setRandomDestination()
    {
        Debug.Log("Setting random destination...");
        foreach (NavNode i in allNodes)
        {
            i.destination = false;
        }
        // pick a random node to initialize to destination
        int randNode = Random.Range(0, allNodes.Length);
        allNodes[randNode].destination = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}