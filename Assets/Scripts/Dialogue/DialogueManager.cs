using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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
	private bool _isThisDialogueAboutLoveBirds = false; 
	public GameObject PortraitCharacter;
	public GameObject PortraitScarecrow;

	private int countIndexElement = -1; 
	public List<int> scareCrowPortraitDialogueLine;
	public List<int> LoveBirdOneDialogueLine;
	public List<int> LoveBirdTwoDialogueLine;
	public List<int> SecondLoveBirdOneDialogueLine;
	public List<int> SecondLoveBirdTwoDialogueLine;

	private Color _colorLoveBirdOne;
	private Color _colorLoveBirdTwo; 
	
	// Use this for initialization
	void Awake()
	{
		sentences = new Queue<string>();
		animator = this.GetComponent<Animator>();
		
		if (PortraitScarecrow != null)
			PortraitScarecrow.SetActive(false);
		
		_colorLoveBirdOne = new Color( 89.0f / 255.0f,121.0f / 255.0f,186.0f / 255.0f, 255);
		_colorLoveBirdTwo = new Color(71.0f / 255.0f,149.0f / 255.0f,113.0f / 255.0f, 255); 
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

	public void StartDialogue(Dialogue dialogue, bool IsDialogueAboutScarecrow, bool IsDialogueAboutLoveBirds)
	{
		_isThisDialogueAboutScarecrow = IsDialogueAboutScarecrow;
		_isThisDialogueAboutLoveBirds = IsDialogueAboutLoveBirds; 
		countIndexElement = -1; 
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

		if (_isThisDialogueAboutScarecrow || _isThisDialogueAboutLoveBirds)
		{
			countIndexElement++; 
			PortraitCharacter.SetActive(true);
			PortraitScarecrow.SetActive(false);
			dialogueText.color = Color.white; 
			
			var lovebirdOneDialogueLine = _isThisDialogueAboutScarecrow ? LoveBirdOneDialogueLine : SecondLoveBirdOneDialogueLine;
			var lovebirdTwoDialogueLine =
				_isThisDialogueAboutScarecrow ? LoveBirdTwoDialogueLine : SecondLoveBirdTwoDialogueLine; 
				
			// First loving bird
			foreach (var dialogueIndex in lovebirdOneDialogueLine)
			{
				if (dialogueIndex == countIndexElement)
				{
					Debug.Log("in?");
					dialogueText.color = _colorLoveBirdOne; 
				}
			}
			
			// Second loving bird
			foreach (var dialogueIndex in lovebirdTwoDialogueLine)
			{
			
				if (dialogueIndex == countIndexElement)
				{
					Debug.Log("in? two");
					dialogueText.color = _colorLoveBirdTwo; 
				}
			}
			
			if (_isThisDialogueAboutScarecrow)
			{
				foreach (var dialogueIndex in scareCrowPortraitDialogueLine)
				{
					if (dialogueIndex == countIndexElement)
					{
						PortraitCharacter.SetActive(false);
						PortraitScarecrow.SetActive(true);
					}
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
		
		if (_isThisDialogueAboutLoveBirds)
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
	}

}