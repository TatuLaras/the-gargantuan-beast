using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColossusOne : MonoBehaviour
{
    Animator anim;

    [SerializeField] Weakspot[] weakspots;
    [SerializeField] int durability = 1;
    int explosionsEndured = 0;

    [HideInInspector] public int stepsTakenSinceLastTurn = 0;
    [SerializeField] int maxStepsBeforeTurn = 3;
    [SerializeField] GameObject armature;

   
    void Start()
    {
        anim = GetComponent<Animator>();

        foreach(Weakspot weakspot in weakspots)
        {
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

    public void WalkCycle()
    {
        // Things you want to do per walk cycle
    }
}
