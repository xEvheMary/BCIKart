using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TestSetting : MonoBehaviour
{
    public static TestSetting Instance {get; private set;}

    [SerializeField] private TMP_InputField testLapField;
    [SerializeField] private Toggle envToggle;

    private int envVal;

    private const string PLAYER_PREFS_TEST_LAP = "testLap";
    private const string PLAYER_PREFS_ENVIRONMENT_TOGGLE = "envFlag";


    // Start is called before the first frame update
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
        if (PlayerPrefs.HasKey(PLAYER_PREFS_TEST_LAP)){testLapField.text = PlayerPrefs.GetInt(PLAYER_PREFS_TEST_LAP).ToString();}
        if (PlayerPrefs.HasKey(PLAYER_PREFS_ENVIRONMENT_TOGGLE)){envToggle.isOn = PlayerPrefs.GetInt(PLAYER_PREFS_ENVIRONMENT_TOGGLE) > 0;}
    }

    public void Save(){
        int lapValue = int.Parse(testLapField.text);
        if (envToggle.isOn == true){
            envVal = 1;
        }
        else{
            envVal = 0;
        }
        PlayerPrefs.SetInt(PLAYER_PREFS_TEST_LAP, lapValue);
        PlayerPrefs.SetInt(PLAYER_PREFS_ENVIRONMENT_TOGGLE, envVal);
        PlayerPrefs.Save();
        UpdateVisual();
    }
}
