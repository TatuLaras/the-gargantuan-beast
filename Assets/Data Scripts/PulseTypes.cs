using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseTypes
{

    public class PulseType
    {
        public float duration;
        public float frequency;
        public float amplitude;

        public PulseType(float duration, float frequency, float amplitude)
        {
            this.duration = duration;
            this.frequency = frequency;
            this.amplitude = amplitude;
        }
    }

    // Pulse Types (duration, frequency, amplitude)
    public static PulseType basicTouch = new PulseType(0.01f, 300, 0.2f);
    public static PulseType grab = new PulseType(0.02f, 150, 0.3f);

}
