using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    List<TabSingle> tabButtons;
    Color32 def = new Color32(255, 255, 255, 10);
    public TabSingle selectedTab;
    public List<GameObject> pagesToSwap;
    // Start is called before the first frame update
    void Start()
    {
        TabReset();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Subscribe(TabSingle tabButton){
        if (tabButtons == null){
            tabButtons = new List<TabSingle>();
        }
        tabButtons.Add(tabButton);
    }

    public void OnTabEnter(TabSingle tabButton){
        TabReset();
        if (selectedTab == null || tabButton != selectedTab){
            tabButton.background.color = new Color32(def.r, def.g, def.b, 122);
        }
    }

    public void OnTabExit(TabSingle tabButton){
        TabReset();
    }

    public void OnTabSelect(TabSingle tabButton){
        selectedTab = tabButton;
        TabReset();
        tabButton.background.color = new Color32(81, 239, 248, 255);
        int index = tabButton.transform.GetSiblingIndex();
        for(int i=0; i<pagesToSwap.Count; i++){
            if(i == index){pagesToSwap[i].SetActive(true);}
            else{pagesToSwap[i].SetActive(false);}
        }
    }

    public void TabReset(){
        foreach(TabSingle tabButton in tabButtons){
            if (selectedTab != null && tabButton == selectedTab){
                tabButton.background.color = new Color32(81, 239, 248, 255);
                continue;
            }
            tabButton.background.color = def;
        }
    }
}
