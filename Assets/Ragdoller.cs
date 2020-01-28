using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoller : MonoBehaviour
{
    [SerializeField] Rigidbody[] childRb;
    [SerializeField] Collider[] childCol;

    Animator anim;

    [SerializeField] bool ragdollize = false;
    [SerializeField] PathCreation.Examples.PathFollower follower;

    void Start()
    {
        anim = GetComponent<Animator>();

        ToggleRagdoll(false);
    }

    private void Update()
    {
        if(ragdollize == true)
        {
            ToggleRagdoll(true);
            ragdollize = false;
        }
    }

    public void ToggleRagdoll(bool state)
    {
        anim.enabled = !state;
        
        if(follower != null)
        {
            follower.enabled = !state;
        }

        foreach(Rigidbody child in childRb)
        {
            child.isKinematic = !state;
            child.useGravity = state;
            child.velocity = Vector3.zero;
        }

        foreach(Collider child in childCol)
        {
            child.enabled = !state;
        }
    }
}
