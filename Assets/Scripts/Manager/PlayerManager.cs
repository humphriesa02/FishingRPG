using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
	// Party reference
	public GameObject playerParty;

	public GameObject playerPrefab;
	public GameObject player;

	public Vector2 playerStartingPosition; // Actual position to spawn the player
	public Vector2 playerStartingDirection; // Direction to spawn the player

	private void Awake()
	{
		SetPlayerStartingPosition(ChooseRandomPlayerStart());
	}

	private void Start()
	{
		if (player == null)
		{
			SpawnPlayer();
		}
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
	public void SetPlayerStartingPosition(Transform newStartingTransform = null)
	{
		if (newStartingTransform != null)
		{
			playerStartingPosition = CopyTransformPosition(newStartingTransform) - playerStartingDirection;
		}
	}

	// Set the local starting direction for the player based on a supplied Vector direction
	public void SetPlayerStartingDirection(Vector2 newStartingDirection)
	{
		playerStartingDirection = newStartingDirection;
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
