using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(InteractableObject))]
public class Bomb : MonoBehaviour
{
    InteractableObject interactableObject;
    bool fuseLit;
    bool countDownOn = false;

    [SerializeField] float fuseLenghtSeconds = 10f;
    [SerializeField] float explosionRadius = 5f;

    public GameObject bombStrap;
    [SerializeField] GameObject explosionParticlePrefab;

    void Start()
    {
        bombStrap.SetActive(false);
        interactableObject = GetComponent<InteractableObject>();
    }

    void Update()
    {
        fuseLit = interactableObject.fire.ablaze;

        if(fuseLit == true && countDownOn == false)
        {
            countDownOn = true;
            Invoke("Explode", fuseLenghtSeconds);
        }
        
        if(interactableObject.handGrabbing != null)
        {
            bombStrap.SetActive(false);
        }
    }

    void Explode()
    {

        Weakspot[] weakspots = FindObjectsOfType<Weakspot>();

        foreach (Weakspot weakspot in weakspots)
        {
            if(Vector3.Distance(this.transform.position, weakspot.transform.position) <= explosionRadius)
            {
                weakspot.Explode();
            }
        }

        Instantiate(explosionParticlePrefab, this.transform.position, Quaternion.identity);
        Destroy(interactableObject.rootObject);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (Selection.Contains(this.gameObject))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.transform.position, explosionRadius);
        }
    }
#endif
}
