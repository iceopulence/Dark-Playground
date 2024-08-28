using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class QuestNode : QuestSystemNode
{
    public string questName;
    public string questDescription;
    public bool entryPoint;

    public QuestNode(QuestGraphView graphView)
    {
        GUID = System.Guid.NewGuid().ToString();
        title = "Quest Node";
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

        var questNameField = new TextField("Quest Name") { value = questName };
        questNameField.RegisterValueChangedCallback(evt => questName = evt.newValue);
        mainContainer.Add(questNameField);

        var questDescriptionField = new TextField("Quest Description") { value = questDescription };
        questDescriptionField.RegisterValueChangedCallback(evt => questDescription = evt.newValue);
        mainContainer.Add(questDescriptionField);

        var entryPointToggle = new Toggle("Entry Point") { value = entryPoint };
        entryPointToggle.RegisterValueChangedCallback(evt => entryPoint = evt.newValue);
        mainContainer.Add(entryPointToggle);

        RefreshExpandedState();
        RefreshPorts();
    }
}
