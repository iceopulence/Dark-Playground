using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class QuestGraphView : GraphView
{
    public Vector2 defaultNodeSize = new Vector2(200, 150);
    private List<GraphElement> clipboard = new List<GraphElement>();
    private Vector2 lastMousePosition;
    private SelectionPath selectionPath;

    public QuestGraphView()
    {
        selectionPath = new SelectionPath();

        var styleSheet = Resources.Load<StyleSheet>("QuestGraphView");
        if (styleSheet == null)
        {
            Debug.LogError("StyleSheet not found! Please ensure QuestGraphView.uss is in the Resources folder.");
        }
        else
        {
            styleSheets.Add(styleSheet);
        }

        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var grid = new GridBackground();
        Insert(0, grid);
        grid.StretchToParentSize();

        RegisterCallback<KeyDownEvent>(OnKeyDown);
        RegisterCallback<MouseMoveEvent>(OnMouseMove);
    }

    private void OnMouseMove(MouseMoveEvent evt)
    {
        lastMousePosition = evt.mousePosition;
    }

    private void OnKeyDown(KeyDownEvent evt)
    {
        if (evt.ctrlKey)
        {
            switch (evt.keyCode)
            {
                case KeyCode.A:
                    SelectAllNodes();
                    evt.StopPropagation();
                    break;
                case KeyCode.C:
                    CopySelectedNodes();
                    evt.StopPropagation();
                    break;
                case KeyCode.V:
                    PasteNodes();
                    evt.StopPropagation();
                    break;
                case KeyCode.D:
                    DuplicateSelectedNodes();
                    evt.StopPropagation();
                    break;
            }
        }
        else if (evt.shiftKey)
        {
            switch (evt.keyCode)
            {
                case KeyCode.LeftArrow:
                    NavigateConnectedNodes(Direction.Left);
                    evt.StopPropagation();
                    break;
                case KeyCode.RightArrow:
                    NavigateConnectedNodes(Direction.Right);
                    evt.StopPropagation();
                    break;
            }
        }
    }

    private void SelectAllNodes()
    {
        ClearSelection();
        foreach (var node in nodes.ToList())
        {
            AddToSelection(node);
        }
    }

    private void CopySelectedNodes()
    {
        clipboard.Clear();
        foreach (var element in selection)
        {
            if (element is GraphElement graphElement && (graphElement is QuestNode || graphElement is QuestObjectiveNode))
            {
                clipboard.Add(graphElement);
            }
        }
    }

    private void PasteNodes()
    {
        ClearSelection();
        foreach (var element in clipboard)
        {
            if (element is QuestNode questNode)
            {
                var newNode = CreateQuestNode(questNode.title, lastMousePosition != Vector2.zero ? lastMousePosition : questNode.GetPosition().position + new Vector2(20, 20));
                newNode.QuestName = questNode.QuestName;
                newNode.QuestDescription = questNode.QuestDescription;
                AddElement(newNode);
                AddToSelection(newNode);
                selectionPath.StartSelection(newNode);
            }
            else if (element is QuestObjectiveNode objectiveNode)
            {
                var newNode = CreateObjectiveNode(objectiveNode.ObjectiveDescription, lastMousePosition != Vector2.zero ? lastMousePosition : objectiveNode.GetPosition().position + new Vector2(20, 20), objectiveNode.ObjectiveType);
                newNode.IsOptional = objectiveNode.IsOptional;
                newNode.TargetId = objectiveNode.TargetId;
                newNode.TargetTransform = objectiveNode.TargetTransform;
                AddElement(newNode);
                AddToSelection(newNode);
                selectionPath.StartSelection(newNode);
            }
        }
    }

    private void DuplicateSelectedNodes()
    {
        ClearSelection();
        List<GraphElement> elementsToDuplicate = new List<GraphElement>();
        foreach (var element in selection)
        {
            if (element is GraphElement graphElement && (graphElement is QuestNode || graphElement is QuestObjectiveNode))
            {
                elementsToDuplicate.Add(graphElement);
            }
        }
        foreach (var element in elementsToDuplicate)
        {
            if (element is QuestNode questNode)
            {
                var newNode = CreateQuestNode(questNode.title, questNode.GetPosition().position + new Vector2(20, 20));
                newNode.QuestName = questNode.QuestName;
                newNode.QuestDescription = questNode.QuestDescription;
                AddElement(newNode);
                AddToSelection(newNode);
                selectionPath.StartSelection(newNode);
            }
            else if (element is QuestObjectiveNode objectiveNode)
            {
                var newNode = CreateObjectiveNode(objectiveNode.ObjectiveDescription, objectiveNode.GetPosition().position + new Vector2(20, 20), objectiveNode.ObjectiveType);
                newNode.IsOptional = objectiveNode.IsOptional;
                newNode.TargetId = objectiveNode.TargetId;
                newNode.TargetTransform = objectiveNode.TargetTransform;
                AddElement(newNode);
                AddToSelection(newNode);
                selectionPath.StartSelection(newNode);
            }
        }
    }

    private void NavigateConnectedNodes(Direction direction)
    {
        if (selectionPath.ActiveNode == null)
        {
            if (selection.Any())
            {
                selectionPath.StartSelection(selection.Last() as GraphElement);
            }
            else
            {
                return;
            }
        }

        var activeNode = selectionPath.ActiveNode;

        if (activeNode is QuestSystemNode questSystemNode)
        {
            QuestSystemNode nextNode = null;

            if (direction == Direction.Left)
            {
                nextNode = questSystemNode.LeftConnectedNode;
                if (nextNode != null)
                {
                    selectionPath.ContractSelection();
                    RemoveFromSelection(questSystemNode);
                }
            }
            else if (direction == Direction.Right)
            {
                nextNode = questSystemNode.RightConnectedNode;
                if (nextNode != null)
                {
                    selectionPath.ExpandSelection(nextNode);
                    AddToSelection(nextNode);
                }
            }

            if (nextNode != null)
            {
                selectionPath.SetActiveNode(nextNode);
                FocusNode(nextNode);
            }
        }
    }

    private void FocusNode(GraphElement node)
    {
        node.BringToFront();
        node.Focus();
    }

    private enum Direction
    {
        Left,
        Right
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new List<Port>();

        ports.ForEach(port =>
        {
            if (startPort != port && startPort.node != port.node)
            {
                compatiblePorts.Add(port);
            }
        });

        return compatiblePorts;
    }

    public QuestNode CreateQuestNode(string nodeName, Vector2 position)
    {
        var questNode = new QuestNode(this)
        {
            title = nodeName
        };

        questNode.SetPosition(new Rect(position, defaultNodeSize));
        AddElement(questNode);

        Debug.Log($"Created QuestNode with ID: {questNode.ID}, Position: {position}");
        selectionPath.StartSelection(questNode);

        return questNode;
    }

    public QuestObjectiveNode CreateObjectiveNode(string objectiveDescription, Vector2 position, ObjectiveType objectiveType)
    {
        var objectiveNode = new QuestObjectiveNode(this)
        {
            ObjectiveDescription = objectiveDescription,
            ObjectiveType = objectiveType
        };

        objectiveNode.SetPosition(new Rect(position, defaultNodeSize));
        AddElement(objectiveNode);

        Debug.Log($"Created QuestObjectiveNode with ID: {objectiveNode.ID}, Position: {position}");
        selectionPath.StartSelection(objectiveNode);

        return objectiveNode;
    }

    public void CreateAndConnectNode(Port cachedOutputPort, string nodeType, Vector2 position, ObjectiveType objectiveType)
    {
        QuestSystemNode newNode = null;

        switch (nodeType)
        {
            case "QuestNode":
                newNode = CreateQuestNode("New Quest", position);
                break;
            case "ObjectiveNode":
                newNode = CreateObjectiveNode("New Objective", position, objectiveType);
                break;
        }

        if (newNode != null)
        {
            var inputPort = newNode.inputContainer.Query<Port>().First();

            if (inputPort != null && cachedOutputPort != null)
            {
                var tempEdge = new Edge
                {
                    output = cachedOutputPort,
                    input = inputPort
                };

                cachedOutputPort.Connect(tempEdge);
                inputPort.Connect(tempEdge);

                AddElement(tempEdge);

                var cachedNode = cachedOutputPort.node as QuestSystemNode;

                if (cachedNode != null)
                {
                    cachedNode.RightConnectedNode = newNode;
                    newNode.LeftConnectedNode = cachedNode;

                    Debug.Log($"Connected Node {cachedNode.ID} to Node {newNode.ID}");
                }
            }
            else
            {
                Debug.LogError("Ports not found for connecting nodes.");
            }

            // Select the newly created and connected node
            ClearSelection();
            AddToSelection(newNode);
            selectionPath.StartSelection(newNode);
        }
    }

    void ClearSelectionAndPath()
    {
        ClearSelection();
        selectionPath.Clear();
    }
}
