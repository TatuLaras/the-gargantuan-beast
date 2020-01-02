using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penetrable : MonoBehaviour
{
    public float density = 500f;

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.GetComponent<Weapon>())
        {
            Weapon weapon = collision.gameObject.GetComponent<Weapon>();
            if (weapon.ableToPenetrate)
            {
                print("Colliding");
                Physics.IgnoreCollision(collision.collider, this.GetComponent<Collider>());
                collision.gameObject.GetComponent<Rigidbody>().drag = density;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<Weapon>())
        {
            Weapon weapon = collision.gameObject.GetComponent<Weapon>();
            if (weapon.ableToPenetrate)
            {
                print("UNcollide");
                collision.gameObject.GetComponent<Rigidbody>().drag = 0;
            }
        }
    }
}
