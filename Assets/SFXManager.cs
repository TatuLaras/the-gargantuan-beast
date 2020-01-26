using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    GameObject[] sources;
    [SerializeField] GameObject sourcePrefab;
    [SerializeField] int sfxPoolSize = 5;

    [SerializeField] AudioClip testClip;
    void Start()
    {
        sources = new GameObject[sfxPoolSize];
        for(int i = 0; i < sources.Length; i++)
        {
            sources[i] = Instantiate(sourcePrefab, this.transform);
        }

        NewSFXAt(new Vector3(9.401072f, 426.4f, -277.8f), testClip, 1);
    }

    public void NewSFXAt(Vector3 location, AudioClip clip, float volume, float minDistance = 1, float maxDistance = 500)
    {
        foreach(GameObject source in sources)
        {
            AudioSource sourceComponent = source.GetComponent<AudioSource>();
            if(sourceComponent.isPlaying == false)
            {
                source.SetActive(false);
                source.transform.position = location;
                sourceComponent.clip = clip;
                sourceComponent.volume = volume;
                sourceComponent.minDistance = minDistance;
                sourceComponent.maxDistance = maxDistance;
                source.SetActive(true);

                break;
            }
        }
    }
}
