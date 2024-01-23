using System;
using System.Collections;
using System.Collections.Generic;
using KartGame.KartSystems;
using UnityEngine;
using UnityEngine.UI;

public class BarFeedbackUI : MonoBehaviour
{
    [SerializeField] private Image leftImage;
    [SerializeField] private Image rightImage;

    // Start is called before the first frame update
    void Start()
    {
        if(gameObject.activeSelf){Controller.OnSignal += Controller_OnSignal;}
        //Controller.OnSignal += Controller_OnSignal;
        leftImage.fillAmount = 0f;
        rightImage.fillAmount = 0f;
    }

    private void Controller_OnSignal(object sender, Controller.OnSignalArgs e)
    {
        float x = e.signalValue;
        if (x > 0){
            leftImage.fillAmount = 0f;
            rightImage.fillAmount = x;
        }
        else if (x < 0){
            leftImage.fillAmount = Mathf.Abs(x);
            rightImage.fillAmount = 0f;
        }
        else{
            leftImage.fillAmount = 0f;
            rightImage.fillAmount = 0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable(){
        Controller.OnSignal += Controller_OnSignal;
        leftImage.fillAmount = 0f;
        rightImage.fillAmount = 0f;
    }

    private void OnDisable(){
        Controller.OnSignal -= Controller_OnSignal;
        leftImage.fillAmount = 0f;
        rightImage.fillAmount = 0f;
    }
}
