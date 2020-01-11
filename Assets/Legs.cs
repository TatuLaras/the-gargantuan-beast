using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Legs : MonoBehaviour
{
    [SerializeField] float standingUpSpeed = 5f;
    [SerializeField] float groundSteepnessTreshold = 60;
    Transform head;
    Rigidbody rb;
    Climbing climbingManager;
    RefPointVelocities refs;
    FrictionAndVelocity frictionAndVelocity;

    [HideInInspector] public GameObject legRef;
    [HideInInspector] public bool grounded;

    void Start()
    {
        refs = GetComponent<RefPointVelocities>();
        rb = GetComponent<Rigidbody>();
        head = GetComponentInChildren<Camera>().gameObject.transform;
        climbingManager = GetComponent<Climbing>();
        frictionAndVelocity = GetComponent<FrictionAndVelocity>();
    }

    void LateUpdate()
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
        }

        if (grounded == true && climbingManager.climbing == false)
        {
            if (feetLenght <= distanceFromFloor)
            {
                //float fallingFlex = (rb.velocity.y < 0) ? rb.velocity.y : 0;
                //rb.velocity = new Vector3(rb.velocity.x, ((distanceFromFloor - feetLenght) * standingUpSpeed) + fallingFlex, rb.velocity.z);

                frictionAndVelocity.desiredLegVelocityY = (((distanceFromFloor - feetLenght) * standingUpSpeed));

                if (legRef == null)
                {
                    legRef = new GameObject("legref");
                    legRef.transform.position = hit.point;
                    legRef.transform.parent = hit.collider.transform;
                }
                else if (hit.collider.transform != legRef.transform.parent)
                {
                    Destroy(legRef);
                    legRef = new GameObject("legref");
                    legRef.transform.position = hit.point;
                    legRef.transform.parent = hit.collider.transform;
                }
            }
        }
        else
        {
            Destroy(legRef);
            legRef = null;
            if(refs != null)
                refs.leg_posLastFrame = Vector3.zero;
            frictionAndVelocity.desiredLegVelocityY = 0;
        }
    }
}
