using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class InteractableObject : MonoBehaviour
{
    //[SerializeField] float locationStrenght = 100;
    //[SerializeField] float rotationStrenght = 400;

    [HideInInspector] public GameObject handGrabbing = null;
    [HideInInspector] public Rigidbody rb;

    public ObjectType objectType;
    public GameObject rootObject;

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

    }

    void Update()
    {
        if(handGrabbing != null)
        {
            rootObject.transform.position = handGrabbing.transform.position;
            rootObject.transform.rotation = handGrabbing.transform.rotation;

            // Check if left hand, then flip the object
            if(handGrabbing.GetComponent<DetectInteractions>().handController.pose.inputSource == Singleton.instance.leftHand)
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
