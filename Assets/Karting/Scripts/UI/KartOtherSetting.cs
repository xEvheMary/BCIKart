using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KartOtherSetting : MonoBehaviour
{
    //public static KartOtherSetting Instance {get; private set;}

    [SerializeField] private TMP_InputField baseDelayInputField;

    private const string PLAYER_PREFS_BASELINE_DELAY = "baselineDelay";
    // Start is called before the first frame update
    
    private void Awake()
    {
        //Instance = this;
    } 
    
    void Start()
    {
        UpdateVisual();  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateVisual(){
        if (PlayerPrefs.HasKey(PLAYER_PREFS_BASELINE_DELAY)){baseDelayInputField.text = PlayerPrefs.GetInt(PLAYER_PREFS_BASELINE_DELAY, 4).ToString();}
    }

    public void SaveChanges(){
        
        PlayerPrefs.SetInt(PLAYER_PREFS_BASELINE_DELAY, int.Parse(baseDelayInputField.text));
        PlayerPrefs.Save();
        UpdateVisual();
    }
}
