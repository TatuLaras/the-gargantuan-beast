using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectInteractions : MonoBehaviour
{

    [HideInInspector] public InteractableObject grabbable = null;
    [HideInInspector] public InventorySlot slot = null;
    [HideInInspector] public GameObject climbable = null;
    [HideInInspector] public Weakspot weakspot = null;

    public HandController handController;

    void Start()
    {
        if (handController == null)
        {
            Debug.LogError("Assign a hand controller", this.gameObject);
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPaused = true;
#endif
        }
    }

    void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Climbable"))
        {
            climbable = null;
        }

        if (other.GetComponent<InteractableObject>())
        {
            grabbable = null;
        }

        if (other.GetComponent<InventorySlot>())
        {
            slot = null;
        }

        if (other.GetComponent<Weakspot>())
        {
            weakspot = null;
        }
    }

    void OnTriggerStay(Collider other)
    {

        if (other.CompareTag("Climbable"))
        {
            if(climbable == null)
            {
                handController.haptics.Pulse(PulseTypes.basicTouch, handController.pose.inputSource);
            }

            climbable = other.gameObject;
        }

        InteractableObject objectGrabbed = null;

        if (other.GetComponent<InteractableObject>())
        {
            objectGrabbed = other.GetComponent<InteractableObject>();

            if (grabbable == null && objectGrabbed.handGrabbing == null)
            {
                handController.haptics.Pulse(PulseTypes.basicTouch, handController.pose.inputSource);
            }

            if (objectGrabbed.handGrabbing == null)
            {
                grabbable = objectGrabbed;
            }
        }

        if (other.GetComponent<InventorySlot>())
        {
            if(slot == null)
            {
                handController.haptics.Pulse(PulseTypes.basicTouch, handController.pose.inputSource);
            }

            slot = other.GetComponent<InventorySlot>();
        }

        if (other.GetComponent<Weakspot>())
        {
            if (weakspot == null)
            {
                handController.haptics.Pulse(PulseTypes.basicTouch, handController.pose.inputSource);
            }

            weakspot = other.GetComponent<Weakspot>();
        }
    }

}
