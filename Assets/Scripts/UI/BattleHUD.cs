using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/**
* Handles displaying all HUD elements related to the battle
*/
public class BattleHUD : MonoBehaviour
{
	// Dialogue Menu
	public GameObject dialogueContainer;
	public TextMeshProUGUI dialogueText;

	// Party menu
	public GameObject partyMenuContainer;
	public GameObject playerUnitInfoPrefab;
	private List<GameObject> playerUnitsHUD;

	// Enemy Menu
	public GameObject enemyMenuContainer;
	public GameObject enemyUnitInfoPrefab;
	private List<GameObject> enemyUnitsHUD;

	// Action Menu
	public GameObject actionMenuContainer;

	private BattleSystem battleSystem;

	// Sets up HUD related to the enemy and the player party
	public void SetupBattleHUD(List<Unit> enemyUnits, List<Unit> playerUnits, BattleSystem system)
	{
		playerUnitsHUD = new List<GameObject>();
		enemyUnitsHUD = new List<GameObject>();

		if (battleSystem == null)
		{
			battleSystem = system;
		}
		string enemyText = "A wild ";
		for (int i = 0; i < enemyUnits.Count; i++)
		{
			enemyText += enemyUnits[i].unitName;
			if (i != enemyUnits.Count - 1)
			{
				enemyText += ", ";
			}
		}
		enemyText += " approaches...";
		dialogueText.text = enemyText;

		// loop through units, create a HUD prefab, update the text
		foreach (Unit enemyUnit in enemyUnits)
		{
			GameObject enemy = Instantiate(enemyUnitInfoPrefab, enemyMenuContainer.transform);
			enemy.GetComponent<UnitInformationHUD>().DisplayInfo(enemyUnit);
			enemy.GetComponent<Button>().enabled = false;
			enemyUnitsHUD.Add(enemy);
		}

		// loop through units, create a HUD prefab, update the text
		foreach (Unit playerUnit in playerUnits)
		{
			GameObject player = Instantiate(playerUnitInfoPrefab, partyMenuContainer.transform);
			player.GetComponent<UnitInformationHUD>().DisplayInfo(playerUnit);
			playerUnitsHUD.Add(player);
		}

		SwapToDialogueMenu();
	}

	public void SwapToDialogueMenu()
	{
		actionMenuContainer.SetActive(false);
		dialogueContainer.SetActive(true);
	}

	public void SwapToActionMenu()
	{
		actionMenuContainer.SetActive(true);
		dialogueContainer.SetActive(false);
	}

	public void OnAttackButton()
	{
		if (battleSystem != null)
		{
			if (battleSystem.state != BattleState.PLAYERTURN)
			{
				return;
			}

			SelectEnemyToAttack();
		}
	}

	private void SelectEnemyToAttack()
	{
		SwapToDialogueMenu();
		dialogueText.text = "Select an enemy to attack";
		foreach (GameObject enemyUnitObj in enemyUnitsHUD)
		{
			enemyUnitObj.GetComponent<Button>().enabled = true;
		}
		EventSystem.current.SetSelectedGameObject(enemyUnitsHUD[0].gameObject);
	}
}
