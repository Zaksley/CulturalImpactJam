using System;
using UnityEngine;

public class ScarecrowManager : MonoBehaviour
{
    public bool HasFace;
    public bool HasHand;
    public bool HasHead;
    
    public bool CompletedQuest()
    {
        if (HasFace && HasHand && HasHead)
            return true;

        return false; 
    }
}
