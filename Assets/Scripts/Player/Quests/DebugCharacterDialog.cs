using UnityEngine;

public class DebugCharacterDialog : MonoBehaviour
{
    public QuestController.QuestFollowing GiveRetrieveQuest;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || GiveRetrieveQuest == QuestController.QuestFollowing.None)
            return; 
            
        var questController =  other.GetComponentInChildren<QuestController>();

        if (questController != null)
        {
            // Wants to give a quest
            if (questController.FollowingQuest == QuestController.QuestFollowing.None)
            {
                //Give quest
                if (!questController.DictionaryQuests[GiveRetrieveQuest])
                {
                    questController.AttributeQuest(GiveRetrieveQuest); 
                }
                // Quest already done
                else
                {
                    // Do nothing or pop-up? 
                }
            }
            // Wants to retrieve a quest 
            else
            {
                if (questController.FollowingQuest == GiveRetrieveQuest &&
                    !questController.DictionaryQuests[GiveRetrieveQuest])
                {
                    if (CheckRequierements(GiveRetrieveQuest, questController))
                    {
                        questController.CompleteQuest(GiveRetrieveQuest);
                    }
                }
            }
        }
        else
        {
            Debug.Log("Missing quest controller");
        }
    }

    private bool CheckRequierements(QuestController.QuestFollowing followingQuest, QuestController questController)
    {
        switch (followingQuest)
        {
            case QuestController.QuestFollowing.MouseCollect:
                return questController.IsMouseCollected; 
            
            case QuestController.QuestFollowing.FetchTool:
                return questController.IsTheToolRecovered;

            case QuestController.QuestFollowing.FeathersCollectLady:
                return questController.IsFeathersCollected; 
            
            case QuestController.QuestFollowing.ScareCrow:
                return questController.IsScarecrowDone;
        }

        Debug.Log("Requierements not done for quest : " + followingQuest);
        return false;
    }
}
