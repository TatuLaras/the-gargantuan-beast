using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Singleton : MonoBehaviour
{
    public static Singleton instance;

    public SteamVR_Input_Sources leftHand;
    public SteamVR_Input_Sources rightHand;

    public SteamVR_Action_Boolean triggerPress;

    public AudioClip windClip;
    public AudioClip bombClip;

    [SerializeField] GameObject leftHandObj;
    [SerializeField] GameObject rightHandObj;

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

    private void Update()
    {
        if (SteamVR_Input.GetStateDown("trigger", leftHand))
        {
            leftHandObj.GetComponent<FingerInput>().TriggerPress();
        }

        if (SteamVR_Input.GetStateDown("trigger", rightHand))
        {
            rightHandObj.GetComponent<FingerInput>().TriggerPress();
        }
    }

    private void Start()
    {
        audio = GetComponent<AudioManager>();
        StartCoroutine(audio.PlayClip(windClip, AudioType.ambient));

    }

}
