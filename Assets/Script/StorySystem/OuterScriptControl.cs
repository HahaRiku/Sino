using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OuterScriptControl : MonoBehaviour {

    public List<StoryEvent> Events;
}

[System.Serializable]
public class StoryEvent
{
    public string Name;
    public UnityEvent Event;
}
