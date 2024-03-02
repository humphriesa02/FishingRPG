using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
 * HUD representation of a party member
 */
public class UnitInformationHUD : MonoBehaviour
{
	public Image playerImage;
    public TextMeshProUGUI nameText;
	public TextMeshProUGUI hpText;
	public TextMeshProUGUI staminaText;
	public TextMeshProUGUI fishingText;
	private Unit thisUnit;

	public void DisplayInfo(Unit unit)
	{
		if (thisUnit == null)
		{
			thisUnit = unit;
		}
		nameText.text = unit.unitName;
		hpText.text = unit.currentHealth + "/" + unit.maxHealth;
		staminaText.text = unit.stamina.ToString();
		fishingText.text = unit.fishingPower.ToString();
		playerImage = unit.hudImage;
	}

	public void TakeDamage()
	{
		BattleSystem system = GameObject.FindGameObjectWithTag("BattleSystem").GetComponent<BattleSystem>();
		if (system != null)
		{
			// TODO fix this
			//system.AttackUnit(thisUnit);
		}
	}
}
