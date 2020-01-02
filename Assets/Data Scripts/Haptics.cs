using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Haptics : MonoBehaviour
{
    [SerializeField] SteamVR_Action_Vibration haptics;

    public void Pulse(PulseTypes.PulseType pulseType, SteamVR_Input_Sources source)
    {
        haptics.Execute(0, pulseType.duration, pulseType.frequency, pulseType.amplitude, source);
    }
}
