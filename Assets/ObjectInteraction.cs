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
    SFXManager sfx;

    [SerializeField] AudioClip grabAudio;
    [SerializeField] AudioClip holsterAudio;

    void Start()
    {
        radius = GetComponentInChildren<DetectInteractions>();
        haptics = GetComponent<Haptics>();
        handController = GetComponent<HandController>();
        pose = GetComponent<SteamVR_Behaviour_Pose>();
        bodyCollision = FindObjectOfType<BodyCollision>();
        frictionAndVelocity = GetComponentInParent<FrictionAndVelocity>();
        sfx = FindObjectOfType<SFXManager>();
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
                    sfx.NewSFXAt(this.transform.position, holsterAudio, 0.8f);
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

            ReleaseItem();
        }
    }

    public void ReleaseItem()
    {
        if (objectInHand == null)
            return;

        if (objectInHand.objectType == ObjectType.paraglider)
            frictionAndVelocity.paragliding = false;

        objectInHand.rb.useGravity = true;
        objectInHand.rb.isKinematic = false;
        objectInHand.rb.velocity = pose.GetVelocity();
        objectInHand.rb.angularVelocity = pose.GetAngularVelocity();
        objectInHand.OnRelease();

        objectInHand = null;
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

            sfx.NewSFXAt(this.transform.position, grabAudio, 1f);
            haptics.Pulse(PulseTypes.grab, pose.inputSource);
        }
    }

    public void GrabOnTrigger()
    {
        if (radius.slot != null)
        {
            sfx.NewSFXAt(this.transform.position, grabAudio, 1f);

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
