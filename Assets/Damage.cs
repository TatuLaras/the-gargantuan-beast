using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Damage : MonoBehaviour
{
    Animator anim;
    MusicTrigger music;

    [SerializeField] Weakspot[] weakspots;
    [SerializeField] int durability = 1;
    [SerializeField] int explosionsEndured = 0;

    void Start()
    {
        anim = GetComponent<Animator>();
        music = GetComponent<MusicTrigger>();

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
        music.dying = true;
        music.PlayDyingMusic();
    }
}
