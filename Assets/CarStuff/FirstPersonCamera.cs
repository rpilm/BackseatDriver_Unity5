using UnityEngine;
using System.Collections;

public class FirstPersonCamera : MonoBehaviour 
{
    public Transform cameraHandle;

    public float maxHeading;

    void Start()
    {
        transform.position = cameraHandle.position;
        transform.rotation = cameraHandle.rotation;
    }
    void Update()
    {
        //update
        transform.position = cameraHandle.position;
        
        //Lerp to direction of the camera toward the direction of velocity to simulate realistic head motion
        Vector3 carVel = cameraHandle.root.GetComponent<Rigidbody>().velocity;
        transform.forward = Vector3.Lerp(transform.forward, carVel, .5f);

        //if you're not already looking past the point where your head could turn
        if(Vector3.Angle(cameraHandle.forward,transform.forward) < maxHeading)
        {
            
        }
    }
}
