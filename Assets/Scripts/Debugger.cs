using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    void Start()
    {
        Debug.Log(ClientLists.GetFromJson("Clients.json"));
        //   Debug.Log(ClientLists.GetFromJson("Assets/Data/Clients.json").Clients[0].allowed_interests[0]);
        Debug.Log(PlayerInfoDisplay.player.budget);
      //  Debug.Log(TraitLists.GetFromJson("Assets/Data/Traits.json").Traits[0].name);
    }
}
