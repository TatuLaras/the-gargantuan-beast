using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColossusOne : MonoBehaviour
{
    Animator anim;

    [SerializeField] Weakspot[] weakspots;
    [SerializeField] int durability = 1;
    int explosionsEndured = 0;

    void Start()
    {
        anim = GetComponent<Animator>();

        foreach(Weakspot weakspot in weakspots)
        {
            //weakspot.explosionEvent.AddListener(CheckDie);
            weakspot.damage = CheckDie;
        }
    }

    void CheckDie()
    {
        print("Checkdie");
        explosionsEndured++;

        if(explosionsEndured >= durability)
        {
            Die();
        }
    }

    void Die()
    {
        anim.SetTrigger("Die");
    }
}
