using UnityEngine;
using System.Collections;

public class FirstPersonCamera : MonoBehaviour 
{
    public Transform cam { get; set; }
    public SimpleCarController carController { get; set; }
    public float maxHeading;
    public float maxStickHeading;

    void Awake()
    {
        //assign a ref to our car controller
        carController = transform.root.GetComponent<SimpleCarController>();
        //instantiate our first person camera
        GameObject dummy = (GameObject)Resources.Load("TheMainCamera");
        dummy = Instantiate(dummy, transform.position, transform.rotation) as GameObject;
        cam = dummy.transform;
    }
    void Update()
    {
        //the first person camera follows the rotation and position of the FPCamHandle (this transform) but is not a child so it's rotation is independant
        //the camera's look will LERP toward the direction of the car's velocity over time based on how long the left stick is held in that direction
        //the camera look can also be freely influenced by the right stick to allow the player to look around independantly

        //determine the signed angle between the car's forawrd direction and its velocity vector
        float sign = Mathf.Sign(transform.root.InverseTransformPoint(transform.root.position + carController.rb.velocity).x);
        float rotationAngle = sign * Vector3.Angle(transform.root.forward, carController.rb.velocity);
        //modify that angle
        rotationAngle *= Mathf.Clamp(GameControl.horizontalAxisHeldFor / 1f, 0, 1); //angle the camera into the turn more based on how long the left stick is held
        rotationAngle += maxStickHeading * GameControl.SquaredInput("2ndStickX"); //add the squared influence from the right stick independantly control the head
        rotationAngle = Mathf.Clamp(rotationAngle, -maxHeading, maxHeading); //clamp the rotation so the camera doesn't turn more than the heading
        //create a new direction vector that is rotated rotationAngle degrees from the car's forward direction
        Vector3 newCamDir = Quaternion.AngleAxis(rotationAngle, Vector3.up) * transform.root.forward;

        if (carController.speedInMph < 0)   //if the car is travelling backwards juts ignore everything we just did and look forward
            newCamDir = transform.root.forward;

        //now LERP our rotation towards our newCamDir 
        transform.forward = Vector3.Lerp(transform.forward, newCamDir, .25f);

        //update the cam's position and rotation based off our position and rotation
        cam.position = transform.position;
        cam.forward = transform.forward;

        //draw vectors for debugging
        Debug.DrawRay(transform.position, newCamDir, Color.red);
        Debug.DrawRay(transform.position, carController.rb.velocity, Color.magenta);
        Debug.DrawRay(transform.position, transform.root.forward, Color.yellow);
        Debug.DrawRay(transform.position, transform.forward, Color.cyan);
    }
}
