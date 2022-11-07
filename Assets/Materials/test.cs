using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public Material mat;

    private void Start()
    {
        float a = mat.GetFloat("_WingsAppear");
        Debug.Log(a);
    }
}
