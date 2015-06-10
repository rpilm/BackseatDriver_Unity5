using UnityEngine;
using System.Collections;

public class Road : MonoBehaviour {

    public string name;

    private NavNode[] roadNodes;

	// Use this for initialization
	void Start () {
       roadNodes = GetComponentsInChildren<NavNode>();
	}

    //for quick access to node data
    NavNode getNavNode(GameObject node)
    {
        NavNode n = node.GetComponent<NavNode>();
        if (!n)
        {
            Debug.LogError("GameObject " + node.name + " does not contain a NavNode");
        }
        return n;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
