using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInitiator : MonoBehaviour
{
    [SerializeField] private DebugCharacterDialog debugCharacterDialog;

    private bool _playerIsIn;
    [SerializeField] private GameObject NPCInteractCanva;

    private Collider player;
    
    void Update()
    {
        bool dialogueButton = Input.GetKeyDown(KeyCode.E);

        if (dialogueButton && _playerIsIn)
        {
            
            debugCharacterDialog.GiveQuest(player);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player = other;
            _playerIsIn = true;
            NPCInteractCanva.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            _playerIsIn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _playerIsIn = false;
            NPCInteractCanva.SetActive(false);
        }
    }
}
