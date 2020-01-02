using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public InteractableObject itemInSlot = null;

    public bool PutItemInSlot(InteractableObject item)
    {
        if(itemInSlot != null || item == null)
        {
            return false;
        }

        itemInSlot = item;

        item.rootObject.transform.position = this.transform.position;
        item.rootObject.transform.parent = this.transform;
        item.rootObject.SetActive(false);

        return true;
    }

    public InteractableObject TakeItemFromSlot()
    {
        if(itemInSlot == null)
        {
            return null;
        }

        itemInSlot.rootObject.SetActive(true);
        itemInSlot.rootObject.transform.parent = null;

        InteractableObject itemToReturn = itemInSlot;
        itemInSlot = null;

        return itemToReturn;
    }
}
