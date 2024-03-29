using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;
	public PlayerManager PlayerManager;
	public CameraManager CameraManager;
	public DialogueManager DialogueManager;
	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(this.gameObject);
			return;
		}
		Instance = this;
		DontDestroyOnLoad(Instance.gameObject);

		// Get Components from Children
		PlayerManager = GetComponentInChildren<PlayerManager>();
		CameraManager = GetComponentInChildren<CameraManager>();
		DialogueManager = GetComponentInChildren<DialogueManager>();

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	private void Start()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name != "Battle")
		{
			PlayerManager.SpawnPlayer();
		}
	}
}
