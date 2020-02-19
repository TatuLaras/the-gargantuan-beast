using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    [SerializeField] AudioClip orbShatter;

    public void Destruct()
    {
        FindObjectOfType<SFXManager>().NewSFXAt(this.transform.position, orbShatter, 1);
        FindObjectOfType<ParticleManager>().SpawnParticleAt(this.transform.position, Particle.orb, Quaternion.identity);
        GameObject rootObject = GetComponent<InteractableObject>().rootObject;
        rootObject.SetActive(false);
        Destroy(rootObject);
    }
}
