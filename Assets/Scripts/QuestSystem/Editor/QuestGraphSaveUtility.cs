using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class QuestGraphSaveUtility
{
    private QuestGraphView _targetGraphView;
    private List<Edge> Edges => _targetGraphView.edges.ToList();
    private List<QuestNode> QuestNodes => _targetGraphView.nodes.ToList().OfType<QuestNode>().ToList();
    private List<QuestObjectiveNode> ObjectiveNodes => _targetGraphView.nodes.ToList().OfType<QuestObjectiveNode>().ToList();

    public static QuestGraphSaveUtility GetInstance(QuestGraphView targetGraphView)
    {
        return new QuestGraphSaveUtility
        {
            _targetGraphView = targetGraphView
        };
    }

    public void SaveGraph(string fileName)
    {
        var questContainer = ScriptableObject.CreateInstance<QuestContainer>();

        if (!SaveNodes(questContainer)) return;
        SaveEdges(questContainer);

        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
        {
            AssetDatabase.CreateFolder("Assets", "Resources");
        }

        AssetDatabase.CreateAsset(questContainer, $"Assets/Resources/{fileName}.asset");
        AssetDatabase.SaveAssets();
    }

    private bool SaveNodes(QuestContainer questContainer)
    {
        if (!QuestNodes.Any() && !ObjectiveNodes.Any()) return false;

        foreach (var node in QuestNodes)
        {
            questContainer.questNodeData.Add(new QuestNodeData
            {
                GUID = node.GUID,
                questName = node.QuestName,
                questDescription = node.QuestDescription,
                position = node.GetPosition().position,
                entryPoint = node.EntryPoint
            });
        }

        foreach (var node in ObjectiveNodes)
        {
            questContainer.objectiveNodeData.Add(new ObjectiveNodeData
            {
                GUID = node.GUID,
                objectiveDescription = node.ObjectiveDescription,
                objectiveType = node.ObjectiveType,
                isOptional = node.IsOptional,
                targetId = node.TargetId,
                position = node.GetPosition().position,
                targetTransform = node.TargetTransform
            });
        }

        return true;
    }

    private void SaveEdges(QuestContainer questContainer)
    {
        if (!Edges.Any()) return;

        foreach (var edge in Edges)
        {
            var outputNode = edge.output.node as QuestNode;
            var inputNode = edge.input.node as QuestNode;

            if (outputNode == null)
            {
                var outputObjectiveNode = edge.output.node as QuestObjectiveNode;
                if (outputObjectiveNode != null)
                {
                    questContainer.nodeLinks.Add(new NodeLinkData
                    {
                        baseNodeGuid = outputObjectiveNode.GUID,
                        targetNodeGuid = inputNode?.GUID
                    });
                }
            }

            if (inputNode == null)
            {
                var inputObjectiveNode = edge.input.node as QuestObjectiveNode;
                if (inputObjectiveNode != null)
                {
                    questContainer.nodeLinks.Add(new NodeLinkData
                    {
                        baseNodeGuid = outputNode?.GUID,
                        targetNodeGuid = inputObjectiveNode.GUID
                    });
                }
            }

            if (outputNode != null && inputNode != null)
            {
                questContainer.nodeLinks.Add(new NodeLinkData
                {
                    baseNodeGuid = outputNode.GUID,
                    targetNodeGuid = inputNode.GUID
                });
            }
        }
    }

    public void LoadGraph(string fileName)
    {
        var questContainer = Resources.Load<QuestContainer>(fileName);
        if (questContainer == null)
        {
            EditorUtility.DisplayDialog("File Not Found", "Target graph file does not exist!", "OK");
            return;
        }

        ClearGraph();
        CreateNodes(questContainer);
        ConnectNodes(questContainer);
    }

    private void ClearGraph()
    {
        QuestNodes.FindAll(n => !n.EntryPoint).ForEach(n => _targetGraphView.RemoveElement(n));
        ObjectiveNodes.ForEach(n => _targetGraphView.RemoveElement(n));

        Edges.ForEach(edge => _targetGraphView.RemoveElement(edge));
    }

    private void CreateNodes(QuestContainer questContainer)
    {
        foreach (var nodeData in questContainer.questNodeData)
        {
            var tempNode = _targetGraphView.CreateQuestNode(nodeData.questName, nodeData.position);
            tempNode.GUID = nodeData.GUID;
            tempNode.QuestName = nodeData.questName;
            tempNode.QuestDescription = nodeData.questDescription;
            tempNode.EntryPoint = nodeData.entryPoint;

            tempNode.SetPosition(new Rect(nodeData.position, _targetGraphView.defaultNodeSize));
            _targetGraphView.AddElement(tempNode);
        }

        foreach (var nodeData in questContainer.objectiveNodeData)
        {
            var tempNode = _targetGraphView.CreateObjectiveNode(nodeData.objectiveDescription, nodeData.position, nodeData.objectiveType);
            tempNode.GUID = nodeData.GUID;
            tempNode.ObjectiveDescription = nodeData.objectiveDescription;
            tempNode.ObjectiveType = nodeData.objectiveType;
            tempNode.IsOptional = nodeData.isOptional;
            tempNode.TargetId = nodeData.targetId;
            tempNode.TargetTransform = nodeData.targetTransform;

            tempNode.SetPosition(new Rect(nodeData.position, _targetGraphView.defaultNodeSize));
            _targetGraphView.AddElement(tempNode);
        }
    }

    private void ConnectNodes(QuestContainer questContainer)
    {
        foreach (var nodeLink in questContainer.nodeLinks)
        {
            var baseNode = _targetGraphView.nodes.ToList().First(x => (x as QuestNode)?.GUID == nodeLink.baseNodeGuid || (x as QuestObjectiveNode)?.GUID == nodeLink.baseNodeGuid);
            var targetNode = _targetGraphView.nodes.ToList().First(x => (x as QuestNode)?.GUID == nodeLink.targetNodeGuid || (x as QuestObjectiveNode)?.GUID == nodeLink.targetNodeGuid);

            LinkNodes(baseNode, targetNode);
        }
    }

    private void LinkNodes(Node baseNode, Node targetNode)
    {
        var outputPort = baseNode.outputContainer[0] as Port;
        var inputPort = targetNode.inputContainer[0] as Port;

        var tempEdge = new Edge
        {
            output = outputPort,
            input = inputPort
        };

        tempEdge.input.Connect(tempEdge);
        tempEdge.output.Connect(tempEdge);

        _targetGraphView.Add(tempEdge);
    }
}
