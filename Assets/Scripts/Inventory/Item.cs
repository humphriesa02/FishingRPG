using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public string description;
    public AudioClip itemUseSound;

    public virtual void UseItem(Unit unitToUseItem)
    {
        Debug.Log("Using " +  itemName);
    }
}
