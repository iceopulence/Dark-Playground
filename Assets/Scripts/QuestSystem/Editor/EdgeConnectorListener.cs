using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EdgeConnectorListener : IEdgeConnectorListener
{
    private QuestGraphView graphView;
    private Port cachedOutputPort; // Add this field

    public EdgeConnectorListener(QuestGraphView graphView)
    {
        this.graphView = graphView ?? throw new System.ArgumentNullException(nameof(graphView));
    }

    public void OnDrop(GraphView graphView, Edge edge)
    {
        Debug.Log("Edge dropped on port.");
        if (edge.input != null && edge.output != null)
        {
            graphView.AddElement(edge);
        }
    }

    public void OnDropOutsidePort(Edge edge, Vector2 position)
    {
        Debug.Log("Edge dropped outside port.");
        cachedOutputPort = edge.output; // Cache the output port
        ShowContextMenu(position);
    }

    private void ShowContextMenu(Vector2 position)
    {
        if (graphView == null)
        {
            Debug.LogError("GraphView is null!");
            return;
        }

        var menu = new GenericMenu();
        menu.AddItem(new GUIContent("Add Item Objective Node"), false, () =>
        {
            graphView.CreateAndConnectNode(cachedOutputPort, "ObjectiveNode", position, ObjectiveType.Item);
        });
        menu.AddItem(new GUIContent("Add Location Objective Node"), false, () =>
        {
            graphView.CreateAndConnectNode(cachedOutputPort, "ObjectiveNode", position, ObjectiveType.Location);
        });
        // Add more node types here as needed

        menu.DropDown(new Rect(position, Vector2.zero));
    }
}