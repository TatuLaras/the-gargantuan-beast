using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteThis : MonoBehaviour
{
    
    void Update()
    {
        this.transform.position += Vector3.forward * Time.deltaTime*0.5f;
    }
}
