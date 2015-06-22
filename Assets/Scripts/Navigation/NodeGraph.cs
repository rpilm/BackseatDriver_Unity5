using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/** Holds the graph data structure formed by street intersections,
 * Determines the graph dynamically */

[RequireComponent(typeof(BoxCollider))]
public class NodeGraph : MonoBehaviour {

    public GameObject line;

    NavNode[] allNodes;
    Intersection[] allIntersections;

	// Use this for initialization
	void Awake () {
        allNodes = GetComponentsInChildren<NavNode>();
        allIntersections = GetComponentsInChildren<Intersection>();
        Debug.Log(allNodes.Length + " nodes present");

	}

    public NavNode[] getAllNodes()
    {
        return allNodes;
    }


	// Update is called once per frame
	void Update () {
	    
	}
}
