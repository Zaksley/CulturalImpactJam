using UnityEngine;


public class ScarecrowItem : MonoBehaviour
{
    [SerializeField] private int numberItem = 0;

    public int ReturnPosition()
    {
        return numberItem; 
    }
}
