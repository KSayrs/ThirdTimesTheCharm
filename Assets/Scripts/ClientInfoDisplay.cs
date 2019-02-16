using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientInfoDisplay : MonoBehaviour
{
    [SerializeField] Text Name = null;
    [SerializeField] Text Description = null;
    public Image Pic = null;
    [SerializeField] Text Budget = null;
    [SerializeField] ClientInfoPopup popup = null;

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
        popup.SetClientInfo();
    }

    public void UpdateBudget(int amount)
    {
        PlayerInfoDisplay.player.budget += amount;
        Budget.text = PlayerInfoDisplay.player.budget.ToString();
    }

    public void UpdateClientImage(string path)
    {
        Pic.sprite = Resources.Load<Sprite>(path);
        popup.SetClientPic(path);
    }
}
