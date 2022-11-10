using UnityEngine;

public class QuestCollider : MonoBehaviour
{
    [SerializeField] private QuestController _questController;
    [SerializeField] private PlayerHandleScarecrow _handleScarecrow; 
    
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
            
            case "Hands":
                if (_questController.FollowingQuest == QuestController.QuestFollowing.ScareCrow)
                {
                    int value = other.GetComponent<ScarecrowItem>().ReturnPosition();
                    _questController.HasHands[value-1] = true; 
                    _questController.numberHands++; 
                    other.gameObject.SetActive(false);
                    
                    _handleScarecrow.UpdateButtons();
                }
                break; 
            
            case "Faces":
                if (_questController.FollowingQuest == QuestController.QuestFollowing.ScareCrow)
                {
                    int value = other.GetComponent<ScarecrowItem>().ReturnPosition();
                    _questController.HasFaces[value-1] = true;
                    _questController.numberFaces++; 
                    other.gameObject.SetActive(false);
                    
                    _handleScarecrow.UpdateButtons();
                }
                break; 
            
            case "Heads":
                if (_questController.FollowingQuest == QuestController.QuestFollowing.ScareCrow)
                {
                    int value = other.GetComponent<ScarecrowItem>().ReturnPosition();
                    _questController.HasHeads[value-1] = true; 
                    _questController.numberHeads++; 
                    other.gameObject.SetActive(false);
                    
                    _handleScarecrow.UpdateButtons();
                }
                break; 
        }
    }
}