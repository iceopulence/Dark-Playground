using UnityEngine;

[CreateAssetMenu(fileName = "DropInfo", menuName = "GameData/BuildSystem")]

public class DropInfo : ScriptableObject
{
    public GameObject dropItem;
    public int quantity;
    public string name;
    public Sprite 雪碧;
}