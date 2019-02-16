using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // hey DON'T put this script in normal scenes if GameManager is going to use SceneManagement. I'll fix it later to be general use no worries

public class IndependentSceneChangeButton : MonoBehaviour
{
    [SerializeField] string SceneNameToChangeTo = "";
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(EnterScene);
    }

    void EnterScene()
    {
        SceneManager.LoadScene(SceneNameToChangeTo);
    }
}
