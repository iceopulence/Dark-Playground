using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SelectionPath
{
    private LinkedList<GraphElement> _path = new LinkedList<GraphElement>();
    public GraphElement ActiveNode { get; private set; }

    public void StartSelection(GraphElement node)
    {
        if (node == null)
        {
            Debug.LogError("Cannot start selection with a null node.");
            return;
        }
        _path.Clear();
        _path.AddLast(node);
        ActiveNode = node;
        LogSelectionPath();

        foreach(GraphElement element in _path)
        {
            Debug.Log(element.ToString());
        }
    }

    public void Clear()
    {
        _path.Clear();
    }

    public void ExpandSelection(GraphElement node)
    {
        if (node == null)
        {
            Debug.LogError("Cannot expand selection with a null node.");
            return;
        }
        _path.AddLast(node);
        ActiveNode = node;
        LogSelectionPath();
    }

    public void ContractSelection()
    {
        if (_path.Count > 1)
        {
            _path.RemoveLast();
            ActiveNode = _path.Last?.Value;
            LogSelectionPath();
        }
        else
        {
            Debug.LogWarning("Cannot contract selection, path has only one node.");
        }
    }

    public void LogSelectionPath()
    {
        string pathString = string.Join(" -> ", _path.Select(node => (node as QuestSystemNode)?.ID.ToString() ?? node.name));
        Debug.Log($"Selection Path: {pathString}");
        Debug.Log($"Active Node: {(ActiveNode as QuestSystemNode)?.ID.ToString() ?? ActiveNode.name}");
    }

    public void SetActiveNode(GraphElement node)
    {
        if (node == null)
        {
            Debug.LogError("Cannot set active node to null.");
            return;
        }
        ActiveNode = node;
        LogSelectionPath();
    }
}