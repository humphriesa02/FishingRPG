using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
	private List<Unit> unitsStats;
	[SerializeField] private GameObject actionsMenu;
	void Start()
	{
		unitsStats = new List<Unit>();
		GameObject[] playerUnits = GameObject.FindGameObjectsWithTag("PlayerUnit");

		foreach (GameObject playerUnit in playerUnits)
		{
			Unit currentUnit = playerUnit.GetComponent<Unit>();
			currentUnit.calculateNextActTurn(0);
			unitsStats.Add(currentUnit);
		}
		GameObject[] enemyUnits = GameObject.FindGameObjectsWithTag("EnemyUnit");
		foreach (GameObject enemyUnit in enemyUnits)
		{
			Unit currentUnit = enemyUnit.GetComponent<Unit>();
			currentUnit.calculateNextActTurn(0);
			unitsStats.Add(currentUnit);
		}
		unitsStats.Sort();
		this.actionsMenu.SetActive(false);
		this.nextTurn();
	}
	public void nextTurn()
	{
		Unit currentUnit = unitsStats[0];
		unitsStats.Remove(currentUnit);
		if (!currentUnit.isDead())
		{
			GameObject currentUnitObj = currentUnit.gameObject;
			currentUnit.calculateNextActTurn(currentUnit.nextActTurn);
			unitsStats.Add(currentUnit);
			unitsStats.Sort();
			if (currentUnitObj.tag == "PlayerUnit")
			{
				Debug.Log("Player unit acting");
			}
			else
			{
				Debug.Log("Enemy unit acting");
			}
		}
		else
		{
			this.nextTurn();
		}
	}
}
