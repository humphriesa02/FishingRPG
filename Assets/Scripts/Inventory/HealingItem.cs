using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Healing Item", menuName = "Inventory/Healing Item")]
public class HealingItem : Item
{
    public int healingAmount;

	public override void UseItem(Unit unitToUseItem)
	{
		base.UseItem(unitToUseItem);
		unitToUseItem.currentHealth = Mathf.Clamp(unitToUseItem.currentHealth + healingAmount, 0, unitToUseItem.maxHealth);
		Debug.Log("New health after item usage: " + unitToUseItem.currentHealth);
	}
}
