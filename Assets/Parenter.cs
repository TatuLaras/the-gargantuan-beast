using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parenter : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<BodyCollision>() != null)
        {
            FindObjectOfType<BodyController>().transform.parent = this.transform;
            print("Parenter enter main");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<BodyCollision>() != null)
        {
            FindObjectOfType<BodyController>().transform.parent = null;
            print("Parenter exit main");

        }
    }
}
