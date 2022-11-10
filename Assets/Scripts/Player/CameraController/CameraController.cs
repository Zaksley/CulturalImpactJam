using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GeneralController _generalController;
    [SerializeField] private GlideController _glideController;
    [SerializeField] private TopDownCharacterMover _topDownCharacterMover; 

    public GameObject MainCamera;
    public GameObject FlightCamera;
    public GameObject PlungeCamera; 
    
    private void Update()
    {
        // if (_glideController.IsNormalFlying)
        // {
        //     MainCamera.SetActive(false);
        //     FlightCamera.SetActive(true);
        //     PlungeCamera.SetActive(false);
        // }
        // else if (_glideController.IsPlunging)
        // {
        //     MainCamera.SetActive(false);
        //     FlightCamera.SetActive(false);
        //     PlungeCamera.SetActive(true);
        // }
        // else
        // {
        //     MainCamera.SetActive(true);
        //     FlightCamera.SetActive(false);
        //     PlungeCamera.SetActive(false);
        // }
    }
}
