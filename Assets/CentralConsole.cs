using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralConsole : MonoBehaviour
{
    [SerializeField] Animator wireAnim;

    public void Burn()
    {
        wireAnim.SetTrigger("burn");
    }
}
