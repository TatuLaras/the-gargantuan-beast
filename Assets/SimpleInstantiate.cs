using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleInstantiate : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] int amount;
    [SerializeField] float timeBetween = 0.5f;
        
    void Start()
    {
        StartCoroutine("Instantiator");
    }

    IEnumerator Instantiator()
    {
        for(int i = 0; i < amount; i++)
        {
            Instantiate(prefab, this.transform.position, Quaternion.identity, this.transform);
            yield return new WaitForSeconds(timeBetween);
        }
    }
}
