using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Present in every scene
// add bgm to this gameobject to continue playing between scenes

public class AudioManager : MonoBehaviour
{
    public static Player player;

    static bool created = false;
    void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }
        else Destroy(gameObject);
    }
}
