using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseManager : MonoBehaviour
{
    [SerializeField] GameObject ItemBox = null;
    [SerializeField] float ItemPadding = 8f;
    List<GameObject> ExistingItems = new List<GameObject>();
    [SerializeField] ScrollRect scrollRect = null;

    private void OnEnable()
    {
        SetHeight();
        scrollRect.verticalNormalizedPosition = 1f;

        for (int i = 0; i < PlayerInfoDisplay.player.chosenItems.Count; i++)
        {
            GameObject Box;
            if (i == 0 && ExistingItems.Count == 0) ExistingItems.Add(ItemBox);

            if (i >= ExistingItems.Count)
            {
                Box = Instantiate(ItemBox, transform);
                var PrevRect = ExistingItems[i - 1].GetComponent<RectTransform>();
                var BoxRect = Box.GetComponent<RectTransform>();
                BoxRect.localPosition = new Vector3(BoxRect.localPosition.x, PrevRect.localPosition.y - (128f + ItemPadding), BoxRect.localPosition.z);
                ExistingItems.Add(Box);
            }
            else Box = ExistingItems[i];

            PopulateItemBox(Box, PlayerInfoDisplay.player.chosenItems[i]);

            if(ExistingItems.Count > 0) ItemBox.SetActive(true);
        }
    }

    //------------------------------------------------------------

    void SetHeight()
    {
        var height = (128f + ItemPadding) * (PlayerInfoDisplay.player.chosenItems.Count);
        var rT = GetComponent<RectTransform>();
        if (height > rT.sizeDelta.y) rT.sizeDelta = new Vector2(rT.sizeDelta.x, height);
    }

    //------------------------------------------------------------

    void PopulateItemBox(GameObject Box, objectDef item)
    {
        var itemBox = Box.GetComponent<ItemBox>();
        itemBox.ItemImage.sprite = Resources.Load<Sprite>(item.image);
        itemBox.ItemName.text = item.name;

        PopulateTraitBox(itemBox.ItemTraits.Traits, item);
    }

    //------------------------------------------------------------

    // todo refactor -- this is also used in GameManager
    void PopulateTraitBox(List<Image> TraitIcons, objectDef item)
    {
        for (int i = 0; i < TraitIcons.Count; i++)
        {
            if (i < item.traits.Count)
            {
                foreach (var trait in Data.Traits)
                {
                    if (trait.name == item.traits[i])
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
