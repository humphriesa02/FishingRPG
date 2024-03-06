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
	public Unit unit;

	[SerializeField] private Animator anim;

	private void Start()
	{
		if (anim == null)
		{
			anim = GetComponent<Animator>();
		}
	}

	// Initially called to assign the unit and
	// set all initial HUD values
	public void DisplayInfo(Unit otherUnit)
	{
		if (unit == null)
		{
			unit = otherUnit;
		}
		nameText.text = unit.unitName;
		hpText.text = "HP: " + unit.currentHealth + "/" + unit.maxHealth;
		staminaText.text = "SP: " + unit.stamina;
		fishingText.text = "FP: " + unit.fishingPower;
		playerImage.sprite = unit.hudImage;
	}

	// Since enemies in the HUD are just buttons,
	// when the button is clicked for an enemy this is called.
	// Call the attack unit functionality on the battle system
	public void TakeDamage()
	{
		BattleSystem system = GameObject.FindGameObjectWithTag("BattleSystem").GetComponent<BattleSystem>();
		if (system != null)
		{
			anim.SetTrigger("isTakingDamage");
			StartCoroutine(system.AttackUnit(this));
		}
	}

	public void PlayDeathAnim()
	{
		anim.SetTrigger("isDead");
	}

	// Simple health text updater function
	public void UpdateHealth()
	{
		if (unit != null && hpText != null)
		{
			hpText.text = unit.currentHealth + "/" + unit.maxHealth;
		}
	}
}
