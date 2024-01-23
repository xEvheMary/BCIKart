using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SettingsUI : MonoBehaviour
{
    public static SettingsUI Instance {get; private set;}
    private Action onBackButtonAction;
    //[SerializeField] private Button testModeButton;
    //[SerializeField] private Button calibModeButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button applyButton;

    private void Awake(){
        Instance = this;
        
        backButton.onClick.AddListener(() => {
            Hide();
            onBackButtonAction();
        });

        
        ///applyButton.onClick.AddListener(() => {
        ///    if(LSLSetting.Instance.isActiveAndEnabled){Debug.Log("LSL Active");LSLSetting.Instance.SaveChanges();}
        //    if(GameSetting.Instance.isActiveAndEnabled){Debug.Log("game Active");GameSetting.Instance.ApplyChanges();}
        //    if(KartSetting.Instance.isActiveAndEnabled){Debug.Log("kart Active");KartSetting.Instance.SaveChanges();}
        //    if(KartOtherSetting.Instance.isActiveAndEnabled){Debug.Log("kart o Active");KartOtherSetting.Instance.SaveChanges();}
        //});
    }

    void Start()
    {
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Show(Action onBackButtonAction){
        this.onBackButtonAction = onBackButtonAction;
        
        gameObject.SetActive(true);

        //testModeButton.Select();
    }
    
    public void Hide(){
        gameObject.SetActive(false);
    }
}
