using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldItem : MonoBehaviour
{
    public DialogueSequence[] dialogueSequence;
	public Item item;

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			GameManager.Instance.DialogueManager.StartDialogue(dialogueSequence);
			GameManager.Instance.PlayerManager.inventory.AddItem(item);
			Destroy(this.gameObject);
		}
	}
}
