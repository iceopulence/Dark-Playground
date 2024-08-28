using UnityEngine;

public class GameInitializer : MonoBehaviour
{

    [SerializeField] Quest simpleQuest;
    private void Start()
    {   
        if(simpleQuest != null)
        {
            // Add the quest to the quest manager
            QuestManager.Instance.AddQuest(simpleQuest);
            print("ran");
        }
    }
}
