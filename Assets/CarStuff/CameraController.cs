using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
    public GameObject firstPersonCam;
    public GameObject thirdPersonCam;
    public GameObject backUpCam;
    public GameObject rightSideViewCam;
    public GameObject leftSideViewCam;

    public GameObject mainCam;
    public GameObject currentCam;
	// Use this for initialization
	void Start () 
    {
        // set third person as default camera
        thirdPersonCam.SetActive(true);

        firstPersonCam.SetActive(false);
        backUpCam.SetActive(false);
        rightSideViewCam.SetActive(false);
        leftSideViewCam.SetActive(false);

        mainCam = thirdPersonCam;
        currentCam = thirdPersonCam;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(Input.GetButtonDown("ChangeCamera"))
        {
            if (firstPersonCam.activeSelf)
            {
                mainCam = thirdPersonCam;
                ChangeMainCamera(thirdPersonCam);
            }
            else
            {
                mainCam = firstPersonCam;
                ChangeMainCamera(firstPersonCam);
            }
        }


        if(Input.GetButtonDown("RearView"))
        {
            ChangeMainCamera(backUpCam);
        }
        if (Input.GetButtonUp("RearView"))
        {
            ChangeMainCamera(mainCam);
        }

        // TODO: setup controls for side view cameras
        if (Input.GetKeyDown(KeyCode.Joystick1Button2))
        {
            ChangeMainCamera(leftSideViewCam);
        }
        if (Input.GetKeyUp(KeyCode.Joystick1Button2))
        {
            ChangeMainCamera(mainCam);
        }

        if (Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            ChangeMainCamera(rightSideViewCam);
        }
        if (Input.GetKeyUp(KeyCode.Joystick1Button1))
        {
            ChangeMainCamera(mainCam);
        }
	}


    void ChangeMainCamera(GameObject newCam)
    {
        currentCam.SetActive(false);
        newCam.SetActive(true);
        currentCam = newCam;
    }
    void ChangeTempCamera()
    {

    }
}
