using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class TabSwitcher : MonoBehaviour
{
    [SerializeField] List<TabObject> Tabs = new List<TabObject>();

    [Serializable]
    public class TabObject
    {
        public Text Tab;
        public GameObject TabWindow;
    }

    private TabObject currentTab;

    void Awake()
    {
        if (Tabs.Count > 0) currentTab = Tabs[0];
        else Debug.LogWarning("TABS EMPTY");
    }

    void OnEnable()
    {
        currentTab.TabWindow.SetActive(true);
    }

    void Update()
    {
        if (Input.GetButtonUp("TabRight"))
        {
            if (currentTab == null) currentTab = Tabs[0];
            Debug.Log("Tabbed Right");
            if (Tabs.IndexOf(currentTab) + 1 < Tabs.Count)
            {
                currentTab.TabWindow.SetActive(false);
                var alpha = currentTab.Tab.color;
                alpha.a = 0.5f;
                currentTab.Tab.color = alpha;
                currentTab = Tabs[Tabs.IndexOf(currentTab) + 1];
                currentTab.TabWindow.SetActive(true);
                alpha = currentTab.Tab.color;
                alpha.a = 1f;
                currentTab.Tab.color = alpha;
            }
        }

        if (Input.GetButtonUp("TabLeft"))
        {
            if (currentTab == null) currentTab = Tabs[0];
            Debug.Log("Tabbed Left");
            if (Tabs.IndexOf(currentTab) - 1 >= 0)
            {
                currentTab.TabWindow.SetActive(false);
                var alpha = currentTab.Tab.color;
                alpha.a = 0.5f;
                currentTab.Tab.color = alpha;
                currentTab = Tabs[Tabs.IndexOf(currentTab) - 1];
                currentTab.TabWindow.SetActive(true);
                alpha = currentTab.Tab.color;
                alpha.a = 1f;
                currentTab.Tab.color = alpha;
            }
        }
    }
}
