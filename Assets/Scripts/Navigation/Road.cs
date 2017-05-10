using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Road : MonoBehaviour {

    [SerializeField]        //so it's editable in Inspector, but not mutable in code
    private string roadName;

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

    public string getRoadName()
    {
        return roadName;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
