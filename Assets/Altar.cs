using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar : MonoBehaviour
{
    [SerializeField] bool open = true;
    Collider collider => GetComponent<Collider>();
    Animator anim => GetComponentInParent<Animator>();
    [SerializeField] GameObject innerParticle;
    [SerializeField] GameObject innerActivatedParticle;

    private void Start()
    {
        SetOpen(open);
    }

    public void SetOpen(bool state)
    {
        open = state;
        anim.SetBool("open", state);
        innerParticle.SetActive(state);
        innerActivatedParticle.SetActive(!state);
        collider.isTrigger = state;
    }

    void OrbSubmit(Orb orb)
    {
        orb.Destruct();
        SetOpen(false);
        FindObjectOfType<CentralConsole>().Burn();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (open && other.GetComponent<Orb>())
        {
            OrbSubmit(other.GetComponent<Orb>());
        }
    }
}
