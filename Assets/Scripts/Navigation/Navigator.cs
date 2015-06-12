using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/** Navigator script.  Attach to the vehicle to
 * handle all the navigation functions.
 */

public class Navigator : MonoBehaviour {

    //FIFO to hold the current path for navigation
    //recalculated when driver is off the path
    Queue<NavNode> path;
    private string currentRoad;


	// Use this for initialization
	void Start () {
        path = new Queue<NavNode>();
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
    }

    void OnTriggerEnter(Collider c)
    {
        int roadLayer = LayerMask.NameToLayer("Road");
        if (c.gameObject.layer == roadLayer)
        {
            string n = c.gameObject.GetComponent<NavNode>().getRoadName();
            //if it's an intersection, it doesn't have a road name
            if (n != null)
            {
                currentRoad = n;
            }
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 150, 150, 100), "Road: " + currentRoad);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
