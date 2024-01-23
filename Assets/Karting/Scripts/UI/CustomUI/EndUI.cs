using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class EndUI : MonoBehaviour
{
    public static EndUI Instance {get; private set;}
    private Action onBackButtonAction;
    //[SerializeField] private Button testModeButton;
    //[SerializeField] private Button calibModeButton;
    [SerializeField] private Button backButton;

    private void Awake(){
        Instance = this;
        
        backButton.onClick.AddListener(() => {
            SceneManager.LoadSceneAsync("MenuScene");
        });
    }

    void Start()
    {
        //Show();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Show(){        
        gameObject.SetActive(true);
    }
    
    public void Hide(){
        gameObject.SetActive(false);
    }
}
