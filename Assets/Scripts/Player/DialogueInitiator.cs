using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInitiator : MonoBehaviour
{
    private InputHandler _input;
    [SerializeField]
    private GameObject DialogueCanvas;
    public bool DialogueIsOn;
    private bool _playerIsCloserToEnemy;

    // Start is called before the first frame update
    void Awake()
    {
        _input = GetComponent<InputHandler>();
        DialogueIsOn = false;
        _playerIsCloserToEnemy = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(_input.DialogueButton && _playerIsCloserToEnemy)
        {
            if (!DialogueIsOn)
            {
                DialogueCanvas.GetComponent<Animator>().SetTrigger("NPC_Enter");
                DialogueIsOn = true;
            }
            else
            {
                DialogueCanvas.GetComponent<Animator>().SetTrigger("NPC_Exit");
                DialogueIsOn = false;
            }
        }
    }

    public void SetIfPlayerIsCloserToEnemy(bool value)
    {
        _playerIsCloserToEnemy = value;
    }
}
