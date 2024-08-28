using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest Container", menuName = "Quests/Quest Container")]
public class QuestContainer : ScriptableObject
{
    public List<NodeLinkData> nodeLinks = new List<NodeLinkData>();
    public List<QuestNodeData> questNodeData = new List<QuestNodeData>();
    public List<ObjectiveNodeData> objectiveNodeData = new List<ObjectiveNodeData>(); // Added this list
}

[System.Serializable]
public class NodeLinkData
{
    public string baseNodeGuid;
    public string targetNodeGuid;
}

[System.Serializable]
public class QuestNodeData
{
    public string GUID;
    public string questName;
    public string questDescription;
    public Vector2 position;
    public bool entryPoint; // This property is included
    // Add any other fields that are part of the QuestNodeData
}


[System.Serializable]
public class ObjectiveNodeData
{
    public string GUID;
    public string objectiveDescription;
    public ObjectiveType objectiveType;
    public bool isOptional;
    public string targetId;
    public Vector2 position;
    public Transform targetTransform; // This property is included
}
