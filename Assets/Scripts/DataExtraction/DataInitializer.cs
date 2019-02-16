// putting this on the same gameObject that the AudioManager is on

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataInitializer : MonoBehaviour
{
    [SerializeField] List<JsonReference> Jsons = null;
    public static Dictionary<string, string> JsonReferences;

    void Awake()
    {
        JsonReferences = new Dictionary<string, string>();

        foreach (var json in Jsons)
        {
            JsonReferences.Add(json.id, json.json.text);
            Debug.Log(json.id);
        }

        Data.Clients = ClientLists.CreateFromJSON(JsonReferences["Clients"]).Clients;
        Data.Traits = TraitLists.CreateFromJSON(JsonReferences["Traits"]).Traits;
        Data.Objects = ObjectList.CreateFromJSON(JsonReferences["Objects"]).Objects;
        Data.Events = EventList.CreateFromJSON(JsonReferences["Events"]).Events;
        Data.Results = EventHandler.PopulateResults(Data.Events);
    }
}

public class Data
{
    public static List<Client> Clients;
    public static List<objectDef> Objects;
    public static List<Trait> Traits;
    public static List<Event> Events;
    public static Dictionary<string, EventHandler.EventResult> Results;
}

public class EventHandler
{
    public Dictionary<string, EventResult> Results;
    public delegate void EventResult();

    public static Dictionary<string, EventResult> PopulateResults(List<Event> Events)
    {
        Dictionary<string, EventResult> results = new Dictionary<string, EventResult>();

        //        1 - Wizard offers to buy the just - purchased item from us, 
        // for $300.If we accept, we lose the item, and that type becomes available again for future choices. 
        // (But we do NOT get an extra turn!)

        results.Add("buyback", () => 
        {
            var lastItem = PlayerInfoDisplay.player.chosenItems[PlayerInfoDisplay.player.chosenItems.Count - 1];
            PlayerInfoDisplay.player.chosenItems.Remove(lastItem);
        });

        //2 - Wizard offers to transmute the just - purchased item for $100.If we accept, 
        // we lose the item and replace it with a random non - identical item of the same type.
        results.Add("transmute", () => 
        {
            var lastItem = PlayerInfoDisplay.player.chosenItems[PlayerInfoDisplay.player.chosenItems.Count - 1];
            PlayerInfoDisplay.player.chosenItems.Remove(lastItem);

            List<objectDef> possibleItems = new List<objectDef>();
            foreach (var item in Data.Objects)
            {
                if (item.type == lastItem.type && item != lastItem) possibleItems.Add(item);
            }

            var rng = Random.Range(0, possibleItems.Count);
            PlayerInfoDisplay.player.chosenItems.Add(possibleItems[rng]);
        });


        //  3 - Wizard offers to “improve” our client’s personality.Two random non - identical options are presented
        // from the interests list.The selected one becomes additional scoring criteria.

        //results.Add(Events[0].name, () =>
        //{
        //    var studiousTrait = Traits.Where(x => x.name == "studious");
        //    var agressiveTrait = Traits.Where(x => x.name == "aggressive");
        //});

        //  4 - Wizard asks us what his favorite number is.Generate 2 random numbers for the answer 
        // -both are wrong.Lose $200 as a penalty.

        results.Add("number", () =>
        {
            // do nothing, the budget is updated in game manager
        });

        // 5 - Wizard asks us how our amnesia is doing.Options are “What amnesia ?” and “Fine, thanks” 
        // wizard then removes ALL types from our “already picked” list, so we can start getting repeat item types.
        results.Add("amnesia", () =>
        {
            // special case, handled in gm
        });

        // 6 - Wizard is collecting money for the wizard’s ball.Can offer $100 or $300.If we give $300, we get an extra turn in the game.
        results.Add("donation", () =>
        {
            PlayerInfoDisplay.player.budget -= 200;
        });

        // 7 - Wizard asks us if we’re already selected a theme for our client, and offers to help us expand on it.Options are “no thanks” and “yes please” (which costs $200).This does nothing.

        results.Add("retheme", () =>
        {
            // do nothing
        });

        return results;
    }
}
