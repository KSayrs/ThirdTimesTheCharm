using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundCounter : MonoBehaviour
{
    [SerializeField] List<Image> Rounds = null;
    [SerializeField] Color updateColor = Color.green;

    public void UpdateRound(int turn)
    {
        for (int i = 0; i < turn; i++)
        {
            if (i > Rounds.Count - 1) return;

            if (!Rounds[i].gameObject.activeInHierarchy) Rounds[i].gameObject.SetActive(true);
            Rounds[i].color = updateColor;

        }
    }
}
