using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneChangeButton : MonoBehaviour
{
    [SerializeField] string SceneNameToChangeTo;
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(EnterScene);
    }

    void EnterScene()
    {
        GameManager.EnterScene(SceneNameToChangeTo);
    }
}
