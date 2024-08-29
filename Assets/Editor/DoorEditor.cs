using UnityEngine;
using UnityEditor;
using System.IO;

[CustomEditor(typeof(Door))]
public class DoorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Door doorScript = (Door)target;

        if (GUILayout.Button("Generate Key"))
        {
            GenerateKeyForDoor(doorScript);
        }
    }

    private void GenerateKeyForDoor(Door door)
    {
        // Define the offset in local coordinates (e.g., right in front of the door)
        Vector3 offset = new Vector3(0f, 0f, 4f); // Change this as needed

        // Correct way to list prefab names within the Resources/Keys folder
        Object[] loadedKeyPrefabs = Resources.LoadAll("Keys", typeof(GameObject));
        if (loadedKeyPrefabs.Length == 0)
        {
            Debug.LogError("No key prefabs found in Resources/Keys folder!");
            return;
        }

        // Pick a random key prefab
        GameObject keyPrefab = (GameObject)loadedKeyPrefabs[Random.Range(0, loadedKeyPrefabs.Length)];

        if (keyPrefab == null)
        {
            Debug.LogError("Failed to load key prefab.");
            return;
        }

        // Use the existing requiredKey from the door script
        string requiredKey = door.requiredKey;
        if (string.IsNullOrEmpty(requiredKey))
        {
            Debug.LogWarning("Required key on door is not set. Please set a key ID on the door first.");
            return;
        }

        // Instantiate and configure the key prefab
        GameObject keyInstance = Instantiate(keyPrefab, door.transform.position + door.transform.TransformDirection(offset), door.transform.rotation);
        Key keyScript = keyInstance.GetComponent<Key>();
        if (keyScript == null)
        {
            keyScript = keyInstance.AddComponent<Key>();
        }
        keyScript.keyID = requiredKey;

        Debug.Log("Key generated with ID: " + requiredKey + " at position: " + keyInstance.transform.position);
    }
}