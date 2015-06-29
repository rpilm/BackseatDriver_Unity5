using UnityEngine;
using System.Collections;

public class ThirdPersonCameraHandle : MonoBehaviour 
{
    public Transform cam { get; set; }
    public SimpleCarController carController { get; set; }

    public float maxHeading;
    public float maxStickHeading;

	void Awake () 
    {
        //assign a ref to our car controller
        carController = transform.root.GetComponent<SimpleCarController>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
	}
	void Update () 
    {
        //every single line here was copied verbatim from the first person camera, except the last couple lines which only reflect
        //that this camera is a child of it's handle (this)as well as some rotation angle coefficients 
        //this third person camera isn't great, but works well for debugging

        //determine the signed angle between the car's forawrd direction and its velocity vector
        float sign = Mathf.Sign(transform.root.InverseTransformPoint(transform.root.position + carController.rb.velocity).x);
        float rotationAngle = sign * Vector3.Angle(transform.root.forward, carController.rb.velocity);//times 2 for greater weighting from the velocity
        //modify that angle
        rotationAngle *= Mathf.Clamp(BSDInput.horizontalAxisHeldFor / .5f, 0, 10) * carController.speedInMph/carController.maxSpeed; //angle the camera into the turn more based on how long the left stick is held
        rotationAngle += maxStickHeading * BSDInput.SquaredInput("2ndStickX"); //add the squared influence from the right stick independantly control the head
        rotationAngle = Mathf.Clamp(rotationAngle, -maxHeading, maxHeading); //clamp the rotation so the camera doesn't turn more than the heading
        //create a new direction vector that is rotated rotationAngle degrees from the car's forward direction
        Vector3 newCamDir = Quaternion.AngleAxis(rotationAngle, Vector3.up) * transform.root.forward;

        if (carController.speedInMph < 3)   //if the car is travelling backwards juts ignore everything we just did and look forward
            newCamDir = transform.root.forward;

        //now LERP our rotation towards our newCamDir 
        transform.forward = Vector3.Lerp(transform.forward, newCamDir, .25f);

	}

}
