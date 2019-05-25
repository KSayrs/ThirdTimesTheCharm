using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartButtonSelector : MonoBehaviour
{
    public EventSystem eventSystem;
    public Button startButton;

    // Start is called before the first frame update
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(startButton.gameObject);
    }
}
