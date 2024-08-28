using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class QuestManagerEditorWindow : EditorWindow
{
    private QuestManager questManager;

    [MenuItem("Window/Quest Manager")]
    public static void ShowWindow()
    {
        GetWindow<QuestManagerEditorWindow>("Quest Manager");
    }

    private void OnGUI()
    {
        GUILayout.Label("Active Quests", EditorStyles.boldLabel);

        if (!Application.isPlaying)
        {
            EditorGUILayout.HelpBox("Quest Manager can only be viewed while the game is running.", MessageType.Info);
            return;
        }

        if (questManager == null)
        {
            questManager = FindObjectOfType<QuestManager>();
        }

        if (questManager == null)
        {
            EditorGUILayout.HelpBox("Quest Manager not found in the scene.", MessageType.Warning);
            return;
        }

        foreach (var quest in questManager.ActiveQuests)
        {
            GUILayout.Space(10);
            GUILayout.Label($"Quest: {quest.questName} - Status: {quest.status}", EditorStyles.boldLabel);

            DisplayObjectives("Active Objectives", quest.objectives, ObjectiveStatus.Active);
            DisplayObjectives("Hidden Objectives", quest.objectives, ObjectiveStatus.Hidden);
            DisplayObjectives("Completed Objectives", quest.objectives, ObjectiveStatus.Completed);
            DisplayObjectives("Skipped Objectives", quest.objectives, ObjectiveStatus.Skipped);
        }
    }

    private void DisplayObjectives(string title, List<QuestInstanceObjective> objectives, ObjectiveStatus status)
    {
        GUILayout.Label(title, EditorStyles.boldLabel);
        foreach (var objective in objectives)
        {
            if (objective.status == status)
            {
                GUILayout.Label($"  Objective: {objective.description}");
                GUILayout.Label($"  Type: {objective.objectiveType}");
                GUILayout.Label($"  Status: {objective.status}");
                if (objective.objectiveType == ObjectiveType.Item)
                {
                    GUILayout.Label($"  Progress: {objective.currentItemCount}/{objective.requiredItemCount}");
                }
            }
        }
    }

    private void OnInspectorUpdate()
    {
        Repaint();
    }
}
