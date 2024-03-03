using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

// Different states of the battle
public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

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

		// Set up the UI
		battleHUD.SetupBattleHUD(enemyUnits, playerUnits, this);

		// Wait
		yield return new WaitForSeconds(2f);

		// Start the player's turn
		state = BattleState.PLAYERTURN;
		PlayerTurn(0);
	}

	IEnumerator EndBattle()
	{
		if (state == BattleState.WON)
		{
			battleHUD.SetDialogueText("You won the battle! Experience gained!");
		}
		else if (state == BattleState.LOST)
		{
			battleHUD.SetDialogueText("Your party has all been knocked out!");
		}
		yield return new WaitForSeconds(2f);

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
		battleHUD.SwapToActionMenu();

		/// Player attacking functionality happens in HUD,
		/// refer to BattleHUD.cs as well as UnitInformationHUD.cs
	}

	void EnemyTurn(int enemyIndex = 0)
	{
		print("Enemy turn!");
		currentEnemyIndex = enemyIndex;
		battleHUD.SwapToDialogueMenu();

		// Find a random player to attack
		UnitInformationHUD randomPartyUnit = battleHUD.SelectRandomPartyMember();
		StartCoroutine(AttackUnit(randomPartyUnit));
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

			yield return new WaitForSeconds(1f);

			// Apply damage to the HUD's unit variable
			bool isDead = unitToAttackHUD.unit.TakeDamage(playerUnits[currentPlayerIndex].damage);

			battleText = unitToAttackHUD.unit.unitName + " takes " + playerUnits[currentPlayerIndex].damage + " damage!";
			battleHUD.SetDialogueText(battleText);
			// Update the HUD health of the unit
			unitToAttackHUD.UpdateHealth();

			// Wait
			yield return new WaitForSeconds(2f);
			print("We are in player state");
			if (isDead)
			{
				print("Killed the enemy");
				EnemyEliminated(unitToAttackHUD);
			}
			else
			{
				print("Did not kill the enemy");
				NextPartyMemberTurn();
			}
			
		}
		else if (state == BattleState.ENEMYTURN) // Enemies are attacking
		{
			print("We are in enemy's turn");
			string battleText = enemyUnits[currentEnemyIndex].unitName + " attacks " + unitToAttackHUD.unit.unitName + "!";
			battleHUD.SetDialogueText(battleText);

			yield return new WaitForSeconds(2f);

			// Apply damage to the HUD's unit variable
			bool isDead = unitToAttackHUD.unit.TakeDamage(enemyUnits[currentEnemyIndex].damage);

			battleText = unitToAttackHUD.unit.unitName + " takes " + enemyUnits[currentEnemyIndex].damage + " damage!";
			battleHUD.SetDialogueText(battleText);
			// Update the HUD health of the unit
			unitToAttackHUD.UpdateHealth();

			// Wait
			yield return new WaitForSeconds(2f);
			if (isDead)
			{
				print("Killed the party member");
				PartyMemberEliminated(unitToAttackHUD);
			}
			else
			{
				print("Should go to next enemy turn");
				NextEnemyTurn();
			}
		}
	}

	// Process the elimination of an enemy by a player
	private void EnemyEliminated(UnitInformationHUD unitToAttackHUD)
	{
		// If it has, remove it from it's respective unit list
		enemyUnits.Remove(unitToAttackHUD.unit);
		// Also remove it from the battle hud
		battleHUD.RemoveEnemyFromHUD(unitToAttackHUD);

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

	private void NextPartyMemberTurn()
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
			PlayerTurn();
		}
	}
}
