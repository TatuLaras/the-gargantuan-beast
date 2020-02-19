using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodableCage : MonoBehaviour
{
    public void Explode()
    {
        this.gameObject.SetActive(false);
        FindObjectOfType<ParticleManager>().SpawnParticleAt(this.transform.position, Particle.cageExplode, this.transform.rotation);
    }
}
