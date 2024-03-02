using UnityEngine;
using UnityEngine.SceneManagement;

public class StartBattle : MonoBehaviour
{
	[SerializeField] private PlayerParty party;
    void Start()
    {
		DontDestroyOnLoad(this.gameObject);
		SceneManager.sceneLoaded += OnSceneLoaded;
		this.gameObject.SetActive(false);
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name == "Title")
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
			Destroy(this.gameObject);
		}
		else
		{
			this.gameObject.SetActive(scene.name == "Battle");
			foreach(var unit in party.playerUnits)
			{
				unit.gameObject.SetActive(scene.name == "Battle");
			}
		}
	}
}
