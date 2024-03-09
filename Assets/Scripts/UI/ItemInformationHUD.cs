using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemInformationHUD : MonoBehaviour
{
	public TextMeshProUGUI nameText;
	public Item item;

	public void DisplayInfo(Item otherItem)
	{
		if (item == null)
		{
			item = otherItem;
		}
		nameText.text = item.itemName;
	}

	public void OnItemButtonClicked()
	{
		if (item != null)
		{
			BattleSystem system = GameObject.FindGameObjectWithTag("BattleSystem").GetComponent<BattleSystem>();
			if (system != null)
			{
				system.UnitItemSelection(this);
			}
		}
	}
}
