using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/** Navigator script.  Attach to the vehicle to
 * handle all the navigation functions.
 */

public class Navigator : MonoBehaviour {

    //defines wiggle room for considering a road "straight ahead"
    public float directionalTolerance = 10f;


    //FIFO to hold the current path for navigation
    //recalculated when driver is off the path
    private Stack<NavNode> path;
    private NodeGraph graph;
    private string currentRoadName;
    private NavNode currentNode;
    private bool followingPath;


	// Use this for initialization
	void Start () {
        path = new Stack<NavNode>();
        graph = GameObject.FindGameObjectWithTag("RoadGraph").GetComponent<NodeGraph>();
        followingPath = false;

        //subscribe to road completion event
        NavNode.OnRoadsConnected += new NavNode.OnRoadsConnectedHandler(OnRoadsConnected);
	}

    void OnRoadsConnected()
    {
        PerformPathfinding();
    }


    void PerformPathfinding()
    {
        if (!followingPath)
        {
            FormPath();
            followingPath = true;
        }
        else
        {
            if (currentNode.GetComponent<Intersection>())
            {
                Debug.Log("Checking stack");
                NavNode nextNodeInPath = path.Peek();
                Debug.Log("Next road is " + nextNodeInPath.getRoadName());
                //figure out which way you're going
                foreach (NavNode n in currentNode.getNeighbors())
                {
                    //if the next node is an intersection, start telling them where to go
                    if (n == nextNodeInPath)
                    {
                        Vector3 directionToNode = NavNode.vecToNode(transform, n).normalized;
                        float dottedAngle = Mathf.Rad2Deg*Mathf.Acos(Vector3.Dot(transform.right, directionToNode));
                        Debug.Log("Angle " + dottedAngle);
                        if (dottedAngle < 90-directionalTolerance)
                        {
                            //right side
                            Debug.Log("TURN RIGHT onto " + n.getRoadName() + "!!");
                        }
                        else if (dottedAngle > 90+directionalTolerance)
                        {
                            //left side
                            Debug.Log("TURN LEFT onto " + n.getRoadName() + "!!");
                        }
                        else
                        {
                            //straight
                            Debug.Log("GO STRAIGHT onto " + n.getRoadName() + "!!");
                        }
                    }
                }
            }
        }
    }

    void FormPath()
    {
        /* Algorithm:
         * Basic DFS (because not having it be the shortest path might be funny)
         * For the NavNode that the car is currently in:
         *      do a dot product to ensure the node is in front of the player
         *      mark node as visited
         *      check if destination, if not, continue
         * when search is done, mark all as unvisited
         */

        //make dictionary to map visited status with each node
        Dictionary<NavNode, bool> visitDict = new Dictionary<NavNode, bool>();
        foreach (NavNode n in graph.getAllNodes())
        {
            visitDict.Add(n, false);
        }
        currentNode.startPathfinding(path, gameObject.transform, visitDict);
        
    }

    void OnTriggerStay(Collider c)
    {
        int roadLayer = LayerMask.NameToLayer("Road");
        if (c.gameObject.layer == roadLayer)
        {
            NavNode nextNode = c.gameObject.GetComponent<NavNode>();
            if (followingPath)
            {
                /*if (nextNode == path.Peek())
                {
                    path.Pop();
                }*/
            }
            currentNode = nextNode;
            string n = currentNode.getRoadName();
            //if it's an intersection, it doesn't have a road name
            if (n != null)
            {
                currentRoadName = n;
            }
        }
    }

    void OnTriggerExit(Collider c)
    {
        currentRoadName = "N/A";
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 150, 150, 100), "Road: " + currentRoadName);
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(GameControl.SquaredInput("Keyboard-Gas/Brake"));
	}
}
