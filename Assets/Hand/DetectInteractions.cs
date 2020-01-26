using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class DetectInteractions : MonoBehaviour
{

    [HideInInspector] public InteractableObject grabbable = null;
    [HideInInspector] public InventorySlot slot = null;
    [HideInInspector] public GameObject climbable = null;
    [HideInInspector] public Weakspot weakspot = null;
    public GameObject paragliderPoint;

    HandController handController;

    Haptics haptics;
    SteamVR_Behaviour_Pose pose;

    void Start()
    {
        handController = GetComponentInParent<HandController>();
        haptics = handController.GetComponent<Haptics>();
        pose = handController.GetComponent<SteamVR_Behaviour_Pose>();
    }

    void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Climbable"))
        {
            climbable = null;
        }

        if (other.GetComponent<InteractableObject>())
        {
            grabbable = null;
        }

        if (other.GetComponent<InventorySlot>())
        {
            slot = null;
        }

        if (other.GetComponent<Weakspot>())
        {
            weakspot = null;
        }
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Climbable"))
        {
            if(climbable == null)
            {
                haptics.Pulse(PulseTypes.basicTouch, pose.inputSource);
            }

            climbable = other.gameObject;
        }

        InteractableObject objectGrabbed = null;

        if (other.GetComponent<InteractableObject>())
        {
            objectGrabbed = other.GetComponent<InteractableObject>();

            if (grabbable == null && objectGrabbed.handGrabbing == null)
            {
                haptics.Pulse(PulseTypes.basicTouch, pose.inputSource);
            }

            if (objectGrabbed.handGrabbing == null)
            {
                grabbable = objectGrabbed;
            }
        }

        if (other.GetComponent<InventorySlot>())
        {
            if(slot == null)
            {
                haptics.Pulse(PulseTypes.basicTouch, pose.inputSource);
            }

            slot = other.GetComponent<InventorySlot>();
        }

        if (other.GetComponent<Weakspot>())
        {
            if (weakspot == null)
            {
                haptics.Pulse(PulseTypes.basicTouch, pose.inputSource);
            }

            weakspot = other.GetComponent<Weakspot>();
        }
    }

}
