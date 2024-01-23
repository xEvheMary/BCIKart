using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KartSetting : MonoBehaviour
{
    [SerializeField] private TMP_InputField kartSpeedInputField;
    [SerializeField] private TMP_InputField kartAccelInputField;
    private const string PLAYER_PREFS_KART_SPEED = "PPKartSpeed";
    private const string PLAYER_PREFS_KART_ACCEL = "PPKartAccel";
    // Start is called before the first frame update
    
    private void Awake()
    {
        
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
        if (PlayerPrefs.HasKey(PLAYER_PREFS_KART_SPEED)){kartSpeedInputField.text = PlayerPrefs.GetInt(PLAYER_PREFS_KART_SPEED).ToString();}
        if (PlayerPrefs.HasKey(PLAYER_PREFS_KART_ACCEL)){kartAccelInputField.text = PlayerPrefs.GetInt(PLAYER_PREFS_KART_ACCEL).ToString();}
    }

    public void SaveChanges(){
        PlayerPrefs.SetInt(PLAYER_PREFS_KART_SPEED, int.Parse(kartSpeedInputField.text));
        PlayerPrefs.SetInt(PLAYER_PREFS_KART_ACCEL, int.Parse(kartAccelInputField.text));
        PlayerPrefs.Save();
        UpdateVisual();
    }
}
