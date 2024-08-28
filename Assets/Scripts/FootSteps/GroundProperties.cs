using UnityEngine;

public class GroundProperties : MonoBehaviour
{
    [SerializeField] FootStepData footStepData;

    public FootStepData GetNewFootStepData()
    {
        return footStepData;
    }

    public bool CompareID(short id)
    {
        return id == footStepData.groundTypeID;
    }
}