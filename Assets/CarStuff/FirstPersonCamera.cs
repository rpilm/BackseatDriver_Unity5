using UnityEngine;
using System.Collections;

public class FirstPersonCamera : MonoBehaviour 
{
    public Transform cam;
    public SimpleCarController carController { get; set; }
    public float maxHeading;
    public float maxStickHeading;

    void Awake()
    {
        //assign a ref to our car controller
        carController = transform.root.GetComponent<SimpleCarController>();
        //instantiate our first person camera
        GameObject dummy = (GameObject)Resources.Load("FirstPersonCam");
        dummy = Instantiate(dummy, transform.position, transform.rotation) as GameObject;
        cam = dummy.transform;
    }
    void Update()
    {
        //update the cam's position and rotation based off our position and rotation
        cam.position = transform.position;
        cam.forward = transform.forward;

        //create the camera look direction based on velocity to simulate realistic head motion
        //then rotate that new vector by the horizontal input of the right stick to give players more look control
        Vector3 newCamDir = new Vector3();
        //only do this step if you are travelling forwards or fast enough backwards
        if (carController.speedInMph > 5 || carController.speedInMph < -15)
        {
            newCamDir = transform.root.GetComponent<Rigidbody>().velocity;
        }
        newCamDir = Quaternion.AngleAxis(maxStickHeading * Input.GetAxisRaw("2ndStickX"), Vector3.up) * newCamDir;
        //Lerp to direction of the camera toward the new direction
        transform.forward = Vector3.Lerp(transform.forward, newCamDir, Time.deltaTime);

        //if you're not already looking past the point where your head could turn
        if(Vector3.Angle(transform.forward, cam.forward) < maxHeading)
        {
            
        }
    }
}
