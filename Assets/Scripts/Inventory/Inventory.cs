using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    public void AddItem(Item newItem)
    {
        items.Add(newItem);
    }

    public void RemoveItem(Item itemToRemove)
    {
        items.Remove(itemToRemove);
    }

    public void UseSelectedItem(Item item, Unit unitToUseItem)
    {
        if (items.Contains(item))
        {
            print("Using Item");
            item.UseItem(unitToUseItem);
            RemoveItem(item);
        }
    }

    public List<Item> GetInventoryItems()
    {
        return items;
    }
}
