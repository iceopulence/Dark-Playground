using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class QuestEditorWindow : EditorWindow
{
    private QuestGraphView _graphView;

    [MenuItem("Window/Quest Editor")]
    public static void OpenWindow()
    {
        var window = GetWindow<QuestEditorWindow>();
        window.titleContent = new GUIContent("Quest Editor");
    }

    private void OnEnable()
    {
        ConstructGraphView();
        GenerateToolbar();
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(_graphView);
    }

    private void ConstructGraphView()
    {
        _graphView = new QuestGraphView
        {
            name = "Quest Graph"
        };

        _graphView.StretchToParentSize();
        rootVisualElement.Add(_graphView);
    }

    private void GenerateToolbar()
    {
        var toolbar = new Toolbar();

        var nodeCreateButton = new Button(() =>
        {
            var node = _graphView.CreateQuestNode("Quest Node", _graphView.viewTransform.matrix.inverse.MultiplyPoint(Vector3.zero));
            _graphView.AddElement(node);
        })
        {
            text = "Create Quest Node"
        };
        toolbar.Add(nodeCreateButton);

        var objectiveCreateButton = new Button(() =>
        {
            var node = _graphView.CreateObjectiveNode("Objective Node", _graphView.viewTransform.matrix.inverse.MultiplyPoint(Vector3.zero), ObjectiveType.Item); // Example type
            _graphView.AddElement(node);
        })
        {
            text = "Create Objective Node"
        };
        toolbar.Add(objectiveCreateButton);

        rootVisualElement.Add(toolbar);
    }
}
