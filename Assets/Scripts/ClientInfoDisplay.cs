using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientInfoDisplay : MonoBehaviour
{
    [SerializeField] Text Name;
    [SerializeField] Text Description;
    [SerializeField] Image Pic;
    [SerializeField] Text Budget;

    void Start()
    {
        if (PlayerInfoDisplay.player.client == null)
        {
            Description.text = "Choose a client.";
            Name.text = "";
            Budget.text = "0";
        }
    }

    public void UpdateClientAndBudget(int budget)
    {
        Description.text = PlayerInfoDisplay.player.client.description;
        Name.text = PlayerInfoDisplay.player.client.name;
        Pic.sprite = Resources.Load<Sprite>(PlayerInfoDisplay.player.client.image);
        PlayerInfoDisplay.player.budget = budget;
        Budget.text = budget.ToString();
    }

    public void UpdateBudget(int amount)
    {
        PlayerInfoDisplay.player.budget += amount;
        Budget.text = PlayerInfoDisplay.player.budget.ToString();
    }
}
