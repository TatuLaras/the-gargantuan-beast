using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefPointVelocities : MonoBehaviour
{
    Legs legs;
    Climbing climbingManager;
    GameObject legRefLastFrame;
    [SerializeField] float maxVelocity = 0.4f;
    Vector3 leg_deltaPosFixed;

    [HideInInspector] public Vector3 leg_posLastFrame;
    [HideInInspector] public Vector3 leg_refVelocity;

    private void Start()
    {
        legs = GetComponent<Legs>();
        climbingManager = GetComponent<Climbing>();
    }

    void FixedUpdate()
    {
        if (legs.grounded == true && legs.legRef != null)
        {
            if (leg_posLastFrame == Vector3.zero)
                leg_posLastFrame = legs.legRef.transform.position;

            leg_deltaPosFixed = legs.legRef.transform.position - leg_posLastFrame;
            leg_posLastFrame = legs.legRef.transform.position;

            leg_refVelocity = leg_deltaPosFixed / Time.fixedDeltaTime;
            float speed = Vector3.SqrMagnitude(leg_refVelocity);

            if (climbingManager.climbing == false && legRefLastFrame == legs.legRef && speed <= Mathf.Pow(maxVelocity, 2))
            {
                //this.transform.position += leg_deltaPosFixed;
            }
            legRefLastFrame = legs.legRef;
        }
    }
}
