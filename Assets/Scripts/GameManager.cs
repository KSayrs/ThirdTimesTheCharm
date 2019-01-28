using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] ClientInfoDisplay clientAndBudget;

    private bool firstClient;
    public int turn;
    private int[] currentButtons;
    public int[] choices;

    public Button b_FirstButton, b_SecondButton, b_ThirdButton, b_ConfirmButton, b_CancelButton;
    public Text t_FirstButton, t_SecondButton, t_ThirdButton, confirmDescription, confirmMoney, baseItemScore1, baseItemScore2, baseItemScore3;
    public Image confirmImage;
    public GameObject confirmPanel;
    private int confirmButton;

    public Text ItemOneCost, ItemTwoCost, ItemThreeCost;

    public RoundCounter roundCounter;

    private objectDef buttonOneObj;
    private objectDef buttonTwoObj;
    private objectDef buttonThreeObj;

    private Client buttonOneClient;
    private Client buttonTwoClient;
    private Client buttonThreeClient;

    private List<objectDef> EntireObjectList;
    private List<Trait> AllTraits;
    private Dictionary<string, List<objectDef>> categoryDict = new Dictionary<string, List<objectDef>>();

    // Start is called before the first frame update
    void Awake()
    {
        // Initialize variables
        firstClient = true;
        turn = 0;
        currentButtons = new int[3];
        choices = new int[5];

        // Initializes Button Listeners
        b_FirstButton.onClick.AddListener(OptionOne);
        b_SecondButton.onClick.AddListener(OptionTwo);
        b_ThirdButton.onClick.AddListener(OptionThree);
        b_ConfirmButton.onClick.AddListener(ConfirmButton);
        b_CancelButton.onClick.AddListener(CancelButton);

        // Initialize First Buttons
        NewClients(0);
        NewClients(1);
        NewClients(2);

        // init roundCounter
        roundCounter.UpdateRound(0);

        //get all objects
        EntireObjectList = ObjectLists.GetFromJson("Objects.json").Objects;

        // assign categories to dict
        foreach(var item in EntireObjectList)
        {
            if (!categoryDict.ContainsKey(item.type)) categoryDict.Add(item.type, new List<objectDef>{ item });
            else categoryDict[item.type].Add(item);
        }
        Debug.Log(categoryDict);

        // all traits
        AllTraits = TraitLists.GetFromJson("Traits.json").Traits;
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
            firstClient = false;
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
            Debug.Log("Chose an item");
        }

        // Store the current selection
        StoreChoices(button);

        // pick object list for a new category
        int rn = Random.Range(0, categoryDict.Count - 1);
        var values = Enumerable.ToList(categoryDict.Values);
        var objectsInCategory = values[rn];

        // Reroll new buttons
        for (int b = 0; b < 3; b++)
        {
            ChangeButton(b, objectsInCategory);
        }

        // Increment turns
        turn += 1;
        if (turn >= 5)
        {
            EnterScene("Score");
        }
        roundCounter.UpdateRound(turn+1);
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
        var rn = Random.Range(0, categoryDict.Count - 1);
        var currentObject = objectsincat[Random.Range(0, objectsincat.Count - 1)];
        Debug.Log(currentObject.image);
        var TraitIcons = new List<Image>();

        switch (button)
        {
            case 0:
                t_FirstButton.text = currentObject.name;
                b_FirstButton.GetComponent<Image>().sprite = Resources.Load<Sprite>(currentObject.image);
                b_FirstButton.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>(currentObject.sound);
                buttonOneObj = currentObject;
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
        var currentObject = ObjectLists.GetFromJson("Objects.json").Objects.Count;
        int rng = Random.Range(0, currentObject);
        return rng;
    }

    // New Client
    void NewClients(int button)
    {
        var objectCount = ClientLists.GetFromJson("Clients.json").Clients.Count;
        int rng = Random.Range(0, objectCount);

        var currentObject = ClientLists.GetFromJson("Clients.json").Clients[rng];

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
            }
            else if (button == 1)
            {
                confirmImage.GetComponent<Image>().sprite = Resources.Load<Sprite>(buttonTwoClient.image);
                confirmDescription.text = buttonTwoClient.description;
                confirmMoney.text = ItemTwoCost.text;
            }
            else
            {
                confirmImage.GetComponent<Image>().sprite = Resources.Load<Sprite>(buttonThreeClient.image);
                confirmDescription.text = buttonThreeClient.description;
                confirmMoney.text = ItemThreeCost.text;
            }
        }
        else
        {
            if (button == 0)
            {
                confirmImage.GetComponent<Image>().sprite = Resources.Load<Sprite>(buttonOneObj.image);
                confirmDescription.text = buttonOneObj.description;
                confirmMoney.text = ItemOneCost.text;
            }
            else if (button == 1)
            {
                confirmImage.GetComponent<Image>().sprite = Resources.Load<Sprite>(buttonTwoObj.image);
                confirmDescription.text = buttonTwoObj.description;
                confirmMoney.text = ItemTwoCost.text;
            }
            else
            {
                confirmImage.GetComponent<Image>().sprite = Resources.Load<Sprite>(buttonThreeObj.image);
                confirmDescription.text = buttonThreeObj.description;
                confirmMoney.text = ItemThreeCost.text;
            }
        }
    }
    //-------------------------------------------------
    // call this from any script
    public static void EnterScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    int GenerateRandomItemCost()
    {
        return Random.Range(5, 70)*10; // shrug emoji
    }

    int GenerateRandomBudget()
    {
        return Random.Range(80, 160) * 10; // shrug emoji
    }

    void SetUpTraitIcons(List<Image> TraitIcons, objectDef currentObject)
    {
        for (int i = 0; i < TraitIcons.Count; i++)
        {
            if (i < currentObject.traits.Count)
            {
                foreach (var trait in AllTraits)
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