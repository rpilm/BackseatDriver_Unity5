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
    public bool shortcutsEnabled = true;

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
        GameObject graphObj = GameObject.FindGameObjectWithTag("RoadGraph");
        if (graphObj != null)
            graph = graphObj.GetComponent<NodeGraph>();
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
		// reset all nodes to be off the path
		path = new Stack<NavNode>();
		foreach (NavNode n in graph.getAllNodes())
		{
			n.Reset();
		}
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
            followingPath = false;

            // set new destination
            graph.setRandomDestination();
            InitializePathfinding();

            return;
        }

        //check you're on the right track
        //TODO: actually implement this, like trigger an event and/or broadcast
        if (path.Peek() != currentNode)
        {
            if (path.Contains(currentNode) && shortcutsEnabled)
            {
                int counter = 0;
                //node is still in the path, they made a shortcut!
                //keep going
                while (path.Peek() != currentNode)
                {
                    counter++;
                    path.Pop();
                }
				Debug.Log("Took a shortcut over " + counter + " nodes to " + path.Peek());
            }
            else
            {
                Debug.LogWarning("WRONG WAY!!");
                Debug.Log("Expected " + currentNode + ", but got " + path.Peek());
                
				// rebuild a new path
				InitializePathfinding();
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
			Debug.Log("Next node in path is on " + nextNodeInPath.getRoadName() + ":" + nextNodeInPath);
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
                    float angleOfCar = Mathf.Atan2(transform.forward.z, transform.forward.x);
                    float angleOfPath = Mathf.Atan2(directionToNode.z, directionToNode.x);

                    float angleToNode = Mathf.Rad2Deg * (angleOfCar - angleOfPath);

                    Debug.Log("Angle: " + angleToNode);
                    if (Mathf.Abs(angleToNode) <= directionalTolerance)
                    {
                        // Go straight ahead
                        Debug.Log("CONTINUE STRAIGHT onto " + n.getRoadName() + "!!");
                    }
                    else if (Mathf.Abs(angleToNode - 180) <= directionalTolerance)
                    {
                        // U-turn
                        Debug.Log("MAKE A U-TURN onto " + n.getRoadName() + "!!");
                    }
                    else if (angleToNode > directionalTolerance)
                    {
                        //right side
                        Debug.Log("TURN RIGHT onto " + n.getRoadName() + "!!");
                    }
                    else // if (angleToNode > directionalTolerance)
                    {
                        //left side
                        Debug.Log("TURN LEFT onto " + n.getRoadName() + "!!");
                    }
                }
            }
        }
        


        
    }

    void FormPath()
    {
        /* Algorithm:
         * Use BFS to find the shortest path
         * NOTE: possible upgrade to djikstra's if edges are weighted by distance
         * First find destination node and explore toward the player location
         */

        //make dictionary to map visited status with each node
        Dictionary<NavNode, bool> visitDict = new Dictionary<NavNode, bool>();
		NavNode destination = null;
        foreach (NavNode n in graph.getAllNodes())
        {
            visitDict.Add(n, false);
			if (n.destination) // assuming only one destination at a time
			{
				destination = n;
			}
        }
		if (destination != null) 
		{
			currentNode.startPathfinding(path, gameObject.transform, destination, visitDict);
		}
        
    }

    void OnTriggerEnter(Collider c)
    {
        /* It's being called 5 times because of each collider on the car
         * such as the four wheels and the body are triggering this, so
         * detect if it is the parent */

        NavNode collidingNode = c.gameObject.GetComponent<NavNode>();
        if (collidingNode != null)
        {
            currentNode = collidingNode;
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
