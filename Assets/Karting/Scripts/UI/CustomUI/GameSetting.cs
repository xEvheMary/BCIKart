using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameSetting : MonoBehaviour
{
    public static GameSetting Instance {get; private set;}

    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Toggle vsyncToggle;
    [SerializeField] private List<ResItems> resolutions;
    
    private int savedResolutionIndex = 0;

    private void Awake(){
        Instance = this;
    }

    void Start()
    {   
        fullscreenToggle.isOn = Screen.fullScreen;
        if (QualitySettings.vSyncCount == 0){ vsyncToggle.isOn = false; }
        else{ vsyncToggle.isOn = true; }

        dropdown.ClearOptions();

        List<string> resolutionOptions = new List<string>();
        foreach (var resolution in resolutions)
        {
            string resolutionString = $"{resolution.width}x{resolution.height}";
            resolutionOptions.Add(resolutionString);
        }

        dropdown.AddOptions(resolutionOptions);

        savedResolutionIndex = FindRes();
        dropdown.value = savedResolutionIndex;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetResolution(int resolutionIndex)
    {
        if (resolutionIndex >= 0 && resolutionIndex < resolutions.Count)
        {
            ResItems selectedResolution = resolutions[resolutionIndex];
            Screen.SetResolution(selectedResolution.width, selectedResolution.height, fullscreenToggle.isOn);
        }
    }

    public void ApplyChanges(){
        SetResolution(dropdown.value);
        Screen.fullScreen = fullscreenToggle.isOn;
        if (QualitySettings.vSyncCount == 0){ vsyncToggle.isOn = false; }
        else{ vsyncToggle.isOn = true; }
    }

    private int FindRes(){
        int idx = 0;
        for (int i = 0; i < resolutions.Count; i++)
        {
            if(Screen.width == resolutions[i].width && Screen.height == resolutions[i].height){ idx = i; }
        }
        return idx;
    }

    [System.Serializable]
    public class ResItems{
        public int width, height;
    }
}
