using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] ParticleSystem orbShatter;
    [SerializeField] int amountOfShatters = 3;
    ParticleSystem[] orbShatters;


    [SerializeField] ParticleSystem explosion;
    [SerializeField] int amountOfExplosions = 3;
    ParticleSystem[] explosions;

    [SerializeField] ParticleSystem dissolve;
    [SerializeField] int amountOfDissolves = 5;
    ParticleSystem[] dissolves;

    [SerializeField] ParticleSystem cageExplode;
    [SerializeField] int amountOfCageExplodes = 5;
    ParticleSystem[] cageExplodes;

    Dictionary<Particle, ParticleSystem[]> typeToPool = new Dictionary<Particle, ParticleSystem[]>();

    void Start()
    {
        orbShatters = SpawnPool(amountOfShatters, orbShatter);
        typeToPool.Add(Particle.orb, orbShatters);

        explosions = SpawnPool(amountOfExplosions, explosion);
        typeToPool.Add(Particle.explosion, explosions);

        explosions = SpawnPool(amountOfDissolves, dissolve);
        typeToPool.Add(Particle.dissolve, explosions);

        cageExplodes = SpawnPool(amountOfCageExplodes, cageExplode);
        typeToPool.Add(Particle.cageExplode, cageExplodes);
    }

    ParticleSystem[] SpawnPool(int amount, ParticleSystem particle)
    {
        ParticleSystem[] pool = new ParticleSystem[amount];
        for(int i = 0; i < amount; i++)
        {
            pool[i] = Instantiate(particle, this.transform);
            pool[i].gameObject.SetActive(false);
        }
        return pool;
    }

    public void SpawnParticleAt(Vector3 position, Particle particleType, Quaternion rotation)
    {
        ParticleSystem[] pool = typeToPool[particleType];

        if(pool[0].IsAlive() == false)
        {
            pool[0].gameObject.SetActive(false);
            pool[0].transform.position = position;
            pool[0].transform.rotation = rotation;
            pool[0].gameObject.SetActive(true);
        } else
        {
            for(int i = 1; i < pool.Length; i++)
            {
                if(pool[i].IsAlive() == false)
                {
                    pool[i].gameObject.SetActive(false);
                    pool[i].transform.position = position;
                    pool[i].transform.rotation = rotation;
                    pool[i].gameObject.SetActive(true);
                    return;
                }
            }
        }

    }
}
