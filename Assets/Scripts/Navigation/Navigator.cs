using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/** Navigator script.  Attach to the vehicle to
 * handle all the navigation functions.
 */

[RequireComponent(typeof(Collider))]
public class Navigator : MonoBehaviour {

    //defines wiggle room for considering a road "straight ahead"
    public float directionalTolerance = 10f;


    //FIFO to hold the current path for navigation
    //recalculated when driver is off the path
    public Stack<NavNode> path;
    private NodeGraph graph;
    private string currentRoadName;
    private NavNode currentNode;
    private bool followingPath;

    //the next anticipated node in the path
    private NavNode plannedNode;


	// Use this for initialization
	void Awake () {
        path = new Stack<NavNode>();
        graph = GameObject.FindGameObjectWithTag("RoadGraph").GetComponent<NodeGraph>();
        followingPath = false;

        //subscribe to road completion event
        NavNode.OnRoadsConnected += new NavNode.OnRoadsConnectedHandler(OnRoadsConnected);
	}

    void OnRoadsConnected()
    {
        InitializePathfinding();
    }


    void InitializePathfinding()
    {
        FormPath();
        followingPath = true;
    }

    void Navigate()
    {
        
        if (currentNode.destination)
        {
            //finish pathfinding, unmark as destination
            //should behave like this even if the destination is an intersection
            Debug.Log("Found destination!  Xièxie!");
            currentNode.destination = false;
            return;
        }

        //check you're on the right track
        //TODO: actually implement this, like trigger an event and/or broadcast
        if (path.Peek() != currentNode)
        {
            if (path.Contains(currentNode))
            {
                int counter = 0;
                //node is still in the path, they made a shortcut!
                //keep going
                while (path.Peek() != currentNode)
                {
                    counter++;
                    path.Pop();
                }
                Debug.Log("Took a shortcut over " + counter + " nodes");
            }
            else
            {
                Debug.Log("WRONG WAY!!");
                Debug.Log("Anticipated: " + path.Peek() + " Actual: " + currentNode);
                return;
            }
        }

        path.Pop();
        NavNode nextNodeInPath = path.Peek();
        if (nextNodeInPath.GetComponent<Intersection>())
        {
            Debug.Log("Next node is an intersection, pay attention!!");
        }
        else
        {
            Debug.Log("Next node in path is on " + nextNodeInPath.getRoadName());
        }

        if (currentNode.GetComponent<Intersection>())
        {
            //figure out which way you're going
            foreach (NavNode n in currentNode.getNeighbors())
            {
                //if the next node is an intersection, start telling them where to go
                if (n == nextNodeInPath)
                {
                    Vector3 directionToNode = NavNode.vecToNode(transform, n).normalized;
                    float angleToNode = Mathf.Rad2Deg * Mathf.Acos(Vector3.Dot(transform.right, directionToNode));
                    Debug.Log("Angle " + angleToNode);
                    if (angleToNode < 90 - directionalTolerance)
                    {
                        //right side
                        Debug.Log("TURN RIGHT onto " + n.getRoadName() + "!!");
                    }
                    else if (angleToNode > 90 + directionalTolerance)
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

    void OnTriggerEnter(Collider c)
    {
        /* It's being called 5 times because of each collider on the car
         * such as the four wheels and the body are triggering this, so
         * detect if it is the parent */

        NavNode collidingNode = c.gameObject.GetComponent<NavNode>();
        if (collidingNode != null)
        {
            currentNode = c.gameObject.GetComponent<NavNode>();
            if (followingPath)
                Navigate();
            string n = currentNode.getRoadName();
            if (n != null)
            {
                currentRoadName = n;
            }
        }
    }

    void OnTriggerExit(Collider c)
    {
        //currentRoadName = "N/A";
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 150, 150, 100), "Road: " + currentRoadName);
    }
	
	// Update is called once per frame
	void Update () {

    }
}
