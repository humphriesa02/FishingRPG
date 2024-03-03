using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;
	// Starting player party prefab
	public GameObject playerPartyPrefab;
	public GameObject playerParty;

	public GameObject playerPrefab;
	public GameObject player;

	public Transform playerStartingTransform;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this);
			return;
		}
		Instance = this;
		DontDestroyOnLoad(Instance);

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	private void Start()
	{
		if (playerParty == null)
		{
			playerParty = Instantiate(playerPartyPrefab);
		}
		if (player == null)
		{
			player = Instantiate(playerPrefab, playerStartingTransform);
		}
		SceneManager.sceneLoaded += OnSceneLoaded;
	}
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name != "Battle")
		{
			if (player == null)
			{
				player = Instantiate(playerPrefab, playerStartingTransform);
			}
		}
	}

	public void RemovePlayer()
	{
		Destroy(player);
		player = null;
	}
}
