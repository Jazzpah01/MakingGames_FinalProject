using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents instance;

    public delegate void GameEvent(object data);
    public Dictionary<string, GameEvent> gameEvents;

    private void Awake()
    {
        instance = this;
    }

    public void Subscribe(GameEvent function, string eventName)
    {
        if (gameEvents.ContainsKey(eventName))
        {
            gameEvents[eventName] += function;
        } else
        {
            gameEvents.Add(eventName, function);
        }
    }

    public void UnSubscribe(GameEvent function, string eventName)
    {
        if (gameEvents.ContainsKey(eventName))
        {
            gameEvents[eventName] -= function;
        }
        else
        {
            throw new System.Exception("Cannot unsubscribe from an unexisting game event.");
        }
    }

    public void TriggerGameEvent(string eventName, object data)
    {
        if (!gameEvents.ContainsKey(eventName))
            throw new System.Exception("Event doesnt exist!");

        gameEvents[eventName](data);
    }
}