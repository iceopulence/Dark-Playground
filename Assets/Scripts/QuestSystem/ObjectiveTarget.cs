using System;

[Serializable]
public class ObjectiveTarget
{
    public ObjectiveTargetType targetType;
    public string targetId;

    public ObjectiveTarget(ObjectiveTargetType type, string id)
    {
        targetType = type;
        targetId = id;
    }
}

public enum ObjectiveTargetType
{
    Item,
    DialogueNode,
    NPC,
    Location
}
