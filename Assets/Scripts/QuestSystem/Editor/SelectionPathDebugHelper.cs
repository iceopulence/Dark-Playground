using System.Collections.Generic;
using UnityEngine;

public static class SelectionPathDebugHelper
{
    public static string SerializeSelectionPath(LinkedList<QuestSystemNode> path)
    {
        List<string> nodeData = new List<string>();

        foreach (var node in path)
        {
            nodeData.Add($"ID: {node.ID}, Name: {node.title}, GUID: {node.GUID}");
        }

        return JsonUtility.ToJson(new SelectionPathData(nodeData));
    }

    private class SelectionPathData
    {
        public List<string> Nodes;

        public SelectionPathData(List<string> nodes)
        {
            Nodes = nodes;
        }
    }
}
