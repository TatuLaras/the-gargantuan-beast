using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadingFire : MonoBehaviour
{
    public bool ablaze;
    [SerializeField] GameObject fireEffect;

    private void Update()
    {
        fireEffect.SetActive(ablaze);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<SpreadingFire>())
        {
            if(other.GetComponent<SpreadingFire>().ablaze == true)
            {
                ablaze = true;
            }
        }
    }
}
