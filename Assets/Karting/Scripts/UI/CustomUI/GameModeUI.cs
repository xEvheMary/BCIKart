using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GameModeUI : MonoBehaviour
{
    public static GameModeUI Instance {get; private set;}
    [SerializeField] private Button testModeButton;
    [SerializeField] private string testScene;
    [SerializeField] private Button calibModeButton;
    [SerializeField] private List<string> calibScenes;
    [SerializeField] private Button backButton;
    private Action onBackButtonAction;
    string calibModeScene;

    private void Awake(){
        Instance = this;
        testModeButton.onClick.AddListener(() => {
            TestSetting.Instance.Save();
            SceneManager.LoadSceneAsync(testScene);
        });

        calibModeButton.onClick.AddListener(() => {
            CalibrationSetting.Instance.Save();
            SceneManager.LoadSceneAsync(calibScenes[0]);

        });
        backButton.onClick.AddListener(() => {
            Hide();
            CalibrationSetting.Instance.Save();
            TestSetting.Instance.Save();
            onBackButtonAction();
        });
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
