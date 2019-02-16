using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TraitInfoPopupHandler : MonoBehaviour
{
	[SerializeField] GameObject PopupObject = null;
    [SerializeField] GameObject PopupTraitHolder = null;
    [SerializeField] TraitHolder TargetTraitHolder = null;
    [SerializeField] Button button = null;
	
	void Awake()
	{
		button.onClick.AddListener(OnPopup);
	}
	
    void Start()
    {
        PopupObject.SetActive(true);
        PopupObject.SetActive(false);
    }

	void OnPopup()
    {
		PopupObject.SetActive(true);
		
        Debug.Log("TraitInfoPopupHandler: OnPopup() called");
        var popupimages = PopupTraitHolder.GetComponentsInChildren<Image>(true);
        var popupname = PopupTraitHolder.GetComponentsInChildren<Text>(true);
		
        var traitImages = TargetTraitHolder.Traits;
		var visibleTraits = TargetTraitHolder.GetComponentsInChildren<Image>();

        for (int i = 0; i < popupimages.Length; i++)
        {
            if (i >= visibleTraits.Length)
            {
                popupimages[i].gameObject.SetActive(false);
                popupname[i].gameObject.SetActive(false);
            }
            else
            {
                popupimages[i].gameObject.SetActive(true);
                popupname[i].gameObject.SetActive(true);
            }

            foreach (var trait in Data.Traits)
			{
				if (Resources.Load<Sprite>(trait.icon) == traitImages[i].sprite)
				{
					popupimages[i].sprite = Resources.Load<Sprite>(trait.icon);
					popupname[i].text = trait.name;
				}
			}
        }
    }
}
