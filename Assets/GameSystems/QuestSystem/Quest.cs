using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quests/Quest")]
public class Quest : ScriptableObject
{
    public string questName;
    public string questDescription;
    public List<QuestObjective> objectives;
    public QuestStatus status;

    public Quest linkedQuest;

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

                for (int j = i + 1; j < objectives.Count; j++)
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
}

[System.Serializable]
public class QuestObjective
{
    public string description;
    public ObjectiveType objectiveType;
    public ObjectiveStatus status;
    public bool isOptional;
    public Vector3 location;
    public string sceneName;
    public int requiredItemCount = 1;
    public int currentItemCount = 0;
    public string targetId;

    public QuestObjective(ObjectiveType type)
    {
        objectiveType = type;
        status = ObjectiveStatus.Hidden;
    }
}


public enum QuestStatus
{
    NotStarted,
    InProgress,
    Completed
}

public enum ObjectiveType
{
    Location,
    Item,
    DialogueNode,
    Kill,
    Escort,
    Interactable
}

public enum ObjectiveStatus
{
    Hidden,
    Active,
    Completed,
    Skipped
}
