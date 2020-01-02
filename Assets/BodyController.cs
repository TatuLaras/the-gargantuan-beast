using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;


public class BodyController : MonoBehaviour
{

    GameObject climbingHand;
    GameObject climbRef;

    [SerializeField] float standingUpSpeed = 5f;
    [SerializeField] float armClimbStrenght = 100f;
    [SerializeField] float releaseVelocityPercent = 50f;
    [SerializeField] float friction = 0.9f;
    [SerializeField] float groundSteepnessTreshold = 60;

    // Used in individual hand scripts
    public float fingerDeadZone = 0.9f;
    [HideInInspector] public float handStrenght = 50f;
    [HideInInspector] public float armLenght = 0.8f;

    // Not used currently
    [HideInInspector] public float rotationalHandStrenght = 250f;

    Transform head;

    [HideInInspector] public bool grounded;
    [HideInInspector] public bool climbing;
    [HideInInspector] public Rigidbody rb;

    void Start()
    {
        head = GetComponentInChildren<Camera>().gameObject.transform;
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        LegUpdate();
        ClimbUpdate();

        if (grounded)
        {
            rb.velocity = new Vector3(rb.velocity.x * friction, rb.velocity.y, rb.velocity.z * friction);
        }

    }

    void ClimbUpdate()
    {
        Vector3 targetPosition = Vector3.zero;

        // GrabOn()-funktio
        if (climbingHand != null && climbRef != null)
        {
            climbing = true;
            rb.useGravity = false;

            SteamVR_Behaviour_Pose pose = climbingHand.GetComponent<HandController>().pose;

            // Change back to hand position maybe
            targetPosition = this.transform.position + (climbRef.transform.position - transform.TransformPoint(pose.poseAction.GetLocalPosition(pose.inputSource)));

            rb.velocity = (targetPosition - this.transform.position) * armClimbStrenght;

        }
        else
        {
            climbing = false;
            rb.useGravity = true;
        }
    }

    void LegUpdate()
    {
        // Bit shift the index of the layer (0) to get a bit mask
        int layerMask = 1 << 0;

        RaycastHit hit;

        float feetLenght = 10f;
        Vector3 hitNormal = Vector3.zero;

        float distanceFromFloor = Vector3.Dot(head.localPosition, Vector3.up);


        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(head.position, transform.TransformDirection(Vector3.down), out hit, distanceFromFloor + 0.1f, layerMask))
        {
            feetLenght = hit.distance;
            hitNormal = hit.normal;
            grounded = true;
        }
        else
        {
            grounded = false;
        }

        if (Vector3.Angle(Vector3.up, hit.normal) > groundSteepnessTreshold)
        {
            grounded = false;
            print(Vector3.Angle(Vector3.up, hit.normal));
        }

        if (grounded)
        {
            if (feetLenght <= distanceFromFloor)
            {
                float fallingFlex = (rb.velocity.y < 0) ? rb.velocity.y : 0;
                rb.velocity = new Vector3(rb.velocity.x, ((distanceFromFloor - feetLenght) * standingUpSpeed), rb.velocity.z);
            }

        }
    }

    public void GrabOnWall(GameObject hand, GameObject wall)
    {
        //Remove previous climbref
        climbingHand = hand;
        climbRef = new GameObject("climbref");
        climbRef.transform.position = hand.transform.position;
        climbRef.transform.parent = wall.transform;
    }

    public void GrabOff(GameObject hand, Vector3 handVelocity)
    {
        if(hand == climbingHand)
        {
            rb.velocity = rb.velocity * (releaseVelocityPercent / 100);
            climbingHand = null;
            Destroy(climbRef);
        }
    }
}
