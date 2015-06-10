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
        /* Algorithm:
         * if two nodes overlap:
         *      check if they are on the same road
         *      if so, give them a link
         *      if not, find the shared intersection, and have them both connect to the intersection
         *          if no intersection found, throw error
         */

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
                    GameObject link = Instantiate(line) as GameObject;
                    LineRenderer lr = link.GetComponent<LineRenderer>();
                    

                    /*if two nodes are not on the same street, there must be an intersection somewhere
                     * make sure that neither node is an intersection to start with, they should form regular connections automatically
                     */
                    if (node1.gameObject.transform.parent != node2.gameObject.transform.parent 
                        && !node1.gameObject.GetComponent<Intersection>()
                        && !node2.gameObject.GetComponent<Intersection>())
                    {
                        bool found = false;
                        //find their shared intersection
                        foreach (Intersection i in allIntersections)
                        {
                            if (node1.gameObject.GetComponent<BoxCollider>().bounds.Intersects(i.gameObject.GetComponent<BoxCollider>().bounds) &&
                                node2.gameObject.GetComponent<BoxCollider>().bounds.Intersects(i.gameObject.GetComponent<BoxCollider>().bounds))
                            {
                                NavNode iNav = i.gameObject.GetComponent<NavNode>();
                                iNav.addNeighbor(node1);
                                iNav.addNeighbor(node2);
                                found = true;

                                //debug coloring
                                MeshRenderer mr = iNav.gameObject.GetComponent<MeshRenderer>();
                                mr.material.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);

                                lr.SetVertexCount(3);
                                lr.SetPosition(0, node1.gameObject.transform.position);
                                lr.SetPosition(1, iNav.gameObject.transform.position);
                                lr.SetPosition(2, node2.gameObject.transform.position);
                                break;
                            }
                        }
                        if (!found)
                        {
                            Debug.LogError("Could not find intersection of nodes.");
                        }
                    }
                    else
                    {
                        node1.addNeighbor(node2);
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
