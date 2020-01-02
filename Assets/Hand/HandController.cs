using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;

public class HandController : MonoBehaviour
{
    [SerializeField] GameObject player;
    public SteamVR_Action_Skeleton fingers;

    [HideInInspector] public SteamVR_Behaviour_Pose pose;
    [HideInInspector] public Haptics haptics;
    [HideInInspector] public bool[] fingerBoolean = new bool[5];
    [HideInInspector] public bool grabbing;
    [HideInInspector] public InteractableObject objectInHand;

    public DetectInteractions radius;

    bool climbing = false;

    Animator anim;
    Rigidbody rb;
    BodyController bodyController;

    void Start()
    {
        if(player == null)
            player = this.transform.parent.gameObject;

        bodyController = player.GetComponent<BodyController>();
        anim = GetComponent<Animator>();
        pose = GetComponent<SteamVR_Behaviour_Pose>();
        radius = GetComponentInChildren<DetectInteractions>();
        rb = GetComponent<Rigidbody>();
        haptics = GetComponent<Haptics>();
        
    }

    void FixedUpdate()
    {
        //CalculatePhysicsHands();
    }

    void Update()
    {
        // Grabbing bool is set
        FingerInput();

        if (radius.climbable != null && grabbing && !climbing && objectInHand == null)
        {
            bodyController.GrabOnWall(this.gameObject, radius.climbable);
            climbing = true;

            haptics.Pulse(PulseTypes.grab, pose.inputSource);
        }

    }

    void FingerInput()
    {
        anim.SetFloat("thumb", fingers.GetFingerCurl(0));
        fingerBoolean[0] = (fingers.GetFingerCurl(0) > bodyController.fingerDeadZone) ? true : false;

        anim.SetFloat("index", fingers.GetFingerCurl(1));
        fingerBoolean[1] = (fingers.GetFingerCurl(1) > bodyController.fingerDeadZone) ? true : false;

        anim.SetFloat("middle", fingers.GetFingerCurl(2));
        fingerBoolean[2] = (fingers.GetFingerCurl(2) > bodyController.fingerDeadZone) ? true : false;

        anim.SetFloat("ring", fingers.GetFingerCurl(3));
        fingerBoolean[3] = (fingers.GetFingerCurl(3) > bodyController.fingerDeadZone) ? true : false;

        anim.SetFloat("pinky", fingers.GetFingerCurl(4));
        fingerBoolean[4] = (fingers.GetFingerCurl(4) > bodyController.fingerDeadZone) ? true : false;

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
            GrabOn();
        }

        if (fingerDownCount < 2 && grabbing == true)
        {
            grabbing = false;
            GrabOff();
            climbing = false;
            bodyController.GrabOff(this.gameObject, pose.GetVelocity());

        }
    }

    void CalculatePhysicsHands()
    {
        Vector3 targetPos = pose.poseAction.GetLocalPosition(pose.inputSource);
        Vector3 velocityToApproach = targetPos - this.transform.localPosition;
        //velocityToApproach.x = (Mathf.Abs(bodyController.rb.velocity.x) >= 1) ? velocityToApproach.x * (-bodyController.rb.velocity.x): velocityToApproach.x;
        //velocityToApproach.y = (Mathf.Abs(bodyController.rb.velocity.y) >= 1) ? velocityToApproach.y * (-bodyController.rb.velocity.y): velocityToApproach.y;
        //velocityToApproach.z = (Mathf.Abs(bodyController.rb.velocity.z) >= 1) ? velocityToApproach.z * (-bodyController.rb.velocity.z): velocityToApproach.z;

        rb.velocity = (velocityToApproach * bodyController.handStrenght);

        Quaternion targetRot = pose.poseAction.GetLocalRotation(pose.inputSource);

        //Quaternion velocityToTurn = targetRot * Quaternion.Inverse(this.transform.localRotation);

        //float angle;
        //Vector3 axis;
        //velocityToTurn.ToAngleAxis(out angle, out axis);

        //if (angle > 180)
        //    angle -= 360;

        //Vector3 finalAngularVelocity = (Time.fixedDeltaTime * angle * axis) * bodyController.rotationalHandStrenght;

        //finalAngularVelocity.x = float.IsNaN(finalAngularVelocity.x) ? 0 : finalAngularVelocity.x;
        //finalAngularVelocity.y = float.IsNaN(finalAngularVelocity.y) ? 0 : finalAngularVelocity.y;
        //finalAngularVelocity.z = float.IsNaN(finalAngularVelocity.z) ? 0 : finalAngularVelocity.z;

        //rb.angularVelocity = finalAngularVelocity;

        this.transform.localRotation = targetRot;

        if (Vector3.Distance(this.transform.localPosition, targetPos) > bodyController.armLenght)
        {
            if (bodyController.climbing)
                bodyController.GrabOff(this.gameObject, bodyController.rb.velocity);
            this.transform.localPosition = targetPos;
        }
    }

    public void GrabOff()
    {
        if(objectInHand != null && objectInHand.handGrabbing == radius.gameObject)
        {
            objectInHand.handGrabbing = null;

            if (radius.slot != null)
            {
                if (radius.slot.PutItemInSlot(objectInHand))
                {
                    objectInHand = null;
                    radius.grabbable = null;
                    return;
                }
            }

            if(radius.weakspot != null)
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
        if (radius.grabbable != null)
        {
            objectInHand = radius.grabbable;
            radius.grabbable.handGrabbing = radius.gameObject;
            radius.grabbable.rootObject.transform.position = radius.transform.position;
            radius.grabbable.rootObject.transform.rotation = radius.transform.rotation;
            radius.grabbable.rb.useGravity = false;
            radius.grabbable.rb.isKinematic = true;

            haptics.Pulse(PulseTypes.grab, pose.inputSource);
        }

        if(radius.slot != null)
        {
            objectInHand = radius.slot.TakeItemFromSlot();

            if (objectInHand != null)
            {
                objectInHand.handGrabbing = radius.gameObject;
                objectInHand.rootObject.transform.position = radius.transform.position;
                objectInHand.rootObject.transform.rotation = radius.transform.rotation;
                haptics.Pulse(PulseTypes.grab, pose.inputSource);
            }
        }
    }
}
