using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheck : MonoBehaviour
{
    public bool PlayerIsIn;
    [SerializeField] private GameObject NPCInteractCanva;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayerIsIn = true;
            DoWhenPlayerGoesInside();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerIsIn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerIsIn = false;
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
