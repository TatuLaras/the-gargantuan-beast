using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointManager : MonoBehaviour
{
    [SerializeField] Transform spawn;

    void Start()
    {
        this.transform.position = spawn.position;
    }

}
