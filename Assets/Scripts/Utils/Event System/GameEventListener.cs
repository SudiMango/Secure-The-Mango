using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class CustomGameEvent : UnityEvent<Component, object> { }

public class GameEventListener : MonoBehaviour
{
    public GameEvent gameEvent;

    public CustomGameEvent response;

    private void OnEnable()
    {
        gameEvent.registerListener(this);
    }

    private void OnDisable()
    {
        gameEvent.unregisterListener(this);
    }

    public void onEventRaised(Component sender, object data)
    {
        response.Invoke(sender, data);
    }
}
