using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateEnemyMenu : MonoBehaviour
{
    [SerializeField] GameObject enemyMenuPrefab;

    [SerializeField] Sprite enemyMenuSprite;

    [SerializeField] Vector2 initialPosition, itemDimensions;

    [SerializeField] private KillEnemy killEnemyScript;

	private void Awake()
	{
		GameObject enemyUnitsMenu = GameObject.Find("EnemyUnitsMenu");
		GameObject[] existingItems = GameObject.FindGameObjectsWithTag("TargetEnemyUnit");
		Vector2 nextPosition = new Vector2(this.initialPosition.x + (existingItems.Length * this.itemDimensions.x), this.initialPosition.y);
		GameObject targetEnemyUnit = Instantiate(this.enemyMenuPrefab, enemyUnitsMenu.transform) as GameObject;
		targetEnemyUnit.name = "Target" + this.gameObject.name;
		targetEnemyUnit.transform.localPosition = nextPosition;
		targetEnemyUnit.transform.localScale = new Vector2(0.7f, 0.7f);
		targetEnemyUnit.GetComponent<Button>().onClick.AddListener(() => AttackEnemy());
		targetEnemyUnit.GetComponent<Image>().sprite = this.enemyMenuSprite;
		killEnemyScript.menuItem = targetEnemyUnit;
	}

	public void AttackEnemy()
	{

	}
}
