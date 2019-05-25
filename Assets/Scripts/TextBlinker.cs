using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Text))]
public class TextBlinker : MonoBehaviour
{
    [SerializeField] float BlinkRate = 0.25f;

    Text text;
    string storedText = "";

    void Start()
    {
        text = GetComponent<Text>();
        storedText = text.text;
        text.text = "";
    }

    void OnEnable()
    {
        StartCoroutine(BlinkText());
    }

    IEnumerator BlinkText()
    {
        yield return new WaitForEndOfFrame();
        text.text = "";
        while (text != null)
        {
            foreach (var letter in storedText)
            {
                text.text += letter;
                yield return new WaitForSeconds(BlinkRate);
            }

            text.text = "";
            yield return new WaitForSeconds(BlinkRate);
        }
    }
}
