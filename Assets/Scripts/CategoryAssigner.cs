using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Category
{
    addons,
    art,
    bathing,
    bed,
    dwelling,
    instruments,
    landscape,
    recreation
}

[RequireComponent(typeof(Image))]
public class CategoryAssigner : MonoBehaviour
{
    public Category category;
    public bool DontHideSprite;
    public Sprite SwapIfBlank;
    public bool isExceptionSpot = false;

    void Start()
    {
        SetMatches();
    }

    public void SetMatches()
    {
        var image = GetComponent<Image>();
        var match = SearchForMatch();

        if (match != null && isExceptionSpot == match.isException)
        {
            image.sprite = Resources.Load<Sprite>(match.image);
            image.enabled = true;
        }

        else if (!DontHideSprite) image.enabled = false;
        else
        {
            image.sprite = SwapIfBlank;
            image.enabled = true;
        }
    }

    objectDef SearchForMatch()
    {
        foreach (var item in PlayerInfoDisplay.player.chosenItems)
        {
            if (item.type == Enum.GetName(typeof(Category), category))
            {
                Debug.Log("Match found! " + item.name);
                return item;
            }
        }

        return null;
    }
}
