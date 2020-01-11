using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ObjectInteraction : MonoBehaviour
{
    [HideInInspector] public InteractableObject objectInHand;

    DetectInteractions radius;
    Haptics haptics;
    HandController handController;
    SteamVR_Behaviour_Pose pose;
    BodyCollision bodyCollision;
    FrictionAndVelocity frictionAndVelocity;

    void Start()
    {
        radius = GetComponentInChildren<DetectInteractions>();
        haptics = GetComponent<Haptics>();
        handController = GetComponent<HandController>();
        pose = GetComponent<SteamVR_Behaviour_Pose>();
        bodyCollision = FindObjectOfType<BodyCollision>();
        frictionAndVelocity = GetComponentInParent<FrictionAndVelocity>();
    }

    public void GrabOff()
    {
        if (objectInHand != null && objectInHand.handGrabbing == radius.gameObject)
        {
            objectInHand.handGrabbing = null;

            if (objectInHand.objectType == ObjectType.paraglider)
            {
                frictionAndVelocity.paragliding = false;
            }

            if (radius.slot != null)
            {
                if (radius.slot.PutItemInSlot(objectInHand))
                {
                    objectInHand = null;
                    radius.grabbable = null;
                    return;
                }
            }

            if (radius.weakspot != null)
            {
                if (radius.weakspot.PlantBomb(objectInHand, radius.transform.position))
                {
                    objectInHand = null;
                    radius.grabbable = null;
                    return;
                }
            }

            objectInHand.rb.useGravity = true;
            objectInHand.rb.isKinematic = false;
            objectInHand.rb.velocity = pose.GetVelocity();
            objectInHand.rb.angularVelocity = pose.GetAngularVelocity();

            objectInHand = null;
        }
    }

    public void GrabOn()
    {
        if (radius.grabbable != null && radius.grabbable.doNotInteract == false)
        {
            objectInHand = radius.grabbable;
            radius.grabbable.handGrabbing = radius.gameObject;

            if(radius.grabbable.objectType == ObjectType.paraglider)
            {
                radius.grabbable.rootObject.transform.rotation = bodyCollision.transform.rotation;
                frictionAndVelocity.paragliding = true;
            }

            radius.grabbable.rb.useGravity = false;
            radius.grabbable.rb.isKinematic = true;

            haptics.Pulse(PulseTypes.grab, pose.inputSource);
        }

        if (radius.slot != null)
        {
            objectInHand = radius.slot.TakeItemFromSlot();

            if (objectInHand != null)
            {
                if (objectInHand.objectType == ObjectType.paraglider)
                {
                    frictionAndVelocity.paragliding = true;
                }
                objectInHand.handGrabbing = radius.gameObject;
                haptics.Pulse(PulseTypes.grab, pose.inputSource);
            }
        }
    }
}
