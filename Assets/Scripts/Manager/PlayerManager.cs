using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	// Party reference
	public PlayerParty playerParty;
	public Inventory inventory;

	// Player stuff
	public GameObject playerPrefab;
	public GameObject player;
	public string playerName = "Alex";

	public Vector2 playerStartingPosition; // Actual position to spawn the player
	public Vector2 playerStartingDirection; // Direction to spawn the player

	private void Awake()
	{
		SetPlayerStartingPosition(ChooseRandomPlayerStart());
		playerParty = GetComponentInChildren<PlayerParty>();
	}

	private void Start()
	{
		if (player == null)
		{
			SpawnPlayer();
		}
		PlayerPrefs.SetString("PlayerName", playerName);
	}

	// Creates an instance of the movable player character
	public void SpawnPlayer()
	{
		if (player == null && playerStartingPosition != null)
		{
			player = Instantiate(playerPrefab, playerStartingPosition, Quaternion.identity);
		}
	}

	// Destroys the instance of the movable player character if it exists
	public void RemovePlayer()
	{
		if (player != null)
		{
			Destroy(player);
			player = null;
		}
	}

	// Set the local starting vector based on a supplied Transform
	public void SetPlayerStartingPosition(Transform newStartingTransform = null, Vector2 spawnDirection = default(Vector2), bool movePlayerBack = false)
	{
		if (newStartingTransform != null)
		{
			if (movePlayerBack) playerStartingPosition = CopyTransformPosition(newStartingTransform) - spawnDirection;
			else playerStartingPosition = CopyTransformPosition(newStartingTransform);	
		}
		playerStartingDirection = spawnDirection;
	}

	// Chooses a transform by randomly selecting an object tagged with "PlayerStart"
	// Add player start objects in a scene to spawn a player there
	private Transform ChooseRandomPlayerStart()
	{
		GameObject[] playerStarts = GameObject.FindGameObjectsWithTag("PlayerStart");

		if (playerStarts.Length > 0)
		{
			int randomIndex = Random.Range(0, playerStarts.Length - 1);
			return playerStarts[randomIndex].transform;
		}

		return null;
	}

	// Deep copy the position element of a transform and return it
	private Vector2 CopyTransformPosition(Transform transformToCopy)
	{
		Vector2 copiedTransformPosition = transformToCopy.position;
		return new Vector2(copiedTransformPosition.x, copiedTransformPosition.y);
	}
}
