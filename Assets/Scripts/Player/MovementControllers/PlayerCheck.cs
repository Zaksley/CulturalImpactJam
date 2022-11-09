using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheck : MonoBehaviour
{
    private bool _playerIsIn;
    [SerializeField] private GameObject NPCInteractCanva;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            _playerIsIn = true;
            other.GetComponent<DialogueInitiator>().SetIfPlayerIsCloserToEnemy(true);
            DoWhenPlayerGoesInside();
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
            other.GetComponent<DialogueInitiator>().SetIfPlayerIsCloserToEnemy(false);
            DoWhenPlayerGoesOutside();
        }
    }

    public void DoWhenPlayerGoesInside()
    {
        NPCInteractCanva.SetActive(true);
    }

    public void DoWhenPlayerGoesOutside()
    {
        NPCInteractCanva.SetActive(false);
    }
}
