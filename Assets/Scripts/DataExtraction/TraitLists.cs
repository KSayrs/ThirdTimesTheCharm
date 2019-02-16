using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// call from any script:
//  TraitLists.CreateFromJSON(DataInitializer.JsonReferences["Traits"]));

[Serializable]
public class TraitLists
{
    public List<Trait> Traits;

    public static TraitLists CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<TraitLists>(jsonString);
    }
}

[Serializable]
public class Trait
{
    public string name;
    public string icon; // path to image file
    public bool is_theme;
}
