using UnityEngine;
using System.Collections;

public class BSDInput
{
    public static float Horizontal
    {
        get
        {
            return Input.GetAxis("Steering");
        }
        set { }
    }
    public static float Gas_Brake
    {
        get
        {
            return Input.GetAxis("Acceleration");
        }
        set { }
    }
    public static bool AButton
    {
        get
        {
            return false;//if(Input.GetKeyDown)
        }
        set { }
    }

    //returns the Input.GetAxis of axis name only it has been squared (but still retains the sign of the original value)
    //this function also has "smart" use of horizontal and gas_brake
    public static float SquaredInput(string axisName)
    {
        //special cases where we don't know if a controller or keyboard is being used
        if(axisName == "Horizontal")
        {
            axisName = "Steering";
        }
        else if (axisName == "Gas_Brake")
        {
            axisName = "Acceleration";
        }

        //okay now just calculate the signed squared input
        float og = Input.GetAxisRaw(axisName);
        return Mathf.Sign(og) * og * og;
    }

    //returns the number of seconds the horizontal axis has been held in a direction for since it was last 0
    public static float horizontalAxisHeldFor;
    public static float horizontalAxisNotHeldFor;
    public static IEnumerator StickHeldUpdater()
    {
        while (true)
        {
            if (Mathf.Abs(BSDInput.Horizontal) > .15f)
            {
                horizontalAxisHeldFor += Time.deltaTime;
                horizontalAxisNotHeldFor = 0;
            }
            else
            {
                horizontalAxisHeldFor = 0;
                horizontalAxisNotHeldFor += Time.deltaTime;
            }
            yield return null;
        }
    }
}
