using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HideOrChangeOnRoundOne : MonoBehaviour
{

    [SerializeField] List<Alternate> Alternates = null;

    private List<string> storedText = new List<string>();

    [Serializable]
    public struct Alternate
    {
        public Text Text;
        public bool Hide;
        public string AlternateText;
    }

    void Start()
    {
        foreach (var alt in Alternates)
        {
            storedText.Add(alt.Text.text);
            if (alt.Hide) { alt.Text.text = ""; }
            else { alt.Text.text = alt.AlternateText; }
        }
    }

    public void Reset()
    {
        for(int i=0; i<storedText.Count; i++)
        {
            Alternates[i].Text.text = storedText[i];
        }
    }
}
