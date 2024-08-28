using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public abstract class QuestSystemNode : Node
{
    private static int _nextId = 1;

    public int ID { get; private set; }
    public string GUID;
    public QuestSystemNode LeftConnectedNode { get; set; }
    public QuestSystemNode RightConnectedNode { get; set; }

    public string QuestName { get; set; }
    public string QuestDescription { get; set; }
    public bool EntryPoint { get; set; }

    public Port inputPort;
    public Port outputPort;

    public QuestSystemNode()
    {
        ID = _nextId++;
    }

    public abstract void Initialize(QuestGraphView graphView);

    protected Port CreatePort(QuestGraphView graphView, Direction direction, Port.Capacity capacity)
    {
        var port = InstantiatePort(Orientation.Horizontal, direction, capacity, typeof(float));
        port.AddManipulator(new EdgeConnector<Edge>(new EdgeConnectorListener(graphView)));
        return port;
    }
}
