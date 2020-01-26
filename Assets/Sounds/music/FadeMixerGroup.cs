using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class FadeMixerGroup : MonoBehaviour
{

    public bool fading;
    public bool loopForever;
    public string exposedParam;
    public List<AudioClip> queue;

    int queuePointer = 0;
    AudioSource audioSource;
    AudioClip currentSingleClip;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();

    }

    public IEnumerator StartFade(AudioMixer audioMixer, float duration, float targetVolume)
    {
        fading = true;

        float currentTime = 0;
        float currentVol;
        audioMixer.GetFloat(exposedParam, out currentVol);
        currentVol = Mathf.Pow(10, currentVol / 20);
        float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
            audioMixer.SetFloat(exposedParam, Mathf.Log10(newVol) * 20);
            yield return null;
        }

        fading = false;

        yield break;
    }

    public IEnumerator FadeTo(AudioMixer audioMixer, float duration, AudioClip clip)
    {
        fading = true;

        float currentTime = 0;
        float currentVol;
        audioMixer.GetFloat(exposedParam, out currentVol);
        currentVol = Mathf.Pow(10, currentVol / 20);
        float targetValue = Mathf.Clamp(0, 0.0001f, 1);// targvol

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
            audioMixer.SetFloat(exposedParam, Mathf.Log10(newVol) * 20);
            yield return null;
        }

        audioSource.clip = clip;
        currentSingleClip = clip;
        audioSource.Play();
        print("Now playing: " + clip.name);
        yield return new WaitForSeconds(0.2f);

        currentTime = 0;
        audioMixer.GetFloat(exposedParam, out currentVol);
        currentVol = Mathf.Pow(10, currentVol / 20);
        targetValue = Mathf.Clamp(1, 0.0001f, 1);// targvol

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
            audioMixer.SetFloat(exposedParam, Mathf.Log10(newVol) * 20);
            yield return null;
        }

        fading = false;

        yield break;
    }

    public void SwitchClip(AudioClip clip)
    {
        audioSource.clip = clip;
        currentSingleClip = clip;
        audioSource.Play();
        print("Now playing: " + clip.name);
    }

    public void SetVolume(AudioMixer audioMixer, float volume)
    {
        audioMixer.SetFloat(exposedParam, volume);
    }

    public void EmptyQueue()
    {
        queue = new List<AudioClip>();
        queuePointer = 0;
    }

    public void NextInQueue()
    {
        if (fading == true)
            return;

        if(queue.Count > queuePointer)
        {
            SwitchClip(queue[queuePointer]);
            queuePointer++;

        } else if(loopForever == true)
        {
            if(queue.Count > 0)
            {
                queuePointer = 0;
                NextInQueue();
            } else if (currentSingleClip != null)
            {
                SwitchClip(currentSingleClip);
            }
        }
    }
}