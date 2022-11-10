using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayerAndTurnOnCamera : MonoBehaviour
{
    public GameObject Ccamera;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Ccamera.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Ccamera.SetActive(false);
        }
    }
}
