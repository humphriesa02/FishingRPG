using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// Different states of the battle
public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

/**
 * Handles logic for a turn based battle
 */
public class BattleSystem : MonoBehaviour
{
	// State of the battle
    public BattleState state;

	// List of 
	private List<Unit> playerUnits;
	private List<Unit> enemyUnits;

	private BattleHUD battleHUD;

	void Start()
    {
		// Start in the 'start' state
        state = BattleState.START;
		battleHUD = GameObject.FindGameObjectWithTag("BattleHUD").GetComponent<BattleHUD>();
        StartCoroutine(SetupBattle());
    }

	IEnumerator SetupBattle()
    {
		// Collect the list of player units
		playerUnits = new List<Unit>();
		GameObject[] playerUnitGameObjects = GameObject.FindGameObjectsWithTag("PlayerUnit");

		foreach (GameObject obj in playerUnitGameObjects)
		{
			Unit currentUnit = obj.GetComponent<Unit>();
			playerUnits.Add(currentUnit);
		}

		// Collect the list of enemy units
		enemyUnits = new List<Unit>();
		GameObject[] enemyUnitGameObjects = GameObject.FindGameObjectsWithTag("EnemyUnit");

		foreach (GameObject obj in enemyUnitGameObjects)
		{
			Unit currentUnit = obj.GetComponent<Unit>();
			enemyUnits.Add(currentUnit);
		}

		// Set up the UI
		battleHUD.SetupBattleHUD(enemyUnits, playerUnits, this);

		yield return new WaitForSeconds(2f);

		state = BattleState.PLAYERTURN;
		PlayerTurn();
	}

	void PlayerTurn()
	{
		battleHUD.SwapToActionMenu();
	}

	public IEnumerator AttackUnit(Unit attackingUnit, Unit unitToAttack)
	{
		// Damage the enemy
		print("ATTACKING");
		unitToAttack.TakeDamage(attackingUnit.damage);
		yield return new WaitForSeconds(2f);

		// Check if the enemy is dead
		// Change state based on what happens
	}
}
