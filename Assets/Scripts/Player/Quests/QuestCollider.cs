using UnityEngine;

public class QuestCollider : MonoBehaviour
{
    [SerializeField] private QuestController _questController; 
    
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Tool":

                if (_questController.FollowingQuest == QuestController.QuestFollowing.FetchTool)
                {
                    _questController.ToolRecovered();
                    other.gameObject.SetActive(false);
                }
                break; 
            
            case "FeatherLady":

                if (_questController.FollowingQuest == QuestController.QuestFollowing.FeathersCollectLady)
                {
                    _questController.AddFeatherLady();
                    other.gameObject.SetActive(false);
                }
                break; 
            
            case "Mouse":

                if (_questController.FollowingQuest == QuestController.QuestFollowing.MouseCollect)
                {
                    _questController.MouseCollected();
                    other.gameObject.SetActive(false);
                }
                break; 
        }
    }
}