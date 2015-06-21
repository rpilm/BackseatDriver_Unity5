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
        firstPersonCam.SetActive(true);
        thirdPersonCam.SetActive(false);
        backUpCam.SetActive(false);
        rightSideViewCam.SetActive(false);
        leftSideViewCam.SetActive(false);

        mainCam = firstPersonCam;
        currentCam = firstPersonCam;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(Input.GetKeyDown(KeyCode.Joystick1Button3))
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


        if(Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            ChangeMainCamera(backUpCam);
        }
        if (Input.GetKeyUp(KeyCode.Joystick1Button0))
        {
            ChangeMainCamera(mainCam);
        }

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
