using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;


public class BodyController : MonoBehaviour
{
    public float fingerDeadZone = 0.9f;

    private void Update()
    {
        this.transform.rotation = Quaternion.identity;
    }
}
