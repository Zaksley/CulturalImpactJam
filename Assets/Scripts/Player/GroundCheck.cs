using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool isOnGround;

    public LayerMask JumpableFloors; 

    private void Start()
    {
        isOnGround = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(IsInLayerMask(other.gameObject, JumpableFloors))
        {
            isOnGround = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (IsInLayerMask(other.gameObject, JumpableFloors))
        {
            isOnGround = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsInLayerMask(other.gameObject, JumpableFloors))
        {
            isOnGround = false;
        }
    }

    public bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return ((layerMask.value & (1 << obj.layer)) > 0);
    }
}
