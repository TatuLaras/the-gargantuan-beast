using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(InteractableObject))]
public class Bomb : MonoBehaviour
{
    InteractableObject interactableObject;
    GameObject player;

    bool fuseLit;
    bool countDownOn = false;

    [SerializeField] float fuseLenghtSeconds = 10f;
    [SerializeField] float explosionRadius = 5f;
    [SerializeField] float nearAudioRadius = 6f;
    [SerializeField] float farAudioRadius = 15f;
    [SerializeField] GameObject explosionParticlePrefab;

    public GameObject bombStrap;

    public bool attached;

    void Start()
    {
        player = FindObjectOfType<BodyController>().gameObject;
        bombStrap.SetActive(false);
        interactableObject = GetComponent<InteractableObject>();
    }

    void Update()
    {
        if(fuseLit == false && interactableObject.fire.ablaze == true)
        {
            fuseLit = true;

            if (attached)
            {
                StartCoroutine(Singleton.instance.audio.PlayClip(Singleton.instance.bombClip, AudioType.urgent));
                print("bombcount");
            }
        }

        if(fuseLit == true && countDownOn == false)
        {
            countDownOn = true;
            Invoke("Explode", fuseLenghtSeconds);
            StartCoroutine(Singleton.instance.audio.PlayClip(Singleton.instance.bombClip, AudioType.urgent, true));
        }
        
        if(interactableObject.handGrabbing != null)
        {
            bombStrap.SetActive(false);
        }

        if(countDownOn == true)
        {
            Singleton.instance.audio.AdjustVolumeBasedOnDistance(Vector3.Distance(this.transform.position, player.transform.position), nearAudioRadius, farAudioRadius, 0.7f);
        }
    }

    void Explode()
    {
        //ResetTriggers();
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

    void ResetTriggers()
    {
        MusicTrigger[] triggers = FindObjectsOfType<MusicTrigger>();
        foreach(MusicTrigger trigger in triggers)
        {
            trigger.inside = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, explosionRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, nearAudioRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position, farAudioRadius);

    }
}
