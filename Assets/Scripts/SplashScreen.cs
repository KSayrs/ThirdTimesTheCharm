using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
    public float lifeTime = 2.0f;
    public Button b_splashButton;

    // Start is called before the first frame update
    void Start()
    {
        b_splashButton.onClick.AddListener(SplashButton);
    }

    void Awake()
    {
        Destroy(gameObject, lifeTime);
    }

    void SplashButton()
    {
        Destroy(gameObject);
    }
}
