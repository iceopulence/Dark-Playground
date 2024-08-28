using UnityEngine;
using System.Collections.Generic;

public class DontDestroyObject : MonoBehaviour
{
    public static Dictionary<string, DontDestroyObject> instances = new Dictionary<string, DontDestroyObject>();

    private string instanceId;
    
    private void Awake()
    {
        // Use the GameObject's name as the unique identifier
        instanceId = this.gameObject.name;
        
        if (!instances.ContainsKey(instanceId))
        {
            instances.Add(instanceId, this);
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            if(instances[instanceId] != this)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
