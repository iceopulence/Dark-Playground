using UnityEngine;

public class GameInitializer : MonoBehaviour
{

    [SerializeField] Quest simpleQuest;
    private void Start()
    {
        QuestManager questManager = FindObjectOfType<QuestManager>();

        
        if(simpleQuest != null)
        {
            // Add the quest to the quest manager
            questManager.AddQuest(simpleQuest);
            print("ran");
        }
    }
}
