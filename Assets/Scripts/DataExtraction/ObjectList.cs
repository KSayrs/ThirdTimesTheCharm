using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// call from any script:
//  Debug.Log(ObjectLists.CreateFromJSON(DataInitializer.JsonReferences["Objects"]));

[Serializable]
public class ObjectList
{
    public List<objectDef> Objects;

    public static ObjectList CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<ObjectList>(jsonString);
    }
}

[Serializable]
public class objectDef
{
    public string name;
    public string image; // path to image file
    public string sound; // path to audio file
    public string type; // "Match" or "Theme" bonus
    public string description;
    public int value;
    public int cost;
    public bool isException = false;
    public List<string> traits;
    public List<string> allowed_modifiers;
}

