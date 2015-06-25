using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider))]
public class NavNode : MonoBehaviour {
    //triggered by the LateUpdate after the road formation
    public delegate void OnRoadsConnectedHandler();
    public static event OnRoadsConnectedHandler OnRoadsConnected;
    public static bool roadsConnected;

    public List<NavNode> neighbors;
    
    public bool destination = false;
    public int numNeighbors;
    public bool onPath = false;

    private NodeGraph nodeGraph;
    private Road parentRoad;
    private bool initialized = false;
    private bool isIntersection;
    
    //states to determine when to initialize pathfinding
    

    public void Initialize()
    {
        nodeGraph = GameObject.FindGameObjectWithTag("RoadGraph").GetComponent<NodeGraph>();
        neighbors = new List<NavNode>();
        parentRoad = transform.GetComponentInParent<Road>();
        initialized = true;
        numNeighbors = 0;
        roadsConnected = false;
        onPath = false;
        isIntersection = GetComponent<Intersection>();
    }

	// Use this for initialization
	void Start () {
        //ensure data structures and references initialized
        if (!initialized)
        {
            Initialize();
        }
	}

    void OnTriggerEnter(Collider c)
    {
        if (initialized)
        {
            //check for neighbors
            NavNode other = c.gameObject.GetComponent<NavNode>();
            if (other != null)
            {
                    //error if there are three overlapping road segments from same road
                if (parentRoad == other.parentRoad || other.isIntersection)
                {
                    if (neighbors.Count == 2)
                    {
                        Debug.LogError("ERROR: More than two Road Segments from " + parentRoad.getRoadName() + " overlapping");
                    }
                    else
                    {
                        addNeighborIndividual(other);
                    }
                
                }
                else if (isIntersection)
                {
                    addNeighborIndividual(other);
                }
            }
        }
    }

    public static Vector3 vecToNode(Transform tf, NavNode n)
    {
        return n.gameObject.transform.position - tf.position;
    }

    public string getRoadName()
    {
        if (parentRoad != null)
            return parentRoad.getRoadName();
        else
            return null;
    }

    public List<NavNode> getNeighbors()
    {
        return neighbors;
    }

    public void startPathfinding(Stack<NavNode> path, Transform tf,
        Dictionary<NavNode, bool> visitDict)
    {
        Debug.Log("Pathfinding...");
        foreach (NavNode n in neighbors)
        {
            //check each neighbor, use one that is less than 90 degrees in front of it
            Vector3 dir = NavNode.vecToNode(tf, n);
            if (Vector3.Dot(dir, tf.forward) > 0)
            {

                bool found = n.explore(path, visitDict);
                if (found)
                {
                    path.Push(n);

                }
            }
        }    
    }

    public bool explore(Stack<NavNode> path,  Dictionary<NavNode, bool> visitDict)
    {
        visitDict[this] = true;
        if (!destination)
        {
            foreach (NavNode n in neighbors)
            {
                if (!visitDict[n])
                {
                    bool found = n.explore(path, visitDict);
                    if (found)
                    {
                        path.Push(n);
                        onPath = true;
                        return true;
                    }
                }
            }
            return false;
        }
        else
        {
            return true;
        }
    }

    public void addNeighborMutual(NavNode n)
    {
        //is mutual
        neighbors.Add(n);
        n.neighbors.Add(this);

        numNeighbors++;
        n.numNeighbors++;
    }

    public void addNeighborIndividual(NavNode n)
    {
        neighbors.Add(n);
        numNeighbors++;
    }
	
	// Update is called once per frame
	void Update () {
        if (destination)
        {
            gameObject.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }

        //so you don't do it if they are not marked
        if (nodeGraph.debugWholeNetwork || onPath)
        {
            foreach (NavNode n in neighbors)
            {
                /* Colors:
                 * Red: on path
                 * Blue: (only when "debug whole network" 
                 * is marked in NodeGraph), connected 
                 */

                Color lineColor = Color.white;
                if (n.onPath )
                {
                    lineColor = Color.red;
                }
                else if (nodeGraph.debugWholeNetwork)
                {
                    lineColor = Color.blue;
                }
                //have to assign it to get around errors
                if (lineColor != Color.white)
                    Debug.DrawLine(transform.position, n.transform.position, lineColor);
            }
        }
	}

    void LateUpdate()
    {
        //should occur after all the linkages?
        if (!roadsConnected)
        {
            OnRoadsConnected();
            roadsConnected = true;
        }
    }
}