using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LSLSetting : MonoBehaviour
{
    public static LSLSetting Instance {get; private set;}
    
    [SerializeField] private TMP_InputField signalInletInputField;
    [SerializeField] private TMP_InputField markerInletInputField;
    [SerializeField] private TMP_InputField signalOutletInputField;
    [SerializeField] private TMP_InputField markerOutletInputField;

    private const string PLAYER_PREFS_SIGNAL_OUTLET = "ovSignalOutlet";			// Name is outlet, but it's just the inlet from the openvibe's outlet
	private const string PLAYER_PREFS_MARKER_OUTLET = "ovMarkerOutlet";
    private const string PLAYER_PREFS_SIGNAL_INLET = "ovSignalInlet";			// Name is outlet, but it's just the inlet from the openvibe's outlet
	private const string PLAYER_PREFS_MARKER_INLET = "ovMarkerInlet";
    
    private void Awake()
    {
        Instance = this;
    }   
    // Start is called before the first frame update
    void Start()
    {
        UpdateVisual();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void UpdateVisual(){
        if (PlayerPrefs.HasKey(PLAYER_PREFS_SIGNAL_INLET)){signalInletInputField.text = PlayerPrefs.GetString(PLAYER_PREFS_SIGNAL_INLET);}
        if (PlayerPrefs.HasKey(PLAYER_PREFS_MARKER_INLET)){markerInletInputField.text = PlayerPrefs.GetString(PLAYER_PREFS_MARKER_INLET);}
        if (PlayerPrefs.HasKey(PLAYER_PREFS_SIGNAL_OUTLET)){signalOutletInputField.text = PlayerPrefs.GetString(PLAYER_PREFS_SIGNAL_OUTLET);}
        if (PlayerPrefs.HasKey(PLAYER_PREFS_MARKER_OUTLET)){markerOutletInputField.text = PlayerPrefs.GetString(PLAYER_PREFS_MARKER_OUTLET);}
    }

    public void SaveChanges(){
        PlayerPrefs.SetString(PLAYER_PREFS_SIGNAL_INLET, signalInletInputField.text);
        PlayerPrefs.SetString(PLAYER_PREFS_MARKER_INLET, markerInletInputField.text);
        PlayerPrefs.SetString(PLAYER_PREFS_SIGNAL_OUTLET, signalOutletInputField.text);
        PlayerPrefs.SetString(PLAYER_PREFS_MARKER_OUTLET, markerOutletInputField.text);
        PlayerPrefs.Save();
        UpdateVisual();
    }
}

