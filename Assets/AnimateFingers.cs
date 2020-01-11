using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateFingers : MonoBehaviour
{
    Animator anim;
    FingerInput fingerInput;

    void Start()
    {
        anim = GetComponent<Animator>();
        fingerInput = GetComponent<FingerInput>();
    }

    void Update()
    {
        anim.SetFloat("thumb", fingerInput.fingers.GetFingerCurl(0));
        anim.SetFloat("index", fingerInput.fingers.GetFingerCurl(1));
        anim.SetFloat("middle", fingerInput.fingers.GetFingerCurl(2));
        anim.SetFloat("ring", fingerInput.fingers.GetFingerCurl(3));
        anim.SetFloat("pinky", fingerInput.fingers.GetFingerCurl(4));
    }
}
