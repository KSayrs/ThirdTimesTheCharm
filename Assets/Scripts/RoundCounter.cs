using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundCounter : MonoBehaviour
{
    [SerializeField] List<Image> Rounds;
    [SerializeField] Color updateColor;

    public void UpdateRound(int turn)
    {
        for (int i = 0; i < turn; i++)
        {
            if (i > Rounds.Count - 1) return;

            Rounds[i].color = updateColor;
        }
    }
}
