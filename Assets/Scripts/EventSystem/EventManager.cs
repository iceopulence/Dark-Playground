using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private static EventManager _instance;
    public static EventManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("EventManager");
                _instance = go.AddComponent<EventManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    private Dictionary<GameEventType, List<Action<GameEvent>>> eventDictionary = new Dictionary<GameEventType, List<Action<GameEvent>>>();

    private void OnDestroy()
    {
        eventDictionary.Clear();
    }

    public static void StartListening(GameEventType eventType, Action<GameEvent> listener)
    {
        if (Instance.eventDictionary.TryGetValue(eventType, out List<Action<GameEvent>> thisEvent))
        {
            thisEvent.Add(listener);
        }
        else
        {
            thisEvent = new List<Action<GameEvent>> { listener };
            Instance.eventDictionary.Add(eventType, thisEvent);
        }
    }

    public static void StopListening(GameEventType eventType, Action<GameEvent> listener)
    {
        if (_instance == null) return;
        if (Instance.eventDictionary.TryGetValue(eventType, out List<Action<GameEvent>> thisEvent))
        {
            thisEvent.Remove(listener);
            if (thisEvent.Count == 0)
            {
                Instance.eventDictionary.Remove(eventType);
            }
        }
    }

    public static void TriggerEvent(GameEventType eventType, object eventParam = null)
    {
        if (Instance.eventDictionary.TryGetValue(eventType, out List<Action<GameEvent>> thisEvent))
        {
            GameEvent gameEvent = new GameEvent(eventType, eventParam);
            for (int i = thisEvent.Count - 1; i >= 0; i--)
            {
                thisEvent[i]?.Invoke(gameEvent);
            }
        }
    }
}

public enum GameEventType
{
    ItemCollected,
    EnemyDefeated,
    LocationReached,
    DialogueNodePassed,
    PlayerResponseSelected,
    EscortTargetReached // Added for escort objectives
}

public class GameEvent
{
    public GameEventType EventType { get; private set; }
    public object EventData { get; private set; }

    public GameEvent(GameEventType eventType, object eventData)
    {
        EventType = eventType;
        EventData = eventData;
    }
}
