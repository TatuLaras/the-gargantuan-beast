﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;

public class HandController : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] AudioClip grabAudio;

    [HideInInspector] public bool climbing = false;
    [HideInInspector] public BodyController bodyController;

    Haptics haptics;
    DetectInteractions radius;
    FingerInput fingerInput;
    ObjectInteraction objectInteraction;
    HandAudio audio;
    SFXManager sfx;

    SteamVR_Behaviour_Pose pose;

    void Start()
    {
        if(player == null)
            player = this.transform.parent.gameObject;

        bodyController = player.GetComponent<BodyController>();
        pose = GetComponent<SteamVR_Behaviour_Pose>();

        haptics = GetComponent<Haptics>();
        radius = GetComponentInChildren<DetectInteractions>();
        fingerInput = GetComponent<FingerInput>();
        objectInteraction = GetComponent<ObjectInteraction>();
        audio = GetComponent<HandAudio>();
        sfx = FindObjectOfType<SFXManager>();
    }

    void FixedUpdate()
    {
        if (radius.climbable != null && fingerInput.grabbing && !climbing && objectInteraction.objectInHand == null)
        {
            bodyController.GetComponent<Climbing>().GrabOnWall(this.gameObject, radius.climbable);
            climbing = true;

            haptics.Pulse(PulseTypes.grab, pose.inputSource);
            sfx.NewSFXAt(this.transform.position, grabAudio, 1);
        }
    }


}
