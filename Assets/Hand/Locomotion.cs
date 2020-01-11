using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Legs))]
public class Locomotion : MonoBehaviour
{

    [SerializeField] float speed = 800f;
    [SerializeField] SteamVR_Action_Vector2 joystick;
    [SerializeField] float joystickDeadzone = 0.2f;
    [SerializeField] GameObject preferredLocomotionHand;
    Legs legs;
    Climbing climbingManager;
    FrictionAndVelocity frictionAndVelocity;
    Rigidbody rb;

    [HideInInspector] public bool locomoting;

    void Start()
    {
        legs = GetComponent<Legs>();
        rb = GetComponent<Rigidbody>();
        climbingManager = GetComponent<Climbing>();
        frictionAndVelocity = GetComponent<FrictionAndVelocity>();
    }

    void Update()
    {
        float x = joystick.axis.x;
        float y = joystick.axis.y;

        if (Mathf.Abs(x) >= joystickDeadzone || Mathf.Abs(y) >= joystickDeadzone)
        {
            Vector3 velocity = (preferredLocomotionHand.transform.right * x + preferredLocomotionHand.transform.forward * y) * Time.fixedDeltaTime * speed;

            frictionAndVelocity.desiredLocomotionVelocity = new Vector2(velocity.x, velocity.z);

            locomoting = true;
        }
        else
        {
            locomoting = false;
        }
    }
}
