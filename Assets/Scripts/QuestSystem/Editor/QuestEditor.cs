using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Quest))]
public class QuestEditor : Editor
{
    private Transform objectiveTransform;
    private ObjectiveType objectiveType;
    private ObjectiveStatus objectiveStatus;
    private bool isOptional;
    private int requiredItemCount = 1;
    private string targetId;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Quest quest = (Quest)target;

        GUILayout.Space(10);
        GUILayout.Label("Add New Objective", EditorStyles.boldLabel);

        objectiveTransform = (Transform)EditorGUILayout.ObjectField("Objective Transform", objectiveTransform, typeof(Transform), true);
        objectiveType = (ObjectiveType)EditorGUILayout.EnumPopup("Objective Type", objectiveType);
        objectiveStatus = (ObjectiveStatus)EditorGUILayout.EnumPopup("Objective Status", objectiveStatus);
        isOptional = EditorGUILayout.Toggle("Is Optional", isOptional);

        if (objectiveType == ObjectiveType.Item)
        {
            requiredItemCount = EditorGUILayout.IntField("Required Item Count", requiredItemCount);
        }

        targetId = EditorGUILayout.TextField("Target ID", targetId);

        if (GUILayout.Button("Add Objective"))
        {
            AddObjectiveToQuest(quest);
        }

        if (GUILayout.Button("Remove Last Objective"))
        {
            if (quest.objectives.Count > 0)
            {
                quest.objectives.RemoveAt(quest.objectives.Count - 1);
            }
        }

        GUILayout.Space(10);
        GUILayout.Label("Branching", EditorStyles.boldLabel);
        int objectiveIndex = EditorGUILayout.IntField("Objective Index to Branch", 0);
        if (GUILayout.Button("Branch to Objective"))
        {
            quest.BranchToObjective(objectiveIndex);
        }
    }

    private void AddObjectiveToQuest(Quest quest)
    {
        if (objectiveTransform != null)
        {
            QuestObjective newObjective = CreateObjectiveComponent(quest, objectiveTransform);
            quest.objectives.Add(newObjective);

            // Instead of linking, store the necessary information
            StoreConfiguration(quest, newObjective, objectiveTransform);

            objectiveTransform = null; // Reset the transform after adding
        }
        else
        {
            EditorUtility.DisplayDialog("Error", "Please assign a Transform.", "OK");
        }
    }

    private void StoreConfiguration(Quest quest, QuestObjective objective, Transform transform)
    {
        QuestTrigger questTrigger = null;
        // Example assuming you have specific classes for each trigger type
        switch (objectiveType)
        {
            case ObjectiveType.Location:
                questTrigger = transform.GetComponent<QuestLocationTrigger>() ?? transform.gameObject.AddComponent<QuestLocationTrigger>();
                break;
            case ObjectiveType.Item:
                questTrigger = transform.GetComponent<CollectibleQuestItem>() ?? transform.gameObject.AddComponent<CollectibleQuestItem>();
                break;
            // Add other cases as necessary
            default:
                Debug.LogError("Unsupported objective type for triggers.");
                return;
        }

        if (questTrigger != null)
        {
            questTrigger.questName = quest.questName;
            questTrigger.objectiveIndex = quest.objectives.IndexOf(objective);
            questTrigger.targetId = objective.targetId;
        }
    }


    private QuestObjective CreateObjectiveComponent(Quest quest, Transform transform)
    {
        // Create and return the new objective based on the current editor fields
        return new QuestObjective(objectiveType)
        {
            description = GetObjectiveDescription(objectiveType, transform.gameObject.name),
            location = transform.position,
            sceneName = transform.gameObject.scene.name,
            status = objectiveStatus,
            isOptional = isOptional,
            requiredItemCount = requiredItemCount,
            targetId = transform.gameObject.name // or `targetId` from editor
        };
    }

    private string GetObjectiveDescription(ObjectiveType type, string objectiveName)
    {
        switch (type)
        {
            case ObjectiveType.Location:
                return "Reach the " + objectiveName;
            case ObjectiveType.Item:
                return "Collect the " + objectiveName;
            case ObjectiveType.DialogueNode:
                return "Pass the specified dialogue node.";
            case ObjectiveType.Kill:
                return "Defeat the " + objectiveName;
            case ObjectiveType.Escort:
                return "Escort the specified NPC to " + objectiveName;
            case ObjectiveType.Interactable:
                return "Interact with the " + objectiveName;
            default:
                return "Complete the objective at " + objectiveName;
        }
    }

}
