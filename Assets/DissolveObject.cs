using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveObject : MonoBehaviour
{
    [SerializeField] Renderer[] dissolvables;
    bool dissolving;
    ParticleManager particleManager;

    private void Start()
    {
        particleManager = FindObjectOfType<ParticleManager>();
    }

    public IEnumerator Dissolve(bool reverse = false)
    {
        particleManager.SpawnParticleAt(this.transform.position, Particle.dissolve, Quaternion.identity);
        float fadeTime = 0;
        while (fadeTime < 1)
        {
            float someValueFrom0To1;

            if (reverse)
                someValueFrom0To1 = 1 - (fadeTime / 1);
            else
                someValueFrom0To1 = fadeTime / 1;

            foreach (Renderer renderer in dissolvables)
            {
                foreach(Material material in renderer.materials)
                {
                    material.SetFloat("_dissolveamount", someValueFrom0To1);
                }
            }

            fadeTime += Time.deltaTime;

            yield return null;
        }

        dissolving = true;
    }

    public void ResetDissolve()
    {
        foreach (Renderer renderer in dissolvables)
        {
            foreach (Material material in renderer.materials)
            {
                material.SetFloat("_dissolveamount", 0);
            }
        }
    }
}
