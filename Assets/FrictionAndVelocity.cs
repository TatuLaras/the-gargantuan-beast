using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FrictionAndVelocity : MonoBehaviour
{
    [SerializeField] float friction = 0.85f;
    [SerializeField] float airResistance = 0.95f;
    [SerializeField] float airSpeedMultiplier = 1.5f;
    Rigidbody rb;
    Climbing climbingManager;
    Legs legs;
    Locomotion locomotion;
    RefPointVelocities refPointVelocities;
    Vector3 posLastFrame;
    Vector3 deltaPos;

    [HideInInspector] public Vector2 desiredLocomotionVelocity;
    [HideInInspector] public Vector3 desiredClimbVelocity;
    [HideInInspector] public float desiredLegVelocityY;
    [HideInInspector] public Vector3 bodyVelocity;
    public bool paragliding;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        climbingManager = GetComponent<Climbing>();
        legs = GetComponent<Legs>();
        locomotion = GetComponent<Locomotion>();
        refPointVelocities = GetComponent<RefPointVelocities>();
    }

    void FixedUpdate()
    {
        if (posLastFrame == Vector3.zero)
            posLastFrame = this.transform.position;

        deltaPos = this.transform.position - posLastFrame;
        posLastFrame = this.transform.position;
        bodyVelocity = deltaPos / Time.fixedDeltaTime;

        if(climbingManager.climbing == false && legs.grounded == true)
        {
            Vector2 horVelocity = Vector2.zero;
            float vertVelocity = 0;

            if (Mathf.Abs(Vector3.Magnitude(refPointVelocities.leg_refVelocity)) >= 0.1f)
            {
                horVelocity = new Vector2(refPointVelocities.leg_refVelocity.x, refPointVelocities.leg_refVelocity.z);
                vertVelocity = refPointVelocities.leg_refVelocity.y;

                if (locomotion.locomoting)
                {
                    horVelocity += desiredLocomotionVelocity;
                }

            } else
            {
                if (locomotion.locomoting)
                {
                    horVelocity += desiredLocomotionVelocity;
                }
                else
                {
                    horVelocity = new Vector2(rb.velocity.x, rb.velocity.z) * friction;
                }
            }

            if (legs.grounded)
            {
                vertVelocity += desiredLegVelocityY;
            } else
            {
                vertVelocity = rb.velocity.y;
            }

            if (Mathf.Abs(Vector3.Magnitude(refPointVelocities.leg_refVelocity)) >= 0.1f)
            {
                horVelocity += new Vector2(refPointVelocities.leg_refVelocity.x, refPointVelocities.leg_refVelocity.z);
                vertVelocity += refPointVelocities.leg_refVelocity.y;
            }

            Vector3 velocity = new Vector3(horVelocity.x, vertVelocity, horVelocity.y);
            //velocity += refPointVelocities.leg_refVelocity;
            rb.velocity = velocity;
        }
        else if(climbingManager.climbing == true)
        {
            rb.velocity = desiredClimbVelocity;
        }

        if (paragliding == true)
        {
            Vector3 velocity = Vector3.zero;

            if (locomotion.locomoting)
            {
                velocity += new Vector3(desiredLocomotionVelocity.x * airSpeedMultiplier, 0, desiredLocomotionVelocity.y * airSpeedMultiplier);
            }

            velocity = new Vector3(velocity.x, rb.velocity.y * airResistance, velocity.z);
            rb.velocity = velocity;
        }

       

    }
}
