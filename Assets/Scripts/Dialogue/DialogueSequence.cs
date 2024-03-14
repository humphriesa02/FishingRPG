using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogueSequence", menuName = "Dialogue/DialogueSequence")]
public class DialogueSequence : ScriptableObject
{
	public string speakerName;
	public Sprite speakerPortrait;
	[TextArea(3, 10)]
	public string dialogueText;
	public DialogueSequence[] followUpSequences;
}
