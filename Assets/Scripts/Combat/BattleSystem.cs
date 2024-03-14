using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

// Different states of the battle
public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST, RAN }

/**
 * Handles logic for a turn based battle
 */
public class BattleSystem : MonoBehaviour
{
	// State of the battle
    public BattleState state;

	// List of player units
	private List<Unit> playerUnits;
	private int currentPlayerIndex;

	// List of Enemy units
	private List<Unit> enemyUnits;
	private int currentEnemyIndex;

	// Item to be used on ally
	private ItemInformationHUD selectedItem;

	private BattleHUD battleHUD;
	public string returnSceneName;

	void Start()
    {
		// Start in the 'start' state
        state = BattleState.START;
		battleHUD = GameObject.FindGameObjectWithTag("BattleHUD").GetComponent<BattleHUD>();
        StartCoroutine(SetupBattle());
    }

	// Called when the battle system is first started for the battle scene
	// Logic for setting up the battle, not HUD related
	IEnumerator SetupBattle()
    {
		// Collect the list of player units
		playerUnits = new List<Unit>();
		GameObject[] playerUnitGameObjects = GameObject.FindGameObjectsWithTag("PlayerUnit");

		for (int i = 0; i < playerUnitGameObjects.Length; i++)
		{
			Unit currentUnit = playerUnitGameObjects[i].GetComponent<Unit>();
			playerUnits.Add(currentUnit);
		}

		// Collect the list of enemy units
		enemyUnits = new List<Unit>();
		GameObject[] enemyUnitGameObjects = GameObject.FindGameObjectsWithTag("EnemyUnit");

		for (int i = 0; i < enemyUnitGameObjects.Length; i++)
		{
			Unit currentUnit = enemyUnitGameObjects[i].GetComponent<Unit>();
			enemyUnits.Add(currentUnit);
		}

		List<Item> items = GameManager.Instance.PlayerManager.inventory.GetInventoryItems();

		// Set up the UI
		battleHUD.SetupBattleHUD(enemyUnits, playerUnits, items, this);

		// Wait before starting the battle
		yield return new WaitForSeconds(1f);

		// Start the player's turn
		state = BattleState.PLAYERTURN;
		PlayerTurn(0);
	}

	public IEnumerator EndBattle()
	{
		if (state == BattleState.WON)
		{
			battleHUD.SetDialogueText("You won the battle! Experience gained!");
		}
		else if (state == BattleState.LOST)
		{
			battleHUD.SetDialogueText("Your party has all been knocked out!");
		}
		else if (state == BattleState.RAN)
		{
			battleHUD.SetDialogueText("You have successfully escaped");
		}
		// Wait before leaving the battle and returning to the previous scene
		yield return new WaitForSeconds(2.5f);

		// Leave battle section
		if (returnSceneName != null || returnSceneName != "")
		{
			battleHUD.TearDownMenu();
			SceneManager.LoadScene(returnSceneName);
		}
	}

	// For now just swaps to the action menu
	void PlayerTurn(int playerIndex = 0)
	{
		currentPlayerIndex = playerIndex;
		// Display the player who's turn it is
		battleHUD.DisplayCurrentTurnPlayerUnit(playerUnits[currentPlayerIndex]);

		battleHUD.SwapToActionMenu();

		/// Player attacking functionality happens in HUD,
		/// refer to BattleHUD.cs as well as UnitInformationHUD.cs
	}

	public void EnemyTurn(int enemyIndex = 0)
	{
		print("Enemy turn!");
		currentEnemyIndex = enemyIndex;
		battleHUD.DisplayCurrentTurnEnemyUnit(enemyUnits[currentEnemyIndex]);
		battleHUD.SwapToDialogueMenu();

		// Find a random player to attack
		UnitInformationHUD randomPartyUnit = battleHUD.SelectRandomPartyMember();
		int numberOfTries = 0;
		while (randomPartyUnit.unit.isDead && numberOfTries < 10)
		{
			randomPartyUnit = battleHUD.SelectRandomPartyMember();
			numberOfTries++;
		}
		if (numberOfTries >= 10)
		{
			StartCoroutine(EndBattle());
			Debug.LogError("Cannot find a non-dead enemy. Should have ended battle!");
		}
		else
		{
			StartCoroutine(AttackUnit(randomPartyUnit));
		}
	}

	// When the HUD has selected an enemy to attack,
	// actual logic for attacking an enemy, can also process
	// changing turns
	public IEnumerator AttackUnit(UnitInformationHUD unitToAttackHUD)
	{
		battleHUD.ToggleEnemyButtons(false); // Whether it's player or enemy attacking, disable buttons
		// See if it's player attacking or enemy
		if (state == BattleState.PLAYERTURN) // Players are attacking
		{
			string battleText = playerUnits[currentPlayerIndex].unitName + " attacks " + unitToAttackHUD.unit.unitName + "!";
			battleHUD.SetDialogueText(battleText);
			unitToAttackHUD.PlayDamageAnim();

			// Wait after setting the attacking text before actually doing damage
			yield return new WaitForSeconds(1f);
			
			// Apply damage to the HUD's unit variable
			bool isDead = unitToAttackHUD.unit.TakeDamage(playerUnits[currentPlayerIndex].damage);

			battleText = unitToAttackHUD.unit.unitName + " takes " + playerUnits[currentPlayerIndex].damage + " damage!";
			battleHUD.SetDialogueText(battleText);
			// Update the HUD health of the unit
			unitToAttackHUD.UpdateHealth();

			// Wait
			print("We are in player state");
			if (isDead)
			{
				// If the unit is killed, wait a very small amount before
				// showing that the enemy has died
				yield return new WaitForSeconds(1f);
				print("Killed the enemy");
				StartCoroutine(EnemyEliminated(unitToAttackHUD));
			}
			else
			{
				// Otherwise if not killed, wait a little longer before
				// moving to the next turn
				yield return new WaitForSeconds(1f);
				print("Did not kill the enemy");
				NextPartyMemberTurn();
			}
			
		}
		else if (state == BattleState.ENEMYTURN) // Enemies are attacking
		{
			print("We are in enemy's turn");
			string battleText = enemyUnits[currentEnemyIndex].unitName + " attacks " + unitToAttackHUD.unit.unitName + "!";
			battleHUD.SetDialogueText(battleText);
			unitToAttackHUD.PlayDamageAnim();

			// Wait after setting the attacking text before actually doing damage
			yield return new WaitForSeconds(1f);

			// Apply damage to the HUD's unit variable
			bool isDead = unitToAttackHUD.unit.TakeDamage(enemyUnits[currentEnemyIndex].damage);

			battleText = unitToAttackHUD.unit.unitName + " takes " + enemyUnits[currentEnemyIndex].damage + " damage!";
			battleHUD.SetDialogueText(battleText);
			// Update the HUD health of the unit
			unitToAttackHUD.UpdateHealth();
			
			if (isDead)
			{
				print("Killed the party member");
				// If the unit is killed, wait a very small amount before
				// showing that the enemy has died
				yield return new WaitForSeconds(1f);
				PartyMemberEliminated(unitToAttackHUD);
			}
			else
			{
				// Otherwise if not killed, wait a little longer before
				// moving to the next turn
				yield return new WaitForSeconds(1f);
				print("Should go to next enemy turn");
				NextEnemyTurn();
			}
		}
	}

	// Process the elimination of an enemy by a player
	private IEnumerator EnemyEliminated(UnitInformationHUD unitToAttackHUD)
	{
		battleHUD.SetDialogueText(unitToAttackHUD.unit.unitName + " has been defeated!");
		// If it has, remove it from it's respective unit list
		enemyUnits.Remove(unitToAttackHUD.unit);
		// Also remove it from the battle hud
		StartCoroutine(battleHUD.RemoveEnemyFromHUD(unitToAttackHUD));

		// Wait a little after removing the enemy before moving on with
		// the battle
		yield return new WaitForSeconds(1f);
		print("Enemy removed from screen");
		// If there is no more left of that unit type, one side has won, end battle
		if (enemyUnits.Count <= 0)
		{
			print("Battle won");
			state = BattleState.WON;
			StartCoroutine(EndBattle());
		}
		else
		{
			print("Move to next party member");
			NextPartyMemberTurn();
		}
	}

	public void NextPartyMemberTurn()
	{
		// The current player's turn has ended
		currentPlayerIndex++;
		print("Current party member index: " + currentPlayerIndex);
		if (currentPlayerIndex <= playerUnits.Count - 1)
		{
			print("Next player's action");
			PlayerTurn(currentPlayerIndex);
		}
		else
		{
			print("Should move to enemy turn");
			state = BattleState.ENEMYTURN;
			battleHUD.TurnOffUnitTurn();
			EnemyTurn(0);
		}
	}

	private void PartyMemberEliminated(UnitInformationHUD unitToAttackHUD)
	{
		// If it has, remove it from it's respective unit list
		playerUnits.Remove(unitToAttackHUD.unit);
		// Also remove it from the battle hud
		battleHUD.RemovePlayerFromHUD(unitToAttackHUD);
		// If there is no more left of that unit type, one side has won, end battle
		if (playerUnits.Count <= 0)
		{
			state = BattleState.LOST;
			StartCoroutine(EndBattle());
		}
		else
		{
			NextEnemyTurn();
		}
	}

	private void NextEnemyTurn()
	{
		print("Next Enemy Turn");
		currentEnemyIndex++;
		if (currentEnemyIndex <= enemyUnits.Count - 1)
		{
			print("Next enemy take a turn");
			EnemyTurn(currentEnemyIndex);
		}
		else
		{
			print("Return to player turn");
			state = BattleState.PLAYERTURN;
			battleHUD.TurnOffUnitTurn();
			PlayerTurn();
		}
	}

	public bool AttemptToRun()
	{
		float runSuccessChance = CalculateRunChance();

		float randomChance = Random.value;
		print("Random chance: " + randomChance);
		print("Run Success chance: " + runSuccessChance);

		if (randomChance > runSuccessChance)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	private float CalculateRunChance()
	{
		int playerLevelTotal = CalculateTotalLevels(playerUnits);
		int enemyLevelTotal = CalculateTotalLevels(enemyUnits);
		print("Player level total: " + playerLevelTotal);
		print("Enemy level total: " + enemyLevelTotal);

		int levelDifference = enemyLevelTotal - playerLevelTotal;

		float runChance = Sigmoid(levelDifference);
		

		return runChance;
	}

	private int CalculateTotalLevels(List<Unit> units)
	{
		int levelTotal = 0;
		foreach (Unit unit in units)
		{
			levelTotal += unit.level;
		}
		return levelTotal;
	}

	// Calculate run chance on a curve
	private float Sigmoid(int x)
	{
		float k = 0.15f; // Curve steepness
		float runChance = 1f / (1f + Mathf.Exp(-k * x));
		return runChance;
	}

	// Set the selected item as well as allow the user to select a party member
	public void UnitItemSelection(ItemInformationHUD itemHUD)
	{
		selectedItem = itemHUD;
		battleHUD.SelectPartyMemberToUseItemOn(itemHUD);
	}

	public IEnumerator UseItem(UnitInformationHUD unitToUseItemOn)
	{
		battleHUD.TogglePartyMemberButtons(false);
		battleHUD.SwapToDialogueMenu();
		battleHUD.SetDialogueText("Used " + selectedItem.item.itemName + " on " + unitToUseItemOn.unit.unitName);
		GameManager.Instance.PlayerManager.inventory.UseSelectedItem(selectedItem.item, unitToUseItemOn.unit);
		unitToUseItemOn.UpdateHealth();

		print("Used the item");
		battleHUD.RemoveItemFromHUD(selectedItem);
		print("Should move on");

		yield return new WaitForSeconds(1f);
		selectedItem = null;

		NextPartyMemberTurn();
	}
}
