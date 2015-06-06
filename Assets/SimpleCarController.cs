using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimpleCarController : MonoBehaviour 
{
    public List<AxleInfo> axleInfos; //the info about each indiv axle
    public float maxMotorTorque;//maximum torque the motor can apply
    public float maxSteeringAngle;//max steer angle the wheel can have
    
    public void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(rb.centerOfMass.x, -1f, rb.centerOfMass.z);
    }
    public void FixedUpdate()
    {
        float motor = maxMotorTorque * Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

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
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0) return;
        
        Transform visualWheel = collider.transform.GetChild(0);
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;

        //now rotate the wheels 90 degrees, because the cylinder primitives don't have the correct rotation
        //visualWheel.transform.Rotate(0f, 0f, 90f);

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
