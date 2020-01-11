using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingPattern : MonoBehaviour
{
    [SerializeField] float walkingLimits = 200f;
    [SerializeField] int turns = 5; 

    Vector3 startPos;
    Animator anim;
    bool outOfBounds = false;
    int turnsTakenThisTime = 0;

    void Start()
    {
        anim = GetComponent<Animator>();
        startPos = this.transform.position;
    }

    public void StepTaken()
    {
        if(Vector3.Distance(this.transform.position, startPos) >= walkingLimits && outOfBounds == false)
        {
            anim.SetTrigger("Turn");
            outOfBounds = true;
            turnsTakenThisTime = 0;
        } else if (Vector3.Distance(this.transform.position, startPos) <= walkingLimits)
        {
            outOfBounds = false;
        }
    }

    public void CheckTurns()
    {
        turnsTakenThisTime++;
        if(turnsTakenThisTime >= turns)
        {
            anim.SetTrigger("StopTurning");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(startPos != Vector3.zero)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(startPos, walkingLimits);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(startPos, walkingLimits + 60);
        }
        else
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(this.transform.position, walkingLimits);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, walkingLimits + 60);
        }
    }

}
