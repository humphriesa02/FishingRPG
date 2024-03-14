using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private Canvas dialogueCanvas;
	[SerializeField] private TextMeshProUGUI dialogueText;
	[SerializeField] private Image speakerImage;
	[SerializeField] private TextMeshProUGUI speakerText;

    Queue<DialogueSequence> dialogueQueue;

    public void StartDialogue(DialogueSequence[] sequences)
    {
		// Disable player
		GameManager.Instance.PlayerManager.player.GetComponent<PlayerMovement>().ChangePlayerState(PlayerState.Menu);

        dialogueQueue = new Queue<DialogueSequence>(sequences);

		dialogueCanvas.gameObject.SetActive(true);
        StartCoroutine(DisplayNextDialogue());
	}

    IEnumerator DisplayNextDialogue()
    {
		foreach (DialogueSequence sequence in dialogueQueue)
		{
			print(sequence.dialogueText);
			if (sequence.speakerPortrait != null) speakerImage.sprite = sequence.speakerPortrait;
			else speakerImage.enabled = false;

			dialogueText.text = sequence.dialogueText;
			speakerText.text = sequence.speakerName;

			yield return new WaitForSeconds(0.5f);
			yield return new WaitUntil(() => Input.GetButtonDown("Submit"));
		}
		print("Ending dialogue");
		EndDialogue();
	}

	void EndDialogue()
	{
		// Enable player
		GameManager.Instance.PlayerManager.player.GetComponent<PlayerMovement>().ChangePlayerState(PlayerState.Default);

		// Clean up dialogue UI, reset flags, etc.
		dialogueCanvas.gameObject.SetActive(false);
	}
}
