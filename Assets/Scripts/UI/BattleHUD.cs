using System.Collections;
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
	public GameObject attackButton;

	private GameObject lastSelectedButton;

	// Reference to the battle system
	private BattleSystem battleSystem;

	// selecting mode
	bool selectingEnemyMode = false;

	// Sets up HUD related to the enemy and the player party
	public void SetupBattleHUD(List<Unit> enemyUnits, List<Unit> playerUnits, BattleSystem system)
	{
		// Empties out the list of player and enemy HUD objects
		playerUnitsHUD = new List<GameObject>();
		enemyUnitsHUD = new List<GameObject>();

		selectingEnemyMode = false;

		// Assign battle system
		if (battleSystem == null)
		{
			battleSystem = system;
		}

		// Starting dialogue setup
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
			// Ensure the buttons aren't clickable to start
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

		// Start off in dialogue to display starting dialogue
		SwapToDialogueMenu();
	}

	private void Update()
	{
		if (selectingEnemyMode)
		{
			if (Input.GetButton("Cancel"))
			{
				ToggleEnemyButtons(false);
				SwapToActionMenu();
				selectingEnemyMode = false;
			}
		}

		if (EventSystem.current.currentSelectedGameObject == null)
		{
			if (lastSelectedButton != null)
			{
				EventSystem.current.SetSelectedGameObject(lastSelectedButton);
			}
			else
			{
				EventSystem.current.SetSelectedGameObject(attackButton);
			}
		}
	}

	// To change from Action menu to Dialogue menu
	public void SwapToDialogueMenu()
	{
		actionMenuContainer.SetActive(false);
		dialogueContainer.SetActive(true);
	}

	public void SetDialogueText(string text)
	{
		dialogueText.text = text;
	}

	// To change from Dialogue menu to Action menu
	public void SwapToActionMenu()
	{
		actionMenuContainer.SetActive(true);
		EventSystem.current.SetSelectedGameObject(attackButton);
		lastSelectedButton = attackButton;
		dialogueContainer.SetActive(false);
	}

	// When the "Attack" option of the action menu is selected
	// Ensure we are in PlayerTurn state, then allow us to select an enemy
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

	public void OnRunButtonClicked()
	{
		if (battleSystem != null)
		{
			bool runSuccess = battleSystem.AttemptToRun();
			SwapToDialogueMenu();
			if (runSuccess)
			{
				battleSystem.state = BattleState.RAN;
				StartCoroutine(battleSystem.EndBattle());
			}
			else
			{
				StartCoroutine(RunFailed());
			}
		}
	}

	IEnumerator RunFailed()
	{
		SetDialogueText("Could not escape!");
		yield return new WaitForSeconds(2f);
		battleSystem.NextPartyMemberTurn();
	}

	// Set dialogue then enable all the enemy buttons
	private void SelectEnemyToAttack()
	{
		selectingEnemyMode = true;
		SwapToDialogueMenu();
		SetDialogueText("Select an enemy to attack");
		ToggleEnemyButtons(true);
		EventSystem.current.SetSelectedGameObject(enemyUnitsHUD[0].gameObject);
		lastSelectedButton = enemyUnitsHUD[0].gameObject;
	}

	public void ToggleEnemyButtons(bool isOn)
	{
		foreach (GameObject enemyUnitObj in enemyUnitsHUD)
		{
			enemyUnitObj.GetComponent<Button>().enabled = isOn;
		}
	}

	public UnitInformationHUD SelectRandomPartyMember()
	{
		int randIndex = Random.Range(0, playerUnitsHUD.Count);
		return playerUnitsHUD[randIndex].GetComponent<UnitInformationHUD>();
	}

	public IEnumerator RemoveEnemyFromHUD(UnitInformationHUD enemyUnit)
	{
		if (enemyUnitsHUD.Contains(enemyUnit.gameObject))
		{
			enemyUnitsHUD.Remove(enemyUnit.gameObject);
			enemyUnit.PlayDeathAnim();

			yield return new WaitForSeconds(1f);

			Destroy(enemyUnit.gameObject);
		}
	}

	public void RemovePlayerFromHUD(UnitInformationHUD playerUnit)
	{
		if (playerUnitsHUD.Contains(playerUnit.gameObject))
		{
			// Show dead icons or something
		}
	}

	public void TearDownMenu()
	{
		foreach(GameObject enemyUnitObj in enemyUnitsHUD)
		{
			Destroy(enemyUnitObj);
		}
		enemyUnitsHUD.Clear();

		foreach(GameObject playerUnitObj in playerUnitsHUD)
		{
			Destroy(playerUnitObj);
		}
		enemyUnitsHUD.Clear();

		ToggleEnemyButtons(false);
		SwapToActionMenu();
		dialogueText.text = "";
	}

	public void DisplayCurrentTurnPlayerUnit(Unit activeUnit)
	{
		foreach (GameObject playerUnitObj in playerUnitsHUD)
		{
			if (playerUnitObj.GetComponent<UnitInformationHUD>() != null && playerUnitObj.GetComponent<UnitInformationHUD>().unit == activeUnit)
			{
				playerUnitObj.GetComponent<UnitInformationHUD>().IsUnitsTurn(true);
			}
			else
			{
				playerUnitObj.GetComponent<UnitInformationHUD>().IsUnitsTurn(false);
			}
		}
	}

	public void DisplayCurrentTurnEnemyUnit(Unit activeUnit)
	{
		foreach (GameObject enemyUnitObj in enemyUnitsHUD)
		{
			if (enemyUnitObj.GetComponent<UnitInformationHUD>() != null && enemyUnitObj.GetComponent<UnitInformationHUD>().unit == activeUnit)
			{
				enemyUnitObj.GetComponent<UnitInformationHUD>().IsUnitsTurn(true);
			}
			else
			{
				enemyUnitObj.GetComponent<UnitInformationHUD>().IsUnitsTurn(false);
			}
		}
	}
}
