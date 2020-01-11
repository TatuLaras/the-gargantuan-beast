using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Weakspot : MonoBehaviour
{
    [SerializeField] GameObject rootObject;

    float bombAttachBoundX = 0.585f;
    float bombAttachBoundZ = 0.273f;

    [HideInInspector] public bool bombPlanted;
    [HideInInspector] public delegate void Damage();
    [HideInInspector] public Damage damage;

    public bool PlantBomb(InteractableObject item, Vector3 placeToPlant)
    {
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
        item.doNotInteract = true;


        bombPlanted = true;

        return true;
    }

    public void Explode()
    {
        rootObject.SetActive(false);

        if(damage != null)
            damage();
    }
}
