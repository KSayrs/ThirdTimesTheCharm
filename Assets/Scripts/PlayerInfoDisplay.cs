using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoDisplay : MonoBehaviour
{
    public static Player player;

    void Awake()
    {
        player = new Player();
    }
}

public class Player
{
    public List<objectDef> chosenItems;
    public Client client;
    public int budget;

    public Player()
    {
        chosenItems = new List<objectDef>();
        client = null;
        budget = 0;
    }

    public void Clear()
    {
        chosenItems = new List<objectDef>();
        client = null;
        budget = 0;
    }
}