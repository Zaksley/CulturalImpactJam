using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
	public Image portrait;
	public TextMeshProUGUI nameText;
	public TextMeshProUGUI dialogueText;

	public TopDownCharacterMover characterMover;

	private Animator animator;

	private Queue<string> sentences;

	private bool isOnDialogue;
	private bool isTyping;

	private bool _isThisDialogueAboutScarecrow = false;
	public GameObject PortraitCharacter;
	public GameObject PortraitScarecrow;

	private int countIndexElement = -1; 
	public List<int> scareCrowPortraitDialogueLine; 
	
	// Use this for initialization
	void Awake()
	{
		sentences = new Queue<string>();
		animator = this.GetComponent<Animator>();
		
		if (PortraitScarecrow != null)
			PortraitScarecrow.SetActive(false);
	}

	private void Update()
	{
		if (isOnDialogue && !isTyping)
		{
			if (Input.GetKeyDown(KeyCode.E))
			{
				DisplayNextSentence();
			}
		}
	}

	public void StartDialogue(Dialogue dialogue, bool IsDialogueAboutScarecrow)
	{
		_isThisDialogueAboutScarecrow = IsDialogueAboutScarecrow; 
		characterMover.SetIsOnDialogue(true);
		isOnDialogue = true;

		animator.SetTrigger("NPC_Enter");

		portrait.sprite = dialogue.portrait;

		nameText.text = dialogue.name;

		sentences.Clear();

		foreach (string sentence in dialogue.sentences)
		{
			sentences.Enqueue(sentence);
		}

		DisplayNextSentence();
	}

	public void DisplayNextSentence()
	{
		if (sentences.Count == 0)
		{
			EndDialogue();
			return;
		}

		if (_isThisDialogueAboutScarecrow)
		{
			countIndexElement++; 
			PortraitCharacter.SetActive(true);
			PortraitScarecrow.SetActive(false);

			foreach (var dialogueIndex in scareCrowPortraitDialogueLine)
			{
				if (dialogueIndex == countIndexElement)
				{
					PortraitCharacter.SetActive(false);
					PortraitScarecrow.SetActive(true);
				}
			}
		}
		
		string sentence = sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
	}

	IEnumerator TypeSentence(string sentence)
	{
		dialogueText.text = "";
		isTyping = true;
		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return null;
		}
		isTyping = false;
	}

	void EndDialogue()
	{
		animator.SetTrigger("NPC_Exit");
		isOnDialogue = false;
		characterMover.SetIsOnDialogue(false);
	}

}