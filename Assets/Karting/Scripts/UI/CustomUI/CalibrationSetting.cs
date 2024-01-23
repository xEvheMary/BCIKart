using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CalibrationSetting : MonoBehaviour
{
    public static CalibrationSetting Instance {get; private set;}
    [SerializeField] private TMP_InputField calibrationLapField;
    [SerializeField] private TMP_Dropdown calibrationModeDrop;
    private const string PLAYER_PREFS_CALIBRATION_LAP = "calibrationLap";
    private const string PLAYER_PREFS_CALIBRATION_MODE = "calibrationMode";
    

    public int modeIndex;

    private void Awake(){
        Instance = this;
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
        if (PlayerPrefs.HasKey(PLAYER_PREFS_CALIBRATION_LAP)){calibrationLapField.text = PlayerPrefs.GetInt(PLAYER_PREFS_CALIBRATION_LAP).ToString();}
        if (PlayerPrefs.HasKey(PLAYER_PREFS_CALIBRATION_MODE)){calibrationModeDrop.value = PlayerPrefs.GetInt(PLAYER_PREFS_CALIBRATION_MODE);}
    }

    public void Save(){
        int lapValue = int.Parse(calibrationLapField.text);
        PlayerPrefs.SetInt(PLAYER_PREFS_CALIBRATION_LAP, lapValue);
        PlayerPrefs.SetInt(PLAYER_PREFS_CALIBRATION_MODE, modeIndex);
        PlayerPrefs.Save();
        UpdateVisual();
    }

    public void calibModeSelector(){
        modeIndex = calibrationModeDrop.value;
    }
}
