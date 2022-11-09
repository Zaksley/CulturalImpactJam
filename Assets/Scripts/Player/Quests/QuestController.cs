using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    [SerializeField] private GeneralController _generalController; 
    
    // If we are following this quest
    [SerializeField] private int _neededFeathersLady = 3; 
    private int _feathersCounter = 0;

    private Dictionary<QuestFollowing, bool> _dictionaryQuests = new Dictionary<QuestFollowing, bool>();
    public Dictionary<QuestFollowing, bool> DictionaryQuests => _dictionaryQuests;

    // If we collected what we needed to collect
    public bool IsMouseCollected { get; private set; }
    public bool IsFeathersCollected { get; private set; }
    public bool IsScarecrowDone { get; private set; }
    public bool IsTheToolRecovered { get; private set; }
    
    public enum QuestFollowing
    {
        None, 
        MouseCollect, 
        FetchTool,
        FeathersCollectLady,
        ScareCrow,
    }

    private QuestFollowing _followingQuest = QuestFollowing.None;
    public QuestFollowing FollowingQuest => _followingQuest;

    private void Start()
    {
        _dictionaryQuests[QuestFollowing.None] = false; 
        _dictionaryQuests[QuestFollowing.MouseCollect] = false; 
        _dictionaryQuests[QuestFollowing.FetchTool] = false; 
        _dictionaryQuests[QuestFollowing.FeathersCollectLady] = false; 
        _dictionaryQuests[QuestFollowing.ScareCrow] = false; 
    }

    private void Update()
    {
    }

    public void ToolRecovered()
    {
        IsTheToolRecovered = true;
    }

    public void AddFeatherLady()
    {
        _feathersCounter++;

        if (_feathersCounter >= _neededFeathersLady)
        {
            FeathersCollected();
        }
    }

    private void FeathersCollected()
    {
        IsFeathersCollected = true;
    }
    
    public void ScaredcrowDone()
    {
        IsScarecrowDone = true;
    }
    
    public void MouseCollected()
    {
        IsMouseCollected = true;
    }

    public void AttributeQuest(QuestFollowing questGiven)
    {
        _followingQuest = questGiven; 
        Debug.Log("QUEST ACCEPTED : " + questGiven);
    }

    public void CompleteQuest(QuestFollowing questDone)
    {
        _generalController.GainFeather();
        _dictionaryQuests[questDone] = true; 
        UpdateFinishedQuest();
        
        Debug.Log("QUEST DONE : " + questDone);
    }

    private void UpdateFinishedQuest()
    {
        _followingQuest = QuestFollowing.None; 
    }
}
