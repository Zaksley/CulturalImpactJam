using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTheCamera : MonoBehaviour
{
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, mainCamera.transform.rotation, 1);
    }

    private void Update()
    {
        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, mainCamera.transform.rotation, 1);
    }
}
