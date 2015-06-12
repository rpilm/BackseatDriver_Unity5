using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimpleCarController : MonoBehaviour 
{
    public List<AxleInfo> axleInfos;    //the info about each indiv axle
    public float maxMotorTorque;        //maximum torque the motor can apply
    public float maxSteeringAngle;      //max steer angle the wheel can have
    public float brakeMultiplier;       //how much more torque do the brakes do, scaled from maxMotorTorque
    public float maxSpeed;           //the fastest the car can go forward
    public float maxReverseVel;
    public Rigidbody rb;
    [HideInInspector]
    public float mpsToMph = 2.23694f;

    public void Start()
    {
        rb= GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(rb.centerOfMass.x, -1f, rb.centerOfMass.z);

        FirstPersonCamera fpCam = Resources.Load<FirstPersonCamera>("FirstPersonCam");
        fpCam.cameraHandle = transform.Find("FPCamHandle");
        Instantiate(fpCam, transform.position, Quaternion.identity);
    }
    public void FixedUpdate()
    {
        float motor = 0;
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

        Debug.DrawRay(transform.position, rb.velocity);

        //get the torque the motor will apply based off of user input
        motor = GetTorque();

        //for every wheel
        foreach(AxleInfo axleInfo in axleInfos)
        {
            if(axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if(axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
            //add visuals
            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);

        }
    }
    float GetTorque()
    {
        //holding left trigger - brake
        if (Input.GetAxisRaw("Controller-Gas/Brake") > .05)
        {
            return maxMotorTorque * Input.GetAxisRaw("Controller-Gas/Brake") * -1 * brakeMultiplier;
        }
        //holding right trigger - gas, but only if you're going less than your top speed
        else if (Input.GetAxisRaw("Controller-Gas/Brake") < -.05 && rb.velocity.magnitude * mpsToMph <= maxSpeed)
        {
            return maxMotorTorque * Input.GetAxisRaw("Controller-Gas/Brake") * -1;
        }

        //what a scrub, they must be using keyboard inputs

        //holding left trigger - brake
        else if (Input.GetAxisRaw("Keyboard-Gas/Brake") > .05)
        {
            return maxMotorTorque * Input.GetAxisRaw("Keyboard-Gas/Brake") * -1 * brakeMultiplier;
        }
        //holding right trigger - gas, but only if you're going less than your top speed
        else if (Input.GetAxisRaw("Keyboard-Gas/Brake") < -.05 && rb.velocity.magnitude * mpsToMph <= maxSpeed)
        {
            return maxMotorTorque * Input.GetAxisRaw("Keyboard-Gas/Brake") * -1;
        }
        else return 0;
    }
    //used to make the front wheels rotate
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0) return;
        
        Transform visualWheel = collider.transform.GetChild(0);
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }
    void OnGUI()
    {
        //the speed to display in MPH, floored to zero when it is insignificantly small
        float velmph = 2.23694f * rb.velocity.magnitude; if (velmph < .1f) velmph = 0;
            GUI.Label(new Rect(0, 0, 150, 100), "Speed: " + velmph + " MPH");

    }


}

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor; //is this wheel attached to motor?
    public bool steering;//does this wheel apply steer angle?
}
