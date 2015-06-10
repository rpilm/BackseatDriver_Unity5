using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NavNode : MonoBehaviour {

    //modified by NodeGraph
    public HashSet<NavNode> neighbors;

    public void Initialize()
    {
        neighbors = new HashSet<NavNode>();
    }

	// Use this for initialization
	void Start () {
	    
	}

    void OnTriggerEnter(Collider other)
    {
        //check for neighbors
        
    }

    public void addNeighbor(NavNode n)
    {
        //is mutual
        neighbors.Add(n);
        n.neighbors.Add(this);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
