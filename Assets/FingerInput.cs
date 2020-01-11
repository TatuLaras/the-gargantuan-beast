using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class FingerInput : MonoBehaviour
{
    [HideInInspector] public bool[] fingerBoolean = new bool[5];
    [HideInInspector] public bool grabbing;
    public SteamVR_Action_Skeleton fingers;

    HandController handController;
    ObjectInteraction objectInteraction;

    void Start()
    {
        handController = GetComponent<HandController>();
        objectInteraction = GetComponent<ObjectInteraction>();
    }

    void Update()
    {
        fingerBoolean[0] = (fingers.GetFingerCurl(0) > handController.bodyController.fingerDeadZone) ? true : false;
        fingerBoolean[1] = (fingers.GetFingerCurl(1) > handController.bodyController.fingerDeadZone) ? true : false;
        fingerBoolean[2] = (fingers.GetFingerCurl(2) > handController.bodyController.fingerDeadZone) ? true : false;
        fingerBoolean[3] = (fingers.GetFingerCurl(3) > handController.bodyController.fingerDeadZone) ? true : false;
        fingerBoolean[4] = (fingers.GetFingerCurl(4) > handController.bodyController.fingerDeadZone) ? true : false;

        int fingerDownCount = 0;
        foreach (bool finger in fingerBoolean)
        {
            if (finger == true)
            {
                fingerDownCount++;
            }
        }

        if (fingerDownCount >= 2 && grabbing == false)
        {
            grabbing = true;
            objectInteraction.GrabOn();
        }

        if (fingerDownCount < 2 && grabbing == true)
        {
            grabbing = false;
            objectInteraction.GrabOff();
            handController.climbing = false;
            handController.bodyController.GetComponent<Climbing>().GrabOff(this.gameObject);

        }
    }
}
