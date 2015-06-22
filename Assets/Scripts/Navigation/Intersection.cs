using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavNode))]
public class Intersection : MonoBehaviour {

    private NavNode attachedNode;
    private NodeGraph nodeGraph;

	// Use this for initialization
	void Start () {
        attachedNode = GetComponent<NavNode>();
        nodeGraph = GameObject.FindGameObjectWithTag("RoadGraph").GetComponent<NodeGraph>();
	}

    /*void OnTriggerEnter(Collider c)
    {
        //check for neighbors
        NavNode other = c.gameObject.GetComponent<NavNode>();
        if (other != null)
        {
            attachedNode.addNeighborIndividual(other, Instantiate(nodeGraph.line) as GameObject);
        }
    }*/

	// Update is called once per frame
	void Update () {
	
	}
}
