using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// score is tallied here

public class ScoreTally : MonoBehaviour
{
    #region Serialized Variables

    [SerializeField] Text BaseItemScore;
    [SerializeField] Text MatchBonus;
    [SerializeField] Text BriefFulfillment;
    [SerializeField] Text BudgetBonus;
    [SerializeField] Text TotalScore;
    [SerializeField] Text EndGameText;

    [SerializeField] float appearanceRate;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip winclip;
    [SerializeField] AudioClip loseclip;

    #endregion

    private int totalScore = 0;
    private int matchingTraitBonus = 0;
    private int clientModifier = 0;

    void Start()
    {
        BaseItemScore.text = CalculateBaseItemScore().ToString();
        BudgetBonus.text = CalculateBudgetBonus();
        CalculateMatchBonus();
        MatchBonus.text = matchingTraitBonus.ToString();
        BriefFulfillment.text = clientModifier.ToString();

        TotalScore.text = totalScore.ToString();

        // ----------

        StartCoroutine(AppearAfterSeconds(BaseItemScore, appearanceRate));
        StartCoroutine(AppearAfterSeconds(MatchBonus, appearanceRate * 2));
        StartCoroutine(AppearAfterSeconds(BriefFulfillment, appearanceRate * 3));
        StartCoroutine(AppearAfterSeconds(BudgetBonus, appearanceRate * 4));
        StartCoroutine(TotalScoreAppear(TotalScore, appearanceRate * 5));
    }

    IEnumerator AppearAfterSeconds(Text textObject, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        textObject.gameObject.SetActive(true);
        audioSource.Play();
    }

    IEnumerator TotalScoreAppear(Text textObject, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        textObject.gameObject.SetActive(true);
        EndGameText.gameObject.SetActive(true);
        if (totalScore <= 4)
        {
            EndGameText.text = "Terrible!";
            audioSource.clip = loseclip;
        }
        else if (totalScore <= 9)
        {
            EndGameText.text = "Keep your Head!";
            audioSource.clip = winclip;
        }
        else if (totalScore <= 13)
        {
            EndGameText.text = "Cool!";
            audioSource.clip = winclip;
        }
        else if (totalScore >= 15)
        {
            EndGameText.text = "Excellent!";
            audioSource.clip = winclip;
        }
        else if (totalScore >= 15)
        {
            EndGameText.text = "Fashionista!";
            audioSource.clip = winclip;
        }
        audioSource.Play();
    }

    // ---------------------------------

    string CalculateBudgetBonus()
    {
        var budgetBonus = Math.Ceiling((double)PlayerInfoDisplay.player.budget / 100);
        totalScore += (int) budgetBonus;
        return budgetBonus.ToString();
    }

    int CalculateBaseItemScore()
    {
        int baseitemtotal = 0;
        foreach (var item in PlayerInfoDisplay.player.chosenItems)
        {
            baseitemtotal += item.value;
        }
        totalScore += baseitemtotal;
        return baseitemtotal;
    }

    // simple implementation, just looking for matching traits regardless of theme
    void CalculateMatchBonus() // theme bonus
    {
        Dictionary<string, int> traitCount = new Dictionary<string, int>();

        for (int i=0; i< PlayerInfoDisplay.player.chosenItems.Count; i++)
        {
            Debug.Log(PlayerInfoDisplay.player.chosenItems[i].name);
            var currentTraits = PlayerInfoDisplay.player.chosenItems[i].traits;
            foreach (var trait in currentTraits)
            {
                Debug.Log(trait);
                if (!traitCount.ContainsKey(trait)) traitCount.Add(trait, 0);
                else traitCount[trait]++;

                var clientBonuses = PlayerInfoDisplay.player.client.bonuses;
                Debug.Log("Client: " + PlayerInfoDisplay.player.client.name);
                foreach (var bonus in clientBonuses)
                {
                    if (trait == bonus.trait)
                    {
                        clientModifier += bonus.modifier;
                    }
                }
            }
        }

        foreach (var value in traitCount.Values)
        {
            Debug.Log(value);
            matchingTraitBonus += value;
        }

        totalScore += matchingTraitBonus;
        totalScore += clientModifier;
        //  return matchingTraitBonus.ToString();
    }
}
