using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    Transform player;

    [SerializeField] AudioClip music;
    [SerializeField] AudioClip dieMusic;
    [SerializeField] AudioClip roarClip;
    [SerializeField] AudioClip[] additionalClips;
    [Space]
    [SerializeField] Transform radiusOrigin;
    [SerializeField] float radius = 200f;
    [SerializeField] bool distanceOverride;

    [HideInInspector] public bool dying = false;
    [HideInInspector] public bool inside = false;

    void Start()
    {
        player = FindObjectOfType<BodyController>().transform;
        if (radiusOrigin == null)
            radiusOrigin = this.transform;

    }

    void Update()
    {
        if((Vector3.Distance(player.position, radiusOrigin.position) < radius || distanceOverride == true) && inside == false)
        {
            StartCoroutine(Singleton.instance.audio.PlayClip(music, AudioType.music));
            Singleton.instance.audio.AddToQueue(AudioType.music, additionalClips);
            inside = true;
        } else if(Vector3.Distance(player.position, radiusOrigin.position) > radius && inside == true && dying == false)
        {
            Singleton.instance.audio.NoMusic();
            inside = false;
        }
    }

    public void PlayDyingMusic()
    {
        StartCoroutine(Singleton.instance.audio.PlayClip(dieMusic, AudioType.urgent));
        FindObjectOfType<SFXManager>().NewSFXAt(this.transform.position, roarClip, 1, 1000);
    }

    void OnDrawGizmos()
    {
        if(radiusOrigin != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(radiusOrigin.position, radius);
        } else
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(this.transform.position, radius);
        }
    }

}
