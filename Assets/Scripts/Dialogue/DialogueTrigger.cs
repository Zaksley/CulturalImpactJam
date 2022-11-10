using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
	public Dialogue dialogue;
	public bool IsDialogueAboutScarecrow = false;
	public bool IsDialogueAboutLoveBirds = false; 

	public void TriggerDialogue( )
	{
		FindObjectOfType<DialogueManager>().StartDialogue(dialogue, IsDialogueAboutScarecrow, IsDialogueAboutLoveBirds);
	}
}