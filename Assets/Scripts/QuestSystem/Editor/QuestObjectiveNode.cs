using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class QuestObjectiveNode : QuestSystemNode
{
    public string ObjectiveDescription { get; set; }
    public ObjectiveType ObjectiveType { get; set; }
    public bool IsOptional { get; set; }
    public string TargetId { get; set; }
    public Transform TargetTransform { get; set; }

    public QuestObjectiveNode(QuestGraphView graphView)
    {
        GUID = System.Guid.NewGuid().ToString();
        title = "Objective Node";
        Initialize(graphView);
    }

    public override void Initialize(QuestGraphView graphView)
    {
        // Set a default size
        SetPosition(new Rect(Vector2.zero, new Vector2(200, 150)));

        inputPort = CreatePort(graphView, Direction.Input, Port.Capacity.Multi);
        inputPort.portName = "Input";
        inputContainer.Add(inputPort);

        outputPort = CreatePort(graphView, Direction.Output, Port.Capacity.Multi);
        outputPort.portName = "Output";
        outputContainer.Add(outputPort);

        var objectiveDescriptionField = new TextField("Objective Description") { value = ObjectiveDescription };
        objectiveDescriptionField.RegisterValueChangedCallback(evt => ObjectiveDescription = evt.newValue);
        mainContainer.Add(objectiveDescriptionField);

        var objectiveTypeField = new EnumField("Objective Type", ObjectiveType);
        objectiveTypeField.RegisterValueChangedCallback(evt => ObjectiveType = (ObjectiveType)evt.newValue);
        mainContainer.Add(objectiveTypeField);

        var isOptionalToggle = new Toggle("Is Optional") { value = IsOptional };
        isOptionalToggle.RegisterValueChangedCallback(evt => IsOptional = evt.newValue);
        mainContainer.Add(isOptionalToggle);

        var targetIdField = new TextField("Target ID") { value = TargetId };
        targetIdField.RegisterValueChangedCallback(evt => TargetId = evt.newValue);
        mainContainer.Add(targetIdField);

        var targetTransformField = new ObjectField("Target Transform") { objectType = typeof(Transform), value = TargetTransform };
        targetTransformField.RegisterValueChangedCallback(evt => TargetTransform = (Transform)evt.newValue);
        mainContainer.Add(targetTransformField);

        RefreshExpandedState();
        RefreshPorts();
    }
}
