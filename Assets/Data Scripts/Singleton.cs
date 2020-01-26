using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Singleton : MonoBehaviour
{
    public static Singleton instance;

    public SteamVR_Input_Sources leftHand;
    public SteamVR_Input_Sources rightHand;

    public AudioClip windClip;
    public AudioClip bombClip;

    [HideInInspector] public AudioManager audio;

    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        } else
        {
            instance = this;
        }

    }

    private void Start()
    {
        audio = GetComponent<AudioManager>();
        StartCoroutine(audio.PlayClip(windClip, AudioType.ambient));

    }

}
