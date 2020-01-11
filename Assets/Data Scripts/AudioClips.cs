using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClips : MonoBehaviour
{

    [SerializeField] AudioClip grabAudio;

    [HideInInspector] public static AudioClip grab;


    private void Awake()
    {
        grab = grabAudio;
    }
}
