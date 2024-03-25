using System.Collections;
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

	public void SelectUnit()
	{
		BattleSystem system = GameObject.FindGameObjectWithTag("BattleSystem").GetComponent<BattleSystem>();
		if (system != null)
		{
			system.SelectUnit(this);
		}
	}

	public void GiveItem()
	{
		BattleSystem system = GameObject.FindGameObjectWithTag("BattleSystem").GetComponent<BattleSystem>();
		if (system != null)
		{
			StartCoroutine(system.UseItem(this));
		}
	}

	public void PlayDeathAnim()
	{
		anim.SetTrigger("isDead");
	}

	public void PlayKnockoutAnim(bool doPlay)
	{
		anim.SetBool("isKnockedOut", doPlay);
	}

	public void PlayDamageAnim()
	{
		anim.SetTrigger("isTakingDamage");
	}

	public void IsUnitsTurn(bool isUnitsTurn)
	{
		anim.SetBool("isUnitsTurn", isUnitsTurn);
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
