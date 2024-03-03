using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartEncounter : MonoBehaviour
{
	[SerializeField] private GameObject enemyEncounterPrefab;
	private bool spawning = false;
	// Keep track of the previous scene to tell the GameManager
	private string previousScene;
	// Where the player collided with this object in order to return it to where it was
	private Transform playerCollisionTranform;
	void Start()
	{
		DontDestroyOnLoad(this.gameObject);
		SceneManager.sceneLoaded += OnSceneLoaded;
	}
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name == "Battle")
		{
			if (spawning)
			{
				Instantiate(enemyEncounterPrefab);
			}

			// Find the battleSystem
			BattleSystem system = FindObjectOfType<BattleSystem>();
			// Tell it where to return us to when we're done fighting
			system.returnSceneName = previousScene;
			// Set the player starting transform for after the fight
			GameManager.Instance.playerStartingTransform = playerCollisionTranform;
			GameManager.Instance.RemovePlayer();
			SceneManager.sceneLoaded -= OnSceneLoaded;
			Destroy(gameObject);
		}
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			spawning = true;
			playerCollisionTranform = other.transform;
			previousScene = SceneManager.GetActiveScene().name;
			SceneManager.LoadScene("Battle");
		}
	}
}
