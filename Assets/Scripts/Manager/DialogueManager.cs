using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private Canvas dialogueCanvas;
	[SerializeField] private TextMeshProUGUI dialogueText;
	[SerializeField] private Image speakerImageBackground;
	[SerializeField] private Image speakerImage;
	[SerializeField] private Image advanceImage;
	[SerializeField] private TextMeshProUGUI speakerText;
	[SerializeField] private float typeWriterTextDelay = 0.2f;
	[SerializeField] private float noImageMargin = 15f; // Magic num, might check out responsive ui
	[SerializeField] private float withImageMargin = 235f; // Magic num, might check out responsive ui

	// Whether or not a dialogue is currently being written out
	private bool isTyping;
	private string dialogueString;


    Queue<DialogueSequence> dialogueQueue;

    public void StartDialogue(DialogueSequence[] sequences)
    {
		// Disable player
		GameManager.Instance.PlayerManager.player.GetComponent<PlayerMovement>().ChangePlayerState(PlayerState.Menu);

        dialogueQueue = new Queue<DialogueSequence>(sequences);

		dialogueCanvas.gameObject.SetActive(true);
		advanceImage.gameObject.SetActive(false);
        StartCoroutine(DisplayNextDialogue());
	}

	private void Update()
	{
		if (isTyping && Input.GetButtonDown("Submit"))
		{
			isTyping = false;
			dialogueText.text = dialogueString;
		}
	}
	IEnumerator DisplayNextDialogue()
    {
		
		foreach (DialogueSequence sequence in dialogueQueue)
		{
			dialogueText.text = "";
			advanceImage.gameObject.SetActive(false);
			Vector4 localMargin = dialogueText.margin;
			if (sequence.speakerPortrait != null)
			{
				localMargin.x = withImageMargin;
				speakerImageBackground.gameObject.SetActive(true);
				speakerImage.sprite = sequence.speakerPortrait;
			}
			else
			{
				localMargin.x = noImageMargin;
				speakerImageBackground.gameObject.SetActive(false);
				speakerImage.enabled = false;
			}
			dialogueText.margin = localMargin;
			speakerText.text = sequence.speakerName;
			yield return StartCoroutine(TypeWriterText(dialogueText, sequence.dialogueText, typeWriterTextDelay));
			isTyping = false;
			advanceImage.gameObject.SetActive(true);
			yield return new WaitUntil(() => Input.GetButtonDown("Submit"));
		}
		print("Ending dialogue");
		EndDialogue();
	}

	IEnumerator TypeWriterText(TextMeshProUGUI uiTextElement, string textToDisplay, float delayTime, AudioClip dialogueSound = null)
	{
		isTyping = true;
		dialogueString = textToDisplay;

		foreach (char c in textToDisplay)
		{
			if (isTyping)
			{
				uiTextElement.text += c;
				if (dialogueSound != null)
				{
					// TODO play sound when audio manager is created
				}
				yield return new WaitForSeconds(delayTime);
			}
			else break;
			
		}
	}

	void EndDialogue()
	{
		// Enable player
		GameManager.Instance.PlayerManager.player.GetComponent<PlayerMovement>().ChangePlayerState(PlayerState.Default);

		// Clean up dialogue UI, reset flags, etc.
		dialogueCanvas.gameObject.SetActive(false);
	}
}
