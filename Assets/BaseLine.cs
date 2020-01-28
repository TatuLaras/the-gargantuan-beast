using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseLine : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float baselineY = -1f;
    [SerializeField] float movetoY = 10f;

    void Update()
    {
        if(player.position.y < baselineY) {
            player.position = new Vector3(player.position.x, movetoY, player.position.z);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(0, baselineY, 0), new Vector3(1000, 1, 1000));

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(new Vector3(0, movetoY, 0), new Vector3(500, 1, 500));

    }
}
