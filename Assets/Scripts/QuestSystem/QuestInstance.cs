using System.Collections.Generic;
using UnityEngine;

public class QuestInstance
{
    public string questName;
    public string questDescription;
    public List<QuestInstanceObjective> objectives;
    public QuestStatus status;
    public string nextQuestGUID; // Add this field

    public QuestInstance(Quest template)
    {
        questName = template.questName;
        questDescription = template.questDescription;
        objectives = new List<QuestInstanceObjective>();
        foreach (var obj in template.objectives)
        {
            objectives.Add(new QuestInstanceObjective(obj));
        }
        status = QuestStatus.NotStarted;
    }

    public bool CheckObjectives()
    {
        foreach (var objective in objectives)
        {
            if (objective.status != ObjectiveStatus.Completed && !objective.isOptional)
            {
                status = QuestStatus.InProgress;
                return false;
            }
        }
        status = QuestStatus.Completed;
        return true;
    }

    public void RevealNextObjective()
    {
        for (int i = 0; i < objectives.Count; i++)
        {
            if (objectives[i].status == ObjectiveStatus.Hidden && !objectives[i].isOptional)
            {
                objectives[i].status = ObjectiveStatus.Active;

                ActivateOptionals(i);
                break;
            }
            else if(objectives[i]._status == ObjectiveStatus.Active)
            {
                ActivateOptionals(i);
                break;
            }
        }
    }

    private void ActivateOptionals(int nextObjectiveIndex)
    {
        for (int j = nextObjectiveIndex + 1; j < objectives.Count; j++)
                {
                    if (objectives[j].isOptional)
                    {
                        objectives[j].status = ObjectiveStatus.Active;
                    }
                    else
                    {
                        break;
                    }
                }
    }

    public void SkipOptionalObjectives()
    {
        foreach (var objective in objectives)
        {
            if (objective.isOptional && objective.status == ObjectiveStatus.Active)
            {
                objective.status = ObjectiveStatus.Skipped;
            }
        }
    }

    public void BranchToObjective(int objectiveIndex)
    {
        if (objectiveIndex >= 0 && objectiveIndex < objectives.Count)
        {
            foreach (var objective in objectives)
            {
                objective.status = ObjectiveStatus.Hidden;
            }
            objectives[objectiveIndex].status = ObjectiveStatus.Active;
        }
    }

    public void ResetObjectives()
    {
        foreach (var objective in objectives)
        {
            objective.status = ObjectiveStatus.Hidden;
            objective.currentItemCount = 0;
        }
        status = QuestStatus.NotStarted;
    }
}

public class QuestInstanceObjective
{
    public delegate void StatusChangedHandler(ObjectiveStatus newStatus);
    public event StatusChangedHandler OnStatusChanged;

    public string description;
    public ObjectiveType objectiveType;
    public ObjectiveStatus _status; // Backing field for the status property
    public bool isOptional;
    public Vector3 location;
    public string sceneName;
    public int requiredItemCount;
    public int currentItemCount;
    public string targetId;
    public QuestInstanceObjective nextObjective;  // Reference to the next objective

    public QuestInstanceObjective(QuestObjective template)
    {
        description = template.description;
        objectiveType = template.objectiveType;
        _status = template.status; // Initialize the backing field, not the property
        isOptional = template.isOptional;
        location = template.location;
        sceneName = template.sceneName;
        requiredItemCount = template.requiredItemCount;
        currentItemCount = template.currentItemCount;
        targetId = template.targetId;
    }

    public ObjectiveStatus status
    {
        get => _status;
        set
        {
            if (_status != value)
            {
                _status = value;
                OnStatusChanged?.Invoke(_status);
            }
        }
    }
}
