using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour 
{
    public void Awake()
    {
        Application.targetFrameRate = 60;
        Time.fixedDeltaTime = .01111111f;//physics updates 90 times a second for accurate car physics (no wobbling). oof X__x

        //this is needed to update horizontalAxisHeldFor
        StartCoroutine(StickHeldUpdater());
    }
    //returns the Input.GetAxis of axis name only it has been squared (but still retains the sign of the original value)
    public static float SquaredInput(string axisName)
    {
        float og = Input.GetAxisRaw(axisName);
        return Mathf.Sign(og) * og * og;
    }
    //returns the number of seconds the horizontal axis has been held in a direction for since it was last 0
    public static float horizontalAxisHeldFor;
    IEnumerator StickHeldUpdater()
    {
        while(true)
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > .15f)
                horizontalAxisHeldFor += Time.deltaTime;
            else
                horizontalAxisHeldFor = 0;

            yield return null;
        }
    }
}
