using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class InteractableObject : MonoBehaviour
{
    //[SerializeField] float locationStrenght = 100;
    //[SerializeField] float rotationStrenght = 400;
    BodyController player;
    float paragliderSmoothspeed = 60f;

    [HideInInspector] public GameObject handGrabbing = null;
    [HideInInspector] public Rigidbody rb;

    public GameObject rootObject;

    public ObjectType objectType;
    public bool doNotInteract = false;
    public SpreadingFire fire = null;
    public GameObject handlebone = null;

    public bool permanent = false;
    public InventorySlot slot;

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
                    if(player.GetComponent<Legs>().grounded == true)
                    {
                        handGrabbing.GetComponentInParent<ObjectInteraction>().ReleaseItem();
                    }

                    rootObject.transform.position = handGrabbing.transform.position;
                    handlebone.transform.rotation = handGrabbing.GetComponent<DetectInteractions>().paragliderPoint.transform.rotation;

                    Quaternion target = Quaternion.LookRotation(player.GetComponent<Rigidbody>().velocity);
                    target = Quaternion.Euler(new Vector3(0, target.eulerAngles.y, 0));
                    //Quaternion smoothed = Quaternion.Lerp(rootObject.transform.rotation, target, 0.5f);

                    if(Quaternion.Angle(target, rootObject.transform.rotation) >= 5f)
                    {
                        rootObject.transform.rotation = Quaternion.RotateTowards(rootObject.transform.rotation, target, Time.deltaTime * paragliderSmoothspeed);
                    }
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

    public void OnRelease()
    {
        if(permanent == true && slot != null)
        {
            StartCoroutine(ReturnItem());
        }
    }

    IEnumerator ReturnItem()
    {
        StartCoroutine(rootObject.GetComponent<DissolveObject>().Dissolve());
        if(fire != null)
            fire.ablaze = false;
        yield return new WaitForSeconds(1.1f);
        slot.PutItemInSlot(this);


        rootObject.GetComponent<DissolveObject>().ResetDissolve();

    }

}
