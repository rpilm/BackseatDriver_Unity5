using UnityEngine;
using System.Collections;

/** Holds the graph data structure formed by street intersections,
 * Determines the graph dynamically */

public class NodeGraph : MonoBehaviour {

    public GameObject line;

    NavNode[] allNodes;
    Intersection[] allIntersections;

	// Use this for initialization
	void Awake () {
        allNodes = GetComponentsInChildren<NavNode>();
        allIntersections = GetComponentsInChildren<Intersection>();
        Debug.Log(allNodes.Length);
        GenerateGraph();
	}

    void GenerateGraph()
    {
        foreach (NavNode node in allNodes)
        {
            node.Initialize();
        }

        foreach (NavNode node1 in allNodes)
        {
            foreach (NavNode node2 in allNodes)
            {
                if (node1 != node2 
                    && node1.gameObject.GetComponent<BoxCollider>().bounds.Intersects(node2.gameObject.GetComponent<BoxCollider>().bounds)
                    && !node1.neighbors.Contains(node2))
                {

                    //line to denote linked roads
                    //debug use only
                    GameObject link;
                    LineRenderer lr;
                    

                    /*if two nodes are not on the same street, there must be an intersection somewhere
                     * make sure that neither node is an intersection to start with, they should form regular connections automatically
                     */
                    if (node1.gameObject.transform.parent == node2.gameObject.transform.parent)
                    {
                        link = Instantiate(line) as GameObject;
                        lr = link.GetComponent<LineRenderer>();
                        node1.addNeighbor(node2, link);
                        lr.SetVertexCount(2);
                        lr.SetPosition(0, node1.gameObject.transform.position);
                        lr.SetPosition(1, node2.gameObject.transform.position);
                    }
                    else if (node1.gameObject.GetComponent<Intersection>() || node2.gameObject.GetComponent<Intersection>())
                    {
                        link = Instantiate(line) as GameObject;
                        lr = link.GetComponent<LineRenderer>();
                        node1.addNeighbor(node2, link);
                        lr.SetVertexCount(2);
                        lr.SetPosition(0, node1.gameObject.transform.position);
                        lr.SetPosition(1, node2.gameObject.transform.position);
                    }
                }
            }
        }

        Debug.Log("Graph generated");
    }


	// Update is called once per frame
	void Update () {
	    
	}
}
