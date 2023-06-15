using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemClass : MonoBehaviour
{
    public static EventSystemClass Instance { get; private set; }
    public EventSystem EventSystem { get => eventSystem; private set => eventSystem = value; }

    private EventSystem eventSystem;

    private void Awake()
    {
        if (Instance != null)
            Destroy(this.gameObject);
        else
            Instance = this;

        eventSystem = gameObject.GetComponent<EventSystem>();
    }
}
