using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClientInfoPopup : MonoBehaviour
{
    [SerializeField] Text Name = null;
    [SerializeField] Image Pic = null;
    [SerializeField] Text Description = null;
    [SerializeField] Text TraitHint = null;

    void OnEnable()
    {
        if (PlayerInfoDisplay.player.client == null)
        {
            gameObject.SetActive(false);
        }
    }

    public void SetClientInfo()
    {
        Description.text = PlayerInfoDisplay.player.client.description;
        Name.text = PlayerInfoDisplay.player.client.name;
        Pic.sprite = Resources.Load<Sprite>(PlayerInfoDisplay.player.client.image);
        TraitHint.text = PlayerInfoDisplay.player.client.trait;
    }

    public void SetClientPic(string path)
    {
        Pic.sprite = Resources.Load<Sprite>(path);
    }
}
