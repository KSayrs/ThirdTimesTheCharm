using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// call from any script:
//  Debug.Log(ObjectLists.GetFromJson("Assets/Data/Objects.json"));

[Serializable]
public class ObjectLists
{
    public List<objectDef> Objects;

    public static ObjectLists GetFromJson(string path)
    {
        return CreateFromJSON(System.IO.File.ReadAllText(System.IO.Path.Combine(Application.streamingAssetsPath, path)));
    }

    private static ObjectLists CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<ObjectLists>(jsonString);
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
    public List<string> traits;
    public List<string> allowed_modifiers;
}

