using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Climbing : MonoBehaviour
{
    [SerializeField] float armClimbStrenght = 50f;
    GameObject climbRef;
    GameObject climbingHand;
    Rigidbody rb;
    FrictionAndVelocity frictionAndVelocity;

    public bool climbing;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        frictionAndVelocity = GetComponent<FrictionAndVelocity>();
    }

    void Update()
    {
        Vector3 targetPosition = Vector3.zero;

        if (climbingHand != null && climbRef != null)
        {
            climbing = true;
            rb.useGravity = false;

            SteamVR_Behaviour_Pose pose = climbingHand.GetComponent<SteamVR_Behaviour_Pose>();

            targetPosition = this.transform.position + (climbRef.transform.position - transform.TransformPoint(pose.poseAction.GetLocalPosition(pose.inputSource)));

            frictionAndVelocity.desiredClimbVelocity = (targetPosition - this.transform.position) * armClimbStrenght;

        }
        else
        {
            climbing = false;
            rb.useGravity = true;
        }
    }

    public void GrabOnWall(GameObject hand, GameObject wall)
    {
        climbingHand = hand;
        if (climbRef != null)
            Destroy(climbRef);
        climbRef = new GameObject("climbref");
        climbRef.transform.parent = wall.transform;
        climbRef.transform.position = hand.transform.position;
    }

    public void GrabOff(GameObject hand)
    {
        if (hand == climbingHand)
        {
            climbingHand = null;
            Destroy(climbRef);
        }
    }
}
