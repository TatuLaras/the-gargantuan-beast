using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubParenter : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<BodyCollision>() != null && (FindObjectOfType<Legs>().ground.tag == "climbable" || FindObjectOfType<Legs>().grounded == false))
        {
            FindObjectOfType<BodyController>().transform.parent = this.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<BodyCollision>() != null)
        {
            FindObjectOfType<BodyController>().transform.parent = null;

        }
    }
}
