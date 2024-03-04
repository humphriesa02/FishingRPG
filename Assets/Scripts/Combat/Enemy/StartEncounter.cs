using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartEncounter : MonoBehaviour
{
	// The encounter we are battling
	[SerializeField] private GameObject enemyEncounterPrefab;
	private bool spawning = false;
	// Keep track of the previous scene to tell the GameManager
	private string previousScene;
	// Where the player collided with this object in order to return it to where it was
	private Transform playerCollisionTransform;
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
			if (system != null)
			{
				// Tell it where to return us to when we're done fighting
				system.returnSceneName = previousScene;
			}
			
			// Remove the moveable player character
			GameManager.Instance.PlayerManager.RemovePlayer();

			// Cleanup
			SceneManager.sceneLoaded -= OnSceneLoaded;
			Destroy(gameObject);
		}
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			spawning = true;

			// Handle setting where the player will return to
			playerCollisionTransform = other.transform;
			GameManager.Instance.PlayerManager.SetPlayerStartingDirection(other.GetComponent<PlayerMovement>().moveDirection);
			GameManager.Instance.PlayerManager.SetPlayerStartingPosition(playerCollisionTransform);

			// Name the previous scene to return to
			previousScene = SceneManager.GetActiveScene().name;

			// Move to battle scene
			SceneManager.LoadScene("Battle");
		}
	}
}
