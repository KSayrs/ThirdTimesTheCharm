using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HideScoreCanvas : MonoBehaviour
{
    public GameObject scoreCanvas;

    // Start is called before the first frame update
    void Start()
    {
        scoreCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Score"))
        {
            // show canvas on activation
            scoreCanvas.SetActive(true);
        }
    }
}
