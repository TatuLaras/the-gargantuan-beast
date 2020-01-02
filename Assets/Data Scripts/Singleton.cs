using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Singleton : MonoBehaviour
{
    public static Singleton instance;

    public SteamVR_Input_Sources leftHand;
    public SteamVR_Input_Sources rightHand;

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

}
