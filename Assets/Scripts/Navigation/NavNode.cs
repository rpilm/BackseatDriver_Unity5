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
    public NavNode nextInPath;

	private NavNode parentInPath;
    private NodeGraph nodeGraph;
    private Road parentRoad;
    private bool initialized = false;
    private bool isIntersection;


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
        if (OnRoadsConnected != null)
        {
            OnRoadsConnected = new OnRoadsConnectedHandler(OnRoadsConnected);
        }
    }

	public void Reset()
	{
		onPath = false;
		nextInPath = null;
		parentInPath = null;
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
        NavNode dest, Dictionary<NavNode, bool> visitDict)
    {
        Debug.Log("Pathfinding...Starting node is " + gameObject);

        foreach (NavNode n in neighbors)
        {
            //check each neighbor, use one that is less than 90 degrees in front of it
            Vector3 dir = NavNode.vecToNode(tf, n);
            if (Vector3.Dot(dir, tf.forward) > 0)
            {
				bool found = n.explore(visitDict);

				if (found)
				{
					onPath = true;
					nextInPath = n;

					NavNode p = dest;
					while (p != n)
					{
						p.onPath = true;
						path.Push(p);
						p.parentInPath.nextInPath = p;

						p = p.parentInPath;
					}
					p.onPath = true;
					path.Push(p);

					// only do this for one node
					return;
				}
            }
        }
    }

    public bool explore(Dictionary<NavNode, bool> visitDict)
    {
		Queue<NavNode> toExplore = new Queue<NavNode>();
		toExplore.Enqueue(this);
		visitDict[this] = true;

		while (toExplore.Count > 0)
		{
			NavNode cur = toExplore.Dequeue();
			if (cur.destination)
			{
				cur.onPath = true;
				cur.nextInPath = cur.parentInPath;
				return true;
			}

			foreach (NavNode n in cur.neighbors)
			{
				if (!visitDict[n])
				{
					visitDict[n] = true;
					n.parentInPath = cur;
					toExplore.Enqueue(n);
				}
			}
		}

		return false;
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
        MeshRenderer mr = GetComponent<MeshRenderer>();
        if (mr.enabled)
        {
            if (destination)
            {
                gameObject.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
            }
            else
            {
                gameObject.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
        }

        //so you don't do it if they are not marked
        if (onPath && nextInPath != null)
        {
			Vector3 direction = nextInPath.transform.position - transform.position;


			Debug.DrawLine(transform.position, 0.75f*direction+transform.position, Color.red);
        }
        else if (nodeGraph.drawWholeNetwork)
        {
            foreach (NavNode n in neighbors)
            {
                if (!n.onPath)
                    Debug.DrawLine(transform.position, n.transform.position, Color.blue);
            }
        }
	}

    void LateUpdate()
    {
        //should occur after all the linkages?
        if (!roadsConnected && OnRoadsConnected != null)
        {
            OnRoadsConnected();
            roadsConnected = true;
        }
    }
}