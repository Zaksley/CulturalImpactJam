using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHandleScarecrow : MonoBehaviour
{
    [SerializeField] private QuestController _questController;
    [SerializeField] private ScarecrowManager _scarecrowManager; 
    
    [SerializeField] private GameObject SwapFace;
    [SerializeField] private GameObject SwapHand;
    [SerializeField] private GameObject SwapHead;

    [Header("Scarecrow Parts")]
    public List<GameObject> FacesParts = new List<GameObject>(); 
    public List<GameObject> HandsParts = new List<GameObject>(); 
    public List<GameObject> HeadsParts = new List<GameObject>(); 
    
    private int _actualFace = 0;
    private int _actualHand = 0; 
    private int _actualHead = 0;
    
    public bool CanInteractWithScarecrow { get; private set; }
    public bool IsInteractingWithScarecrow { get; private set; }
    [SerializeField] private GameObject InteractCanva;
    
    private void Awake()
    {
        StateButtons(false);
    }

    private void Update()
    {

        if (CanInteractWithScarecrow)
        {
            InteractCanva.SetActive(true);
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                IsInteractingWithScarecrow = true; 
            }
        }
        else
        {
            InteractCanva.SetActive(false);
        }
        
        if (IsInteractingWithScarecrow)
        {
            StateButtons(true);
            
            if (Input.GetKeyDown(KeyCode.I))
            {
                ScrollFace();
            }
            
            if (Input.GetKeyDown(KeyCode.O))
            {
                ScrollHead();
            }
            
            if (Input.GetKeyDown(KeyCode.P))
            {
                ScrollHand();
            }
        }
        else
        {
            StateButtons(false);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Scarecrow"))
        {
            CanInteractWithScarecrow = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Scarecrow"))
        {
            CanInteractWithScarecrow = false;
            IsInteractingWithScarecrow = false; 
        }
    }

    private void StateButtons(bool state)
    {
        SwapFace.SetActive(state); 
        SwapHand.SetActive(state); 
        SwapHead.SetActive(state); 
    }

    public void UpdateButtons()
    {
        if (_questController.numberFaces > 0)
        {
            SwapFace.SetActive(true); 
        }

        if (_questController.numberHands > 0)
        {
            SwapHand.SetActive(true); 
        }

        if (_questController.numberHeads > 0)
        {
            SwapHead.SetActive(true); 
        }
    }

    public void ScrollFace()
    {
        if (_questController.numberFaces == 0)
            return;

        _actualFace = (_actualFace + 1) % 3;
        
        while (!_questController.HasFaces[_actualFace])
        {
            _actualFace = (_actualFace + 1) % 3;
        }
        
        HideParts(FacesParts);
        FacesParts[_actualFace].SetActive(true);
        _scarecrowManager.HasFace = true;

        if (_scarecrowManager.CompletedQuest())
        {
            _questController.ScaredcrowDone(); 
        }
    }

    public void ScrollHand()
    {
        if (_questController.numberHands == 0)
            return;

        _actualHand = (_actualHand + 1) % 3;
        
        while (!_questController.HasHands[_actualHand])
        {
            _actualHand = (_actualHand + 1) % 3;
        }
        
        HideParts(HandsParts);
        HandsParts[_actualHand].SetActive(true);
        _scarecrowManager.HasHand = true; 
        
        if (_scarecrowManager.CompletedQuest())
        {
            _questController.ScaredcrowDone(); 
        }
    }

    public void ScrollHead()
    {
        if (_questController.numberHeads == 0)
            return;
        
        _actualHead = (_actualHead + 1) % 3;
        
        while (!_questController.HasHeads[_actualHead])
        {
            _actualHead = (_actualHead + 1) % 3;
        }
        
        HideParts(HeadsParts);
        HeadsParts[_actualHead].SetActive(true);
        _scarecrowManager.HasHead = true;
        
        if (_scarecrowManager.CompletedQuest())
        {
            _questController.ScaredcrowDone(); 
        }
    }

    private void HideParts(List<GameObject> parts)
    {
        foreach (var part in parts)
        {
            part.SetActive(false);
        }
    }
}
