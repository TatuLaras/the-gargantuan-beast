using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyCollision : MonoBehaviour
{
    [SerializeField] Transform head;

    void Update()
    {
        this.transform.position = head.position;
        this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, head.rotation.eulerAngles.y, this.transform.rotation.z);
    }
}
