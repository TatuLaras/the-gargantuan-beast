using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Locomotion : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float speed = 5f;
    [SerializeField] float walkCircleRadius = 0.3f;
    [SerializeField] SteamVR_Action_Vector2 joystick;

    BodyController bodyController;
    Vector3 playerTargetPos;

    void Start()
    {
        playerTargetPos = player.transform.position;
        if (player != null)
            player = this.transform.parent.gameObject;

        bodyController = player.GetComponent<BodyController>();
    }

    void Update()
    {
        float x = joystick.axis.x;
        float y = joystick.axis.y;

        if (bodyController.climbing == false && bodyController.grounded == true)
        { 
            Rigidbody playerRb = player.GetComponent<Rigidbody>();
            playerTargetPos += (transform.right * x + transform.forward * y) * Time.deltaTime * 5;

            playerTargetPos = new Vector3(  Mathf.Clamp(playerTargetPos.x, player.transform.position.x - walkCircleRadius,
                                            player.transform.position.x + walkCircleRadius),
                                            playerTargetPos.y,
                                            Mathf.Clamp(playerTargetPos.z, player.transform.position.z - walkCircleRadius,
                                            player.transform.position.z + walkCircleRadius) 
                                        );

            Vector3 playerVelocity = (playerTargetPos - player.transform.position) * speed;
            // Exclude y axis
            playerVelocity = new Vector3(playerVelocity.x, playerRb.velocity.y, playerVelocity.z);
            playerRb.velocity = playerVelocity;
        } else
        {
            playerTargetPos = player.transform.position;
        }
    }
}
