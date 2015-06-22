using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NavNode : MonoBehaviour {
    //triggered by the LateUpdate after the road formation
    public delegate void OnRoadsConnectedHandler();
    public static event OnRoadsConnectedHandler OnRoadsConnected;
    public static bool roadsConnected;

    public List<NavNode> neighbors;
    
    [HideInInspector]
    //maps link to a neighbor with the line renderer connecting them for debug purposes
    public Dictionary<NavNode, GameObject> lineDict;
    public bool destination = false;
    public int numNeighbors;
    public bool onPath = false;

    private NodeGraph nodeGraph;
    private Road parentRoad;
    private bool initialized = false;
    private bool isIntersection;

    public void Initialize()
    {
        nodeGraph = GameObject.FindGameObjectWithTag("RoadGraph").GetComponent<NodeGraph>();
        lineDict = new Dictionary<NavNode, GameObject>();
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
                if (parentRoad == other.parentRoad && !isIntersection)
                {
                    //error if there are three overlapping road segments from same road
                    if (neighbors.Count == 2)
                    {
                        Debug.LogError("ERROR: More than two Road Segments from " + parentRoad.getRoadName() + " overlapping");
                    }
                    GameObject newLine = Instantiate(nodeGraph.line) as GameObject;
                    addNeighborIndividual(other, newLine);
                }
                else if (isIntersection)
                {
                    GameObject newLine = Instantiate(nodeGraph.line) as GameObject;
                    addNeighborIndividual(other, newLine);
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
        foreach (NavNode n in neighbors)
        {
            //check each neighbor, use one that is less than 90 degrees in front of it
            Vector3 dir = NavNode.vecToNode(tf, n) ;
            if (Vector3.Dot(dir, tf.forward) > 0)
            {
                bool found = n.explore(path, visitDict);
                if (found)
                {
                    path.Push(n);
                    lineDict[n].GetComponent<LineRenderer>().SetColors(new Color(1f, 0f, 0f, 1f), new Color(1f, 0f, 0f, 1f));
                }
            }
        }
    }

    public bool explore(Stack<NavNode> path,  Dictionary<NavNode, bool> visitDict)
    {
        onPath = true;
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
                        lineDict[n].GetComponent<LineRenderer>().SetColors(new Color(1f, 0f, 0f, 1f), new Color(1f, 0f, 0f, 1f));
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

    public void addNeighborMutual(NavNode n, GameObject line)
    {
        //is mutual
        neighbors.Add(n);
        n.neighbors.Add(this);

        if(!lineDict.ContainsKey(n))
            lineDict.Add(n, line);
        if (!lineDict.ContainsKey(this))
            n.lineDict.Add(this, line);

        LineRenderer lr = line.GetComponent<LineRenderer>();
        lr.SetVertexCount(2);
        lr.SetPosition(0, gameObject.transform.position);
        lr.SetPosition(1, n.gameObject.transform.position);
        numNeighbors++;
        n.numNeighbors++;
    }

    public void addNeighborIndividual(NavNode n, GameObject line)
    {
        neighbors.Add(n);
        lineDict.Add(n, line);
        LineRenderer lr = line.GetComponent<LineRenderer>();
        lr.SetVertexCount(2);
        lr.SetPosition(0, gameObject.transform.position);
        lr.SetPosition(1, n.gameObject.transform.position);
        numNeighbors++;
    }
	
	// Update is called once per frame
	void Update () {
        if (destination)
        {
            gameObject.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
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
