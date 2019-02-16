using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EventList
{
    public List<Event> Events;

    public static EventList CreateFromJSON(string jsonString)
    {
      //  jsonString = System.Text.Encoding.UTF8.GetString(jsonString.bytes, 3, jsonString.bytes.Length - 3);
        return JsonUtility.FromJson<EventList>(jsonString);
    }
}

[Serializable]
public class Event
{
    public string name;
    public string description;
    public int cost;
    public string button1;
    public string button2;
    public bool buttonMatters;
}
