using UnityEngine;
using System.Collections;

public class BSDInput
{
    public static float Horizontal
    {
        get{
            if (Input.GetAxisRaw("Horizontal-Keyboard") != 0)
                return Input.GetAxis("Horizontal-Keyboard");
            else return Input.GetAxisRaw("Horizontal-Controller"); 
        }set { }
    }
    public static float Gas_Brake
    {
        get
        {
            if (Input.GetAxisRaw("Gas_Brake-Keyboard") != 0)
                return Input.GetAxis("Gas_Brake-Keyboard");
            else return Input.GetAxisRaw("Gas_Brake-Controller");
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
            if (Input.GetAxisRaw("Horizontal-Keyboard") != 0)
                axisName = "Horizontal-Keyboard";
            else axisName = "Horizontal-Controller";
        }
        else if (axisName == "Gas_Brake")
        {
            if (Input.GetAxisRaw("Gas_Brake-Keyboard") != 0)
                axisName = "Gas_Brake-Keyboard";
            else axisName = "Gas_Brake-Controller";
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
