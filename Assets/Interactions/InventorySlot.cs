using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public InteractableObject itemInSlot;
    InteractableObject permanentItemInSlot;
    bool forPermanent = false;


    private void Start()
    {
        if (itemInSlot != null)
        {
            itemInSlot.rootObject.transform.position = this.transform.position;
            itemInSlot.rootObject.transform.parent = this.transform;
            itemInSlot.rootObject.SetActive(false);
            itemInSlot.slot = this;
            forPermanent = itemInSlot.permanent;
            permanentItemInSlot = itemInSlot;
        }
    }

    public bool PutItemInSlot(InteractableObject item)
    {
        if(forPermanent == true && item != permanentItemInSlot)
        {
            return false;
        }

        if(itemInSlot != null || item == null)
        {
            return false;
        }

        itemInSlot = item;

        item.rootObject.transform.position = this.transform.position;
        item.rootObject.transform.rotation = this.transform.rotation;
        item.rootObject.transform.parent = this.transform;
        item.rootObject.SetActive(false);
        item.rb.isKinematic = true;
        item.rb.useGravity = false;


        return true;
    }

    public InteractableObject TakeItemFromSlot()
    {
        if(itemInSlot == null)
            return null;

        if (itemInSlot.objectType == ObjectType.paraglider && FindObjectOfType<Legs>().grounded)
            return null;

        itemInSlot.rootObject.SetActive(true);
        itemInSlot.rootObject.transform.parent = null;

        InteractableObject itemToReturn = itemInSlot;
        itemInSlot = null;

        return itemToReturn;
    }
}
