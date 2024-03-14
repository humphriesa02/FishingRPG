using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Key Item", menuName = "Inventory/Key Item")]
public class KeyItem : Item
{
	UnityEvent unityEvent;

	public override void UseItem(Unit unitToUseItem)
	{
		base.UseItem(unitToUseItem);
		if (unityEvent != null)
		{
			unityEvent.Invoke();
		}
	}
}
