using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Weakspot : MonoBehaviour
{
    [SerializeField] GameObject bomb;
    [SerializeField] GameObject rootObject;
    [SerializeField] ParticleSystem explosionParticles;

    float fuseLenghtSeconds = 10;
    float bombAttachBoundX = 0.585f;
    float bombAttachBoundZ = 0.273f;

    [HideInInspector] public bool bombPlanted;
    [HideInInspector] public delegate void Damage();

    public Damage damage;

    void Start()
    {
        if (bomb == null)
        {
            Debug.LogError("Assign a bomb object for the weakspot", this.gameObject);
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPaused = true;
#endif
        }

        if (explosionParticles == null)
        {
            Debug.LogError("Assign a particle system for the weakspot", this.gameObject);
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPaused = true;
#endif
        }

        if (rootObject == null)
        {
            Debug.LogError("Assign a root object for the weakspot", this.gameObject);
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPaused = true;
#endif
        }


        bomb.SetActive(false);
        explosionParticles.gameObject.SetActive(false);
    }

    public bool PlantBomb(InteractableObject item, Vector3 placeToPlant)
    {
        if(item.objectType != ObjectType.bomb || item == null || bombPlanted == true)
        {
            return false;
        }

        Destroy(item.rootObject);
        bomb.SetActive(true);

        Vector3 bombPos = new Vector3(placeToPlant.x, bomb.transform.position.y, placeToPlant.z);

        bombPos.x = Mathf.Clamp(bombPos.x, this.transform.position.x - bombAttachBoundX, this.transform.position.x + bombAttachBoundX);
        bombPos.z = Mathf.Clamp(bombPos.z, this.transform.position.z - bombAttachBoundZ, this.transform.position.z + bombAttachBoundZ);

        bomb.transform.position = bombPos;
        bombPlanted = true;

        Invoke("Explode", fuseLenghtSeconds);

        return true;
    }

    void Explode()
    {
        bomb.SetActive(false);
        explosionParticles.gameObject.SetActive(true);
        explosionParticles.Play();
        rootObject.SetActive(false);
        damage();
    }
}
