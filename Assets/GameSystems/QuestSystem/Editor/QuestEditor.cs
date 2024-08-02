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
            if (objectiveTransform != null || objectiveType == ObjectiveType.Location)
            {
                string objectiveID = objectiveTransform.gameObject.name;
                QuestObjective newObjective = new QuestObjective(objectiveType)
                {
                    description = GetObjectiveDescription(objectiveType, objectiveTransform.gameObject.name),
                    location = objectiveTransform != null ? objectiveTransform.position : Vector3.zero,
                    sceneName = objectiveTransform != null ? objectiveTransform.gameObject.scene.name : "",
                    status = objectiveStatus,
                    isOptional = isOptional,
                    requiredItemCount = requiredItemCount,
                    targetId = objectiveID
                };

                if (objectiveType == ObjectiveType.Item)
                {
                    CollectibleQuestItem collectibleItem = objectiveTransform.GetComponent<CollectibleQuestItem>();
                    if (collectibleItem == null)
                    {
                        collectibleItem = objectiveTransform.gameObject.AddComponent<CollectibleQuestItem>();
                        collectibleItem.itemId = objectiveID;
                        Debug.Log("Added collectible quest item component to " + objectiveTransform.gameObject.name + " with ID: " + collectibleItem.itemId);
                    }
                    collectibleItem.itemId = objectiveID;
                }
                else if(objectiveType == ObjectiveType.Interactable)
                {
                    var interactableObjective = objectiveTransform.GetComponent<QuestInteractable>();
                    if (interactableObjective == null)
                    {
                        interactableObjective = objectiveTransform.gameObject.AddComponent<QuestInteractable>();
                        Debug.Log("Added collectible quest item component to " );
                    }
                    interactableObjective.itemId = objectiveID;
                }

                quest.objectives.Add(newObjective);
                objectiveTransform = null;
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "Please assign a Transform.", "OK");
            }
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
                return "Objective";
        }
    }
}
