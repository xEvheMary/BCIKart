using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InGameMenuManager : MonoBehaviour
{
    [Tooltip("Root GameObject of the menu used to toggle its activation")]
    public GameObject menuRoot;
    [Tooltip("Master volume when menu is open")]
    [Range(0.001f, 1f)]
    public float volumeWhenMenuOpen = 0.3f;
    [Tooltip("Toggle component for shadows")]
    public Toggle shadowsToggle;
    [Tooltip("Toggle component for framerate display")]
    public Toggle framerateToggle;
    [Tooltip("Toggle component for sound control")]
    public Toggle soundToggle;
    [Tooltip("GameObject for the controls")]
    public GameObject controlImage;
    [Tooltip("Toggle component for arrow feedback")]
    public Toggle arrowToggle;
    [Tooltip("Toggle component for bar feedback")]
    public Toggle barToggle;
    private const string PLAYER_PREFS_MUTE_SETTING = "muteFlag";

    //PlayerInputHandler m_PlayerInputsHandler;
    FramerateCounter m_FramerateCounter;
    [SerializeField] private SignUI m_Sign;
    [SerializeField] private BarFeedbackUI m_Bar;

    void Start()
    {
        //m_PlayerInputsHandler = FindObjectOfType<PlayerInputHandler>();
        //DebugUtility.HandleErrorIfNullFindObject<PlayerInputHandler, InGameMenuManager>(m_PlayerInputsHandler, this);

        m_FramerateCounter = FindObjectOfType<FramerateCounter>();
        //DebugUtility.HandleErrorIfNullFindObject<FramerateCounter, InGameMenuManager>(m_FramerateCounter, this);

        menuRoot.SetActive(false);

        shadowsToggle.isOn = QualitySettings.shadows != ShadowQuality.Disable;
        shadowsToggle.onValueChanged.AddListener(OnShadowsChanged);

        framerateToggle.isOn = m_FramerateCounter.uiText.gameObject.activeSelf;
        framerateToggle.onValueChanged.AddListener(OnFramerateCounterChanged);

        soundToggle.isOn = PlayerPrefs.HasKey(PLAYER_PREFS_MUTE_SETTING)? !(PlayerPrefs.GetInt(PLAYER_PREFS_MUTE_SETTING) > 0) : true;
        AudioManager.Instance.SetAllAudioSources();
        soundToggle.onValueChanged.AddListener(OnSoundToggleChanged);

        arrowToggle.isOn = m_Sign.gameObject.activeSelf;
        arrowToggle.onValueChanged.AddListener(OnArrowChanged);

        barToggle.isOn = m_Bar.gameObject.activeSelf;
        barToggle.onValueChanged.AddListener(OnBarChanged);
    }

    private void OnSoundToggleChanged(bool newValue)
    {
        if (newValue){
            // Sound on
            PlayerPrefs.SetInt(PLAYER_PREFS_MUTE_SETTING, 0);
        }
        else{
            PlayerPrefs.SetInt(PLAYER_PREFS_MUTE_SETTING, 1);
        }
        PlayerPrefs.Save();
        AudioManager.Instance.SetAllAudioSources();
    }

    private void Update()
    {
        
        if (Input.GetButtonDown(GameConstants.k_ButtonNamePauseMenu)
            || (menuRoot.activeSelf && Input.GetButtonDown(GameConstants.k_ButtonNameCancel)))
        {
            if (controlImage.activeSelf)
            {
                controlImage.SetActive(false);
                return;
            }

            SetPauseMenuActivation(!menuRoot.activeSelf);

        }

        if (Input.GetAxisRaw(GameConstants.k_AxisNameVertical) != 0)
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                EventSystem.current.SetSelectedGameObject(null);
                shadowsToggle.Select();
            }
        }
    }

    public void ClosePauseMenu()
    {
        SetPauseMenuActivation(false);
    }


    public void TogglePauseMenu()
    {
        SetPauseMenuActivation(!menuRoot.activeSelf);
    }
    void SetPauseMenuActivation(bool active)
    {
        menuRoot.SetActive(active);

        if (menuRoot.activeSelf)
        {
       //     Cursor.lockState = CursorLockMode.None;
          //  Cursor.visible = true;
            Time.timeScale = 0f;
            AudioUtility.SetMasterVolume(volumeWhenMenuOpen);

            EventSystem.current.SetSelectedGameObject(null);
        }
        else
        {
         //   Cursor.lockState = CursorLockMode.Locked;
         //   Cursor.visible = false;
            Time.timeScale = 1f;
            AudioUtility.SetMasterVolume(1);
        }

    }

    void OnShadowsChanged(bool newValue)
    {
        QualitySettings.shadows = newValue ? ShadowQuality.All : ShadowQuality.Disable;
    }

    void OnFramerateCounterChanged(bool newValue)
    {
        m_FramerateCounter.uiText.gameObject.SetActive(newValue);
    }

    void OnArrowChanged(bool newValue)
    {
        m_Sign.gameObject.SetActive(newValue);
    }

    void OnBarChanged(bool newValue)
    {
        m_Bar.gameObject.SetActive(newValue);
    }

    public void OnShowControlButtonClicked(bool show)
    {
        controlImage.SetActive(show);
    }
}
