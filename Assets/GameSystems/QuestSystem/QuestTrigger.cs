using UnityEngine;
using UnityEngine.Events;

public abstract class QuestTrigger : MonoBehaviour
{
    public UnityEvent onCompletion;
    [SerializeField] public QuestInstance questInstance;
    public int objectiveIndex;
    public string targetId;  // Identifier used to match triggers with objectives

    // Add questName field
    public string questName; // Added to store the name of the quest for runtime linking

    protected abstract void TriggerEvent();

    // protected virtual void Start()
    // {
    //     SubscribeToObjectiveStatus();
    // }

    public void SubscribeToObjectiveStatus()
    {
        print(this.gameObject.name + " check1");
        if (questInstance != null && questInstance.objectives.Count > objectiveIndex)
        {
            print(this.gameObject.name + " check2");
            questInstance.objectives[objectiveIndex].OnStatusChanged += HandleStatusChanged;
            HandleStatusChanged(questInstance.objectives[objectiveIndex]._status);  // Initial state
        }
    }

    private void HandleStatusChanged(ObjectiveStatus status)
    {
        // gameObject.SetActive(status == ObjectiveStatus.Active);
        this.enabled = status == ObjectiveStatus.Active;
    }

    protected void Complete()
    {
        if (questInstance != null)
        {
            var currentObjective = questInstance.objectives[objectiveIndex];
            if (currentObjective._status == ObjectiveStatus.Active)
            {
                TriggerEvent();
                onCompletion.Invoke();
                currentObjective._status = ObjectiveStatus.Completed;
                this.enabled = false; // Optionally disable this trigger after completion
            }
            else
            {
                Debug.LogWarning("Trying to complete an objective that is not active.");
            }
        }
    }

    protected virtual void OnDestroy()
    {
        if (questInstance != null && questInstance.objectives.Count > objectiveIndex)
        {
            questInstance.objectives[objectiveIndex].OnStatusChanged -= HandleStatusChanged;
        }
    }
}
