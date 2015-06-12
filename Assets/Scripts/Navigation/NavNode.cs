using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NavNode : MonoBehaviour {

    //modified by NodeGraph
    public HashSet<NavNode> neighbors;
    
    [HideInInspector]
    //maps link to a neighbor with the line renderer connecting them for debug purposes
    public Dictionary<NavNode, GameObject> lineDict;

    public bool isDest = false;

    private Road parentRoad;

    public void Initialize()
    {
        lineDict = new Dictionary<NavNode, GameObject>();
        neighbors = new HashSet<NavNode>();
        parentRoad = transform.GetComponentInParent<Road>();
    }

	// Use this for initialization
	void Start () {
	    
	}

    void OnTriggerEnter(Collider other)
    {
        //check for neighbors
        
    }

    public string getRoadName()
    {
        if (parentRoad != null)
            return parentRoad.name;
        else
            return null;
    }

    public void addNeighbor(NavNode n, GameObject line)
    {
        //is mutual
        neighbors.Add(n);
        n.neighbors.Add(this);

        lineDict.Add(n, line);
        n.lineDict.Add(this, line);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
