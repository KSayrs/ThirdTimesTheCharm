using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region fields
    [SerializeField] ClientInfoDisplay clientAndBudget = null;

    private bool firstClient;
    private bool pigYear;
    [HideInInspector] public int turn;
    public int maxTurns;
    private int[] currentButtons;
    [HideInInspector] public int[] choices;
    public bool[] eventTurns;
    private int rollOne, rollTwo;

    [Header("Buttons")]
    public RoundCounter roundCounter;
    public Button b_FirstButton, b_SecondButton, b_ThirdButton;
    public Text t_FirstButton, t_SecondButton, t_ThirdButton, baseItemScore1, baseItemScore2, baseItemScore3;
    public Text ItemOneCost, ItemTwoCost, ItemThreeCost;

    [Header("Confirm Panel")]
    public Image confirmImage;
    public GameObject confirmPanel, confirmValueObject;
    public Button b_ConfirmButton, b_CancelButton;
    public Text confirmDescription, confirmMoney, confirmButtonText, confirmValue;
    private int confirmButton;

    [Header("Event Panel")]
    public Image eventImage;
    public GameObject eventPanel;
    public Button b_EventButtonOne, b_EventButtonTwo;
    public Text eventDescription, eventMoney;
    private int eventButton;

    [Header("Event Confirm Panel")]
    public Image eventConfirmImage;
    public GameObject eventConfirmPanel;
    public GameObject eventConfirmMoneyPanel;
    public Button b_EventConfirmButton;
    public Text eventConfirmDescription, eventConfirmMoney;

    private int eventConfirmButton;
    private int eventPercentChance = 25;
    private Event currentEvent;

    private objectDef buttonOneObj;
    private objectDef buttonTwoObj;
    private objectDef buttonThreeObj;

    private Client buttonOneClient;
    private Client buttonTwoClient;
    private Client buttonThreeClient;

    private Dictionary<string, List<objectDef>> categoryDict = new Dictionary<string, List<objectDef>>();
    private List<string> permittedCategorites = new List<string>();

#endregion

    private void Awake()
    {
        // assign categories to dict
        foreach (var item in Data.Objects)
        {
            if (!categoryDict.ContainsKey(item.type))
            {
                categoryDict.Add(item.type, new List<objectDef> { item });
                permittedCategorites.Add(item.type);
            }
            else categoryDict[item.type].Add(item);
        }

        maxTurns = categoryDict.Count-2;
        Debug.Log(maxTurns);
        // Initialize variables
        firstClient = true;
        pigYear = true;
        rollOne = -1;
        rollTwo = -1;
        turn = 0;
        currentButtons = new int[3];
        choices = new int[maxTurns+1];

        // Initializes Button Listeners
        b_FirstButton.onClick.AddListener(OptionOne);
        b_SecondButton.onClick.AddListener(OptionTwo);
        b_ThirdButton.onClick.AddListener(OptionThree);
        b_ConfirmButton.onClick.AddListener(ConfirmButton);
        b_CancelButton.onClick.AddListener(CancelButton);
        b_EventButtonOne.onClick.AddListener(EventButtonOne);
        b_EventButtonTwo.onClick.AddListener(EventButtonTwo);
        b_EventConfirmButton.onClick.AddListener(EventConfirmButton);

        // Initialize roundCounter
        roundCounter.UpdateRound(0);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // Initialize First Buttons
        NewClients(0);
        NewClients(1);
        NewClients(2);
    }

    // Clickable buttons
    void OptionOne()    {
        b_FirstButton.GetComponent<AudioSource>().Play();
        LoadConfirmation(0);
    }

    void OptionTwo()    {
        b_SecondButton.GetComponent<AudioSource>().Play();
        LoadConfirmation(1);
    }

    void OptionThree()  {
        b_ThirdButton.GetComponent<AudioSource>().Play();
        LoadConfirmation(2);
    }

    // Next Turn
    void NextTurn(int button)
    {   
        if (turn >= maxTurns)
        {
            EnterScene("Score");
            return;
        }

        // pick a client
        Debug.Log("Clicked button " + button);
        if (firstClient == true)
        {
            if (button == 0)
            {
                PlayerInfoDisplay.player.client = buttonOneClient;
                clientAndBudget.UpdateClientAndBudget(int.Parse(ItemOneCost.text));
            }
            else if (button == 1)
            {
                PlayerInfoDisplay.player.client = buttonTwoClient;
                clientAndBudget.UpdateClientAndBudget(int.Parse(ItemTwoCost.text));
            }
            else
            {
                PlayerInfoDisplay.player.client = buttonThreeClient;
                clientAndBudget.UpdateClientAndBudget(int.Parse(ItemThreeCost.text));
            }
            
            Debug.Log("Chose client " + PlayerInfoDisplay.player.client.name);
        }
        else
        {   // pick an item
            if (button == 0)
            {
                PlayerInfoDisplay.player.chosenItems.Add(buttonOneObj);
                clientAndBudget.UpdateBudget(-int.Parse(ItemOneCost.text));
            }
            else if (button == 1)
            {
                PlayerInfoDisplay.player.chosenItems.Add(buttonTwoObj);
                clientAndBudget.UpdateBudget(-int.Parse(ItemTwoCost.text));
            }
            else
            {
                PlayerInfoDisplay.player.chosenItems.Add(buttonThreeObj);
                clientAndBudget.UpdateBudget(-int.Parse(ItemThreeCost.text));
            }
            var p = PlayerInfoDisplay.player;

            // is the client happy?
            if (p.LikesItem(p.chosenItems[p.chosenItems.Count - 1]))
            {
                clientAndBudget.UpdateClientImage(p.client.image + "_happy"); // shhh I didn't want to update the data model
            }
            else
            {
                clientAndBudget.UpdateClientImage(p.client.image);
            }
        }
        // Spawn event
        if (turn > 0)
        {
            var eventRoll = Random.Range(0, 100);
            if (eventRoll > eventPercentChance)
            {
                eventPercentChance += 25;
            }
            else
            {
                LoadEvent();
                eventPercentChance = 25;
            }

            roundCounter.UpdateRound(turn + 1);
        }

        if (firstClient)
        {
            firstClient = false;
        }
        if (!firstClient && turn == 0)
        {
            roundCounter.UpdateRound(1);
            turn += 1;
        }
        else
        {
            turn += 1;
        }

        // Store the current selection
        StoreChoices(button);

        // pick object list for a new category
        int rn = Random.Range(0, permittedCategorites.Count);
        var cat = permittedCategorites[rn];
        var objectsInCategory = categoryDict[cat];
        permittedCategorites.Remove(cat);
        Debug.Log("Removed Category: " + cat);

        // Reroll new buttons
        for (int b = 0; b < 3; b++)
        {
            ChangeButton(b, objectsInCategory);
        }
    }
     
    // Initiates button change
    void ChangeButton(int button, List<objectDef> objectsincat)
    {
        // Generate button object
        int rng = RandomNumberGenerator();

        // Store current object number
        StoreButton(button, rng);

        // Change button data
        ChangeButtonTextAndImage(button, rng, objectsincat);
    }

    // Changes Button Text and Image
    void ChangeButtonTextAndImage(int button, int rng, List<objectDef> objectsincat)
    {
        var rn = Random.Range(0, categoryDict.Count-1);
        objectDef currentObject = null;
        if (objectsincat.Count > 1) currentObject = objectsincat[Random.Range(0, objectsincat.Count - 1)];
        else currentObject = objectsincat[0];
        Debug.LogWarning(objectsincat.Count);
        var TraitIcons = new List<Image>();

        switch (button)
        {
            case 0:
                t_FirstButton.text = currentObject.name;
                b_FirstButton.GetComponent<Image>().sprite = Resources.Load<Sprite>(currentObject.image);
                b_FirstButton.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>(currentObject.sound);
                buttonOneObj = currentObject;
                objectsincat.Remove(currentObject);
                ItemOneCost.text = currentObject.cost.ToString();
                baseItemScore1.text = currentObject.value.ToString();
                TraitIcons = b_FirstButton.GetComponent<TraitHolder>().Traits;
                SetUpTraitIcons(TraitIcons, currentObject);
                break;
            case 1:
                t_SecondButton.text = currentObject.name;
                b_SecondButton.GetComponent<Image>().sprite = Resources.Load<Sprite>(currentObject.image);
                b_SecondButton.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>(currentObject.sound);
                buttonTwoObj = currentObject;
                objectsincat.Remove(currentObject);
                ItemTwoCost.text = currentObject.cost.ToString();
                baseItemScore2.text = currentObject.value.ToString();
                TraitIcons = b_SecondButton.GetComponent<TraitHolder>().Traits;
                SetUpTraitIcons(TraitIcons, currentObject);
                break;
            case 2:
                t_ThirdButton.text = currentObject.name;
                b_ThirdButton.GetComponent<Image>().sprite = Resources.Load<Sprite>(currentObject.image);
                b_ThirdButton.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>(currentObject.sound);
                buttonThreeObj = currentObject;
                objectsincat.Remove(currentObject);
                ItemThreeCost.text = currentObject.cost.ToString();
                baseItemScore3.text = currentObject.value.ToString();
                TraitIcons = b_ThirdButton.GetComponent<TraitHolder>().Traits;
                SetUpTraitIcons(TraitIcons, currentObject);
                break;
        }
    }

    // Store Choices
    void StoreChoices(int button)
    {
        int oldButton = currentButtons[button];
        choices[turn] = oldButton;
    }

    // Store Button
    void StoreButton(int button, int rng)
    {
        currentButtons[button] = rng;
    }

    // Random Number Generator
    int RandomNumberGenerator()
    {
        var currentObject = Data.Objects.Count;
        int rng = Random.Range(0, currentObject);
        return rng;
    }

    // New Client
    void NewClients(int button)
    {
        var objectCount = Data.Clients.Count;
        int rng = Random.Range(0, objectCount);

        while (rng == rollOne || rng == rollTwo) { rng = Random.Range(0, objectCount); }

        var currentObject = Data.Clients[rng];

        if (rollOne > 0 && rollTwo < 0) { rollTwo = rng;}
        if (rollOne < 0) { rollOne = rng;}

        // reset first object for Year of the Pig
        if (pigYear)
        {
            currentObject = Data.Clients[4];
            pigYear = false;
            rollOne = 4;
        }

        switch (button)
        {
            case 0:
                t_FirstButton.text = currentObject.name;
                b_FirstButton.GetComponent<Image>().sprite = Resources.Load<Sprite>(currentObject.image);
                b_FirstButton.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>(currentObject.sound);
                buttonOneClient = currentObject;
                ItemOneCost.text = GenerateRandomBudget().ToString();
                break;
            case 1:
                t_SecondButton.text = currentObject.name;
                b_SecondButton.GetComponent<Image>().sprite = Resources.Load<Sprite>(currentObject.image);
                b_SecondButton.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>(currentObject.sound);
                buttonTwoClient = currentObject;
			    ItemTwoCost.text = GenerateRandomBudget().ToString();
                break;
            case 2:
                t_ThirdButton.text = currentObject.name;
                b_ThirdButton.GetComponent<Image>().sprite = Resources.Load<Sprite>(currentObject.image);
                b_ThirdButton.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>(currentObject.sound);
                buttonThreeClient = currentObject;
			    ItemThreeCost.text = GenerateRandomBudget().ToString();
                break;
        }
    }

    // Confirm Panel Stuff -----------------------------

    // Show confirmation panel
    void LoadConfirmation(int button)
    {
        UpdateConfirmImage(button);
        confirmButton = button;
        confirmPanel.SetActive(true);
    }

    // Confirm button
    public void ConfirmButton()
    {
        NextTurn(confirmButton);
        confirmPanel.SetActive(false);
        confirmButton = -1;
    }

    // Cancel button
    void CancelButton()
    {
        confirmPanel.SetActive(false);
        confirmButton = -1;
    }

    // Update the confirmation image
    private void UpdateConfirmImage(int button)
    {
        if (firstClient == true)
        {
            if (button == 0)
            {
                confirmImage.GetComponent<Image>().sprite = Resources.Load<Sprite>(buttonOneClient.image);
                confirmDescription.text = buttonOneClient.description;
                confirmMoney.text = ItemOneCost.text;
                confirmButtonText.text = "Choose Client";
            }
            else if (button == 1)
            {
                confirmImage.GetComponent<Image>().sprite = Resources.Load<Sprite>(buttonTwoClient.image);
                confirmDescription.text = buttonTwoClient.description;
                confirmMoney.text = ItemTwoCost.text;
                confirmButtonText.text = "Choose Client";
            }
            else
            {
                confirmImage.GetComponent<Image>().sprite = Resources.Load<Sprite>(buttonThreeClient.image);
                confirmDescription.text = buttonThreeClient.description;
                confirmMoney.text = ItemThreeCost.text;
                confirmButtonText.text = "Choose Client";
            }
        }
        else
        {
            var TraitIcons = confirmPanel.GetComponent<TraitHolder>().Traits;
            confirmValueObject.SetActive(true);
            if (button == 0)
            {
                confirmImage.GetComponent<Image>().sprite = Resources.Load<Sprite>(buttonOneObj.image);
                confirmDescription.text = buttonOneObj.description;
                confirmMoney.text = ItemOneCost.text;
                confirmValue.text = buttonOneObj.value.ToString();
                confirmButtonText.text = "Confirm Purchase";
                SetUpTraitIcons(TraitIcons, buttonOneObj);
            }
            else if (button == 1)
            {
                confirmImage.GetComponent<Image>().sprite = Resources.Load<Sprite>(buttonTwoObj.image);
                confirmDescription.text = buttonTwoObj.description;
                confirmMoney.text = ItemTwoCost.text;
                confirmValue.text = buttonTwoObj.value.ToString();
                confirmButtonText.text = "Confirm Purchase";
                SetUpTraitIcons(TraitIcons, buttonTwoObj);
            }
            else
            {
                confirmImage.GetComponent<Image>().sprite = Resources.Load<Sprite>(buttonThreeObj.image);
                confirmDescription.text = buttonThreeObj.description;
                confirmMoney.text = ItemThreeCost.text;
                confirmValue.text = buttonThreeObj.value.ToString();
                confirmButtonText.text = "Confirm Purchase";
                SetUpTraitIcons(TraitIcons, buttonThreeObj);
            }
        }
    }

    // Event Panel Stuff --------------------------------

    // Show Event panel
    void LoadEvent()
    {
        var rn = Random.Range(0, Data.Events.Count);
        currentEvent = Data.Events[rn];
        eventDescription.text = currentEvent.description;
        b_EventButtonOne.GetComponentInChildren<Text>().text = currentEvent.button1;
        b_EventButtonTwo.GetComponentInChildren<Text>().text = currentEvent.button2;
        eventPanel.SetActive(true);
    }

    // First Event Option
    public void EventButtonOne()
    {
        if (!currentEvent.buttonMatters)
        {
            eventConfirmMoneyPanel.SetActive(true);
            Data.Results[currentEvent.name]();
            clientAndBudget.UpdateBudget(-currentEvent.cost);
        }

        if (currentEvent.name == "donation")
        {
            clientAndBudget.UpdateBudget(-currentEvent.cost);
        }
        
        eventConfirmMoney.text = currentEvent.cost.ToString();
        eventPanel.SetActive(false);
        eventConfirmPanel.SetActive(true);
    }

    // Second Event Option
    public void EventButtonTwo()
    {
        // special conditions can't handle in the data initializer
        if (currentEvent.name == "buyback")
        {
            var lastItem = PlayerInfoDisplay.player.chosenItems[PlayerInfoDisplay.player.chosenItems.Count - 1];
            permittedCategorites.Add(lastItem.type);
            clientAndBudget.UpdateBudget(currentEvent.cost);
            clientAndBudget.UpdateBudget(currentEvent.cost);
        }
        if (currentEvent.name == "amnesia")
        {
            var newlist = new List<string>();
            foreach(var key in categoryDict.Keys) { newlist.Add(key); }
            permittedCategorites = newlist;
        }
        if (currentEvent.name == "donation")
        {
            maxTurns++;
            var newchoices = new int[maxTurns+1];
            for (int i = 0; i < choices.Length; i++)
            {
                newchoices[i] = choices[i];
            }
            choices = newchoices;
        }

        // function
        Data.Results[currentEvent.name]();
        clientAndBudget.UpdateBudget(-currentEvent.cost);
        eventConfirmMoneyPanel.SetActive(true);
        eventConfirmMoney.text = currentEvent.cost.ToString();

        if (currentEvent.name == "donation")
        {
            clientAndBudget.UpdateBudget(-currentEvent.cost);
            eventConfirmMoney.text = "400";
        }

        eventPanel.SetActive(false);
        eventConfirmPanel.SetActive(true);
    }

    // Event Confirm button
    private void EventConfirmButton()
    {
        eventConfirmMoneyPanel.SetActive(false);
        eventConfirmPanel.SetActive(false);
    }

    //-------------------------------------------------
    // call this from any script
    public static void EnterScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    int GenerateRandomBudget()
    {
        return 1000; // shrug emoji
    }

    void SetUpTraitIcons(List<Image> TraitIcons, objectDef currentObject)
    {
        for (int i = 0; i < TraitIcons.Count; i++)
        {
            if (i < currentObject.traits.Count)
            {
                foreach (var trait in Data.Traits)
                {
                    if (trait.name == currentObject.traits[i])
                    {
                        TraitIcons[i].sprite = Resources.Load<Sprite>(trait.icon);
                        TraitIcons[i].gameObject.SetActive(true);
                    }
                }
            }
            else TraitIcons[i].gameObject.SetActive(false);
        }
    }
}
