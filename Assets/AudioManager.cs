using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    AudioSource ambientSource;
    [Space]
    [SerializeField] AudioMixerGroup ambientMixerGroup;
    [SerializeField] string exposedAmbientParam = "ambientVol";
    [SerializeField] bool loopAmbient = false;
    [SerializeField] float ambientVolume = 0.5f;

    AudioSource musicSource;
    [Space]
    [SerializeField] AudioMixerGroup musicMixerGroup;
    [SerializeField] string exposedMusicParam = "musicVol";
    [SerializeField] bool loopMusic = false;
    [SerializeField] float musicVolume = 1f;

    AudioSource urgentSource;
    [Space]
    [SerializeField] AudioMixerGroup urgentMixerGroup;
    [SerializeField] string exposedUrgentParam = "urgentVol";
    [SerializeField] bool loopUrgent = false;
    [SerializeField] float urgentVolume = 1f;

    AudioSource activeSource;
    Dictionary<AudioType, AudioSource> typeToSource = new Dictionary<AudioType, AudioSource>();
    Dictionary<AudioSource, FadeMixerGroup> sourceToFader = new Dictionary<AudioSource, FadeMixerGroup>();

    [Space]

    // Inspector
    [SerializeField] AudioMixer mixer;
    [SerializeField] GameObject sourcePrefab;

    public bool doNotInterrupt = false;

    AudioClip currentClip;

    void Awake()
    {
        // Ambient
        ambientSource = InstantiateSource(exposedAmbientParam, "ambient");
        ambientSource.outputAudioMixerGroup = ambientMixerGroup;
        ambientSource.GetComponent<FadeMixerGroup>().loopForever = loopAmbient;
        typeToSource.Add(AudioType.ambient, ambientSource);
        sourceToFader.Add(ambientSource, ambientSource.GetComponent<FadeMixerGroup>());
        ambientSource.volume = ambientVolume;

        // Music
        musicSource = InstantiateSource(exposedMusicParam, "music");
        musicSource.outputAudioMixerGroup = musicMixerGroup;
        musicSource.GetComponent<FadeMixerGroup>().loopForever = loopMusic;
        typeToSource.Add(AudioType.music, musicSource);
        sourceToFader.Add(musicSource, musicSource.GetComponent<FadeMixerGroup>());
        musicSource.volume = musicVolume;

        // Urgent
        urgentSource = InstantiateSource(exposedUrgentParam, "urgent");
        urgentSource.outputAudioMixerGroup = urgentMixerGroup;
        urgentSource.GetComponent<FadeMixerGroup>().loopForever = loopUrgent;
        typeToSource.Add(AudioType.urgent, urgentSource);
        sourceToFader.Add(urgentSource, urgentSource.GetComponent<FadeMixerGroup>());
        urgentSource.volume = urgentVolume;

    }

    AudioSource InstantiateSource(string exposedParam, string name)
    {
        GameObject sourceObj = Instantiate(sourcePrefab, this.transform);
        sourceObj.name = name;
        sourceObj.GetComponent<FadeMixerGroup>().exposedParam = exposedParam;
        AudioSource source = sourceObj.GetComponent<AudioSource>();
        return source;
    }

    //void SetupSource()

    void LateUpdate()
    {
        if(activeSource != null)
        {
            if(activeSource.isPlaying == false)
            {
                sourceToFader[activeSource].NextInQueue();
            }    
        }
    }

    public IEnumerator PlayClip(AudioClip clip, AudioType audiotype, bool highPriority = false)
    {
        while ((musicSource.GetComponent<FadeMixerGroup>().fading ||
            ambientSource.GetComponent<FadeMixerGroup>().fading ||
            urgentSource.GetComponent<FadeMixerGroup>().fading ||
            doNotInterrupt == true) && highPriority == false)
        {
            yield return new WaitForSeconds(0.5f);
        }

        switch (audiotype)
        {
            case AudioType.ambient:

                if(activeSource != ambientSource)
                {
                    DisableTrack(typeToSource[AudioType.music]);
                    DisableTrack(typeToSource[AudioType.urgent]);
                    activeSource = ambientSource;
                }

                break;


            case AudioType.music:

                if (activeSource != musicSource)
                {
                    DisableTrack(typeToSource[AudioType.ambient]);
                    DisableTrack(typeToSource[AudioType.urgent]);
                    activeSource = musicSource;
                }

                break;


            case AudioType.urgent:

                if (activeSource != urgentSource)
                {
                    DisableTrack(typeToSource[AudioType.music]);
                    DisableTrack(typeToSource[AudioType.ambient]);
                    activeSource = urgentSource;
                }

                break;
        }

        StartCoroutine(activeSource.GetComponent<FadeMixerGroup>().FadeTo(mixer, 1f, clip));

    }

    public void AddToQueue(AudioType audioType, AudioClip[] clips)
    {
        if(typeToSource[audioType] == activeSource)
            sourceToFader[typeToSource[audioType]].queue.AddRange(clips);
    }

    void DisableTrack(AudioSource source)
    {
        StartCoroutine(sourceToFader[source].StartFade(mixer, 2f, 0));
        sourceToFader[source].EmptyQueue();
    }

    public void NoMusic()
    {
        if(activeSource == musicSource)
        {
            StartCoroutine(PlayClip(Singleton.instance.windClip, AudioType.ambient));
        }
    }

    public void AdjustVolumeBasedOnDistance(float distance, float loudestDistance, float quietestDistance, float minVolume)
    {
        float percentage = Mathf.InverseLerp(quietestDistance, loudestDistance, Mathf.Clamp(distance, loudestDistance, quietestDistance));
        percentage = Mathf.Clamp(percentage, minVolume, 1f);
        float volume = percentage * 80;
        volume = -80 + volume;

        sourceToFader[activeSource].SetVolume(mixer, volume);
    }

}

