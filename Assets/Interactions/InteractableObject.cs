using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

[RequireComponent(typeof(Collider))]
public class InteractableObject : MonoBehaviour
{
    //[SerializeField] float locationStrenght = 100;
    //[SerializeField] float rotationStrenght = 400;
    BodyController player;
    [SerializeField] float paragliderSmoothspeed = 2f;

    [HideInInspector] public GameObject handGrabbing = null;
    [HideInInspector] public Rigidbody rb;

    public GameObject rootObject;

    public ObjectType objectType = ObjectType.undefined;
    public bool doNotInteract = false;
    public SpreadingFire fire = null;
    public GameObject handlebone;

    void Start()
    {
        if(rootObject == null)
        {
            Debug.LogError("Assign a root object for the interactable", this.gameObject);
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPaused = true;
#endif
        }
        
        rb = rootObject.GetComponent<Rigidbody>();

        if (doNotInteract)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        player = FindObjectOfType<BodyController>();
    }

    void Update()
    {
        if(handGrabbing != null && !doNotInteract)
        {
            if(objectType == ObjectType.paraglider)
            {
                if(handlebone != null)
                {
                    rootObject.transform.position = handGrabbing.transform.position;
                    handlebone.transform.rotation = handGrabbing.GetComponent<DetectInteractions>().paragliderPoint.transform.rotation;

                    Quaternion target = Quaternion.LookRotation(player.GetComponent<Rigidbody>().velocity);
                    target = Quaternion.Euler(new Vector3(0, target.eulerAngles.y, 0));
                    //Quaternion smoothed = Quaternion.Lerp(rootObject.transform.rotation, target, 0.5f);

                    rootObject.transform.rotation = Quaternion.RotateTowards(rootObject.transform.rotation, target, Time.deltaTime * paragliderSmoothspeed);
                }
                else
                {
                    Debug.LogError("Handlebone not assigned");
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPaused = true;
#endif
                }

            } else
            {
                rootObject.transform.rotation = handGrabbing.transform.rotation;
                rootObject.transform.position = handGrabbing.transform.position;
            }

            // Check if left hand, then flip the object
            if(handGrabbing.GetComponentInParent<SteamVR_Behaviour_Pose>().inputSource == Singleton.instance.leftHand && objectType != ObjectType.paraglider)
                rootObject.transform.Rotate(180, 0, 0, Space.Self);
        }
    }

    //void CalculateObjectPhysics()
    //{
    //    Vector3 targetPos = handGrabbing.transform.position;
    //    Vector3 velocityToApproach = targetPos - this.transform.position;
    //    rb.velocity = velocityToApproach * locationStrenght;
    //    Quaternion targetRot = handGrabbing.transform.rotation;

    //    Quaternion velocityToTurn = targetRot * Quaternion.Inverse(this.transform.localRotation);

    //    float angle;
    //    Vector3 axis;
    //    velocityToTurn.ToAngleAxis(out angle, out axis);

    //    if (angle > 180)
    //        angle -= 360;

    //    Vector3 finalAngularVelocity = (Time.fixedDeltaTime * angle * axis) * rotationStrenght;

    //    finalAngularVelocity.x = float.IsNaN(finalAngularVelocity.x) ? 0 : finalAngularVelocity.x;
    //    finalAngularVelocity.y = float.IsNaN(finalAngularVelocity.y) ? 0 : finalAngularVelocity.y;
    //    finalAngularVelocity.z = float.IsNaN(finalAngularVelocity.z) ? 0 : finalAngularVelocity.z;

    //    rb.angularVelocity = finalAngularVelocity;

    //    if (Vector3.Distance(this.transform.localPosition, targetPos) > 0.5)
    //    {
    //        this.transform.localPosition = targetPos;
    //    }
    //}

}
