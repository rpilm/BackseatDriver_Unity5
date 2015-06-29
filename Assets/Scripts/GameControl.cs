using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour 
{
    public void Awake()
    {
        Application.targetFrameRate = 60;
        Time.fixedDeltaTime = .01111111f;//physics updates 90 times a second for accurate car physics (no wobbling). oof X__x

        //this is needed to update horizontalAxisHeldFor
        StartCoroutine(BSDInput.StickHeldUpdater());
    }    
}
