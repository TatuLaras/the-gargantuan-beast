using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Weakspot : MonoBehaviour
{
    [SerializeField] GameObject rootObject;
    [SerializeField] bool exposed;
    [SerializeField] ExplodableCage cage;

    float bombAttachBoundX = 0.585f;
    float bombAttachBoundZ = 0.273f;

    [HideInInspector] public bool bombPlanted;
    [HideInInspector] public delegate void Damage();
    [HideInInspector] public Damage damage;

    public SpreadingFire[] conditionsToExpose;

    private void Start()
    {
       cage.gameObject.SetActive(!exposed);
    }

    private void Update()
    {
        CheckExpose();
    }

    public bool PlantBomb(InteractableObject item, Vector3 placeToPlant)
    {
        if(exposed == false)
        {
            return false;
        }

        if(item.objectType != ObjectType.bomb || item == null || bombPlanted == true)
        {
            return false;
        }

        Vector3 bombPos = new Vector3(placeToPlant.x, this.transform.position.y, placeToPlant.z);

        bombPos.x = Mathf.Clamp(bombPos.x, this.transform.position.x - bombAttachBoundX, this.transform.position.x + bombAttachBoundX);
        bombPos.z = Mathf.Clamp(bombPos.z, this.transform.position.z - bombAttachBoundZ, this.transform.position.z + bombAttachBoundZ);

        item.rootObject.transform.parent = this.transform;

        item.rootObject.transform.position = bombPos;
        item.rootObject.transform.rotation = Quaternion.identity;
        item.GetComponent<Bomb>().bombStrap.SetActive(true);
        item.GetComponent<Bomb>().attached = true;
        item.doNotInteract = true;


        bombPlanted = true;

        return true;
    }

    public void Explode()
    {
        if(exposed == false)
            return;

        rootObject.SetActive(false);

        if(damage != null)
            damage();
    }

    public void CheckExpose()
    {
        if (exposed == true)
            return;

        bool explode = false;

        foreach(SpreadingFire condition in conditionsToExpose)
        {
            if (condition.ablaze == false)
            {
                explode = false;
                break;
            } else
            {
                explode = true;
            }
        }


        if(explode == true)
        {
            cage.Explode();
            exposed = true;
        }
    }
}
