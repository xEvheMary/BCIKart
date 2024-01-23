using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using KartGame.KartSystems;
using UnityEngine.SceneManagement;
using System;

public enum TestState{Wait, Play, Won, Lost}

public class TestFlowManager : MonoBehaviour
{   
    [Header("Karts")]
    public bool autoFindKarts = true;
    public ArcadeKart playerKart;

    [Header("Parameters")]
    [Tooltip("Duration of the fade-to-black at the end of the game")]
    public float endSceneLoadDelay = 3f;
    [Tooltip("The canvas group of the fade-to-black screen")]
    public CanvasGroup endGameFadeCanvasGroup;
    public PlayableDirector raceCountdownTrigger;
    public float masterVolume = 1f;

    [Header("Delay")]
    [Tooltip("Duration of delay before the fade-to-black, if winning")]
    public float delayBeforeFadeToBlack = 4f;
    [Tooltip("Duration of delay before the win message")]
    public float delayBeforeWinMessage = 2f;

    [Header("Win")]
    [Tooltip("Prefab for the win game message")]
    public DisplayMessage winDisplayMessage;

    [Header("Lose")]
    [Tooltip("Prefab for the lose game message")]
    public DisplayMessage loseDisplayMessage;

    public TestState testState { get; private set; }

    ArcadeKart[] karts;
    ObjectiveManager m_ObjectiveManager;
    TimeManager m_TimeManager;
    Controller controller;
    float m_TimeLoadEndGameScene;
    float elapsedTimeBeforeEndScene = 0;

    private const string PLAYER_PREFS_TIME_RECORD = "recordTime";

    void Start()
    {
        // Find spawned karts
        if (autoFindKarts)
        {
            karts = FindObjectsOfType<ArcadeKart>();
            if (karts.Length > 0)
            {
                if (!playerKart) playerKart = karts[0];
            }
            DebugUtility.HandleErrorIfNullFindObject<ArcadeKart, GameFlowManager>(playerKart, this);
        }

        m_ObjectiveManager = FindObjectOfType<ObjectiveManager>();
		DebugUtility.HandleErrorIfNullFindObject<ObjectiveManager, GameFlowManager>(m_ObjectiveManager, this);

        m_TimeManager = FindObjectOfType<TimeManager>();
        DebugUtility.HandleErrorIfNullFindObject<TimeManager, GameFlowManager>(m_TimeManager, this);

        controller = FindObjectOfType<Controller>();

        AudioUtility.SetMasterVolume(masterVolume);

        Controller.OnGameStart += Controller_OnGameStart;

        winDisplayMessage.gameObject.SetActive(false);
        loseDisplayMessage.gameObject.SetActive(false);

        m_TimeManager.StopRace();
        foreach (ArcadeKart k in karts)
        {
			k.SetCanMove(false);
        }
        
        testState = TestState.Wait;
    }

    private void Controller_OnGameStart(object sender, EventArgs e)
    {
        if (testState == TestState.Wait){
            StartPlay();
        }
    }

    IEnumerator CountdownThenStartRaceRoutine() {
        yield return new WaitForSeconds(3f);
        StartRace();
    }

    void StartRace() {
        foreach (ArcadeKart k in karts)
        {
			k.SetCanMove(true);
        }
        m_TimeManager.StartRace();
    }

    void ShowRaceCountdownAnimation() {
        raceCountdownTrigger.Play();
    }

    IEnumerator ShowObjectivesRoutine() {
        while (m_ObjectiveManager.Objectives.Count == 0)
            yield return null;
        yield return new WaitForSecondsRealtime(0.2f);
        for (int i = 0; i < m_ObjectiveManager.Objectives.Count; i++)
        {
           if (m_ObjectiveManager.Objectives[i].displayMessage) m_ObjectiveManager.Objectives[i].displayMessage.Display();
           yield return new WaitForSecondsRealtime(1f);
        }
    }

    void Update()
    {
        // If not in Play state
        if (testState != TestState.Play && testState != TestState.Wait)
        {
            elapsedTimeBeforeEndScene += Time.deltaTime;        // time elapsed
            if(elapsedTimeBeforeEndScene >= endSceneLoadDelay)
            {
                // Fade out
                float timeRatio = 1 - (m_TimeLoadEndGameScene - Time.time) / endSceneLoadDelay;
                endGameFadeCanvasGroup.alpha = timeRatio;
                // Volume fade out
                float volumeRatio = Mathf.Abs(timeRatio);
                float volume = Mathf.Clamp(1 - volumeRatio, 0, 1);
                AudioUtility.SetMasterVolume(volume);
                // See if it's time to load the end scene (after the delay)
                if (Time.time >= m_TimeLoadEndGameScene)
                {
                    controller.SendEndStim();
                    SceneManager.LoadScene("EndScene");  // Load scene depending on win or lose
                    testState = TestState.Wait;
                }
            }
        }
        // Waiting for space button to start
        else if (testState == TestState.Wait){
            if(Input.GetKeyDown(KeyCode.Space)){
                StartPlay();
            }
        }
        // If in Play state
        else if (testState == TestState.Play)
        {
            if(Input.GetKeyDown(KeyCode.R)){
                playerKart.PositionReset(playerKart.spawnPoint);
            }
            // If objective done
            if (m_ObjectiveManager.AreAllObjectivesCompleted())
                EndGame(true);  // End, win game
            // Time limited and game is over
            if (m_TimeManager.IsFinite && m_TimeManager.IsOver)
                EndGame(false); // End, lose game
        }
    }

    void StartPlay(){
        testState = TestState.Play;
        WaitUI.Instance.Hide();
        //run race countdown animation
        ShowRaceCountdownAnimation();
        StartCoroutine(ShowObjectivesRoutine());            // Show the objective text display (Optional)
        StartCoroutine(CountdownThenStartRaceRoutine());    // The countdown timer
    }

    void EndGame(bool win)
    {
        // unlocks the cursor before leaving the scene, to be able to click buttons
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        m_TimeManager.StopRace();

        PlayerPrefs.SetInt(PLAYER_PREFS_TIME_RECORD, (int)m_TimeManager.TimeRemaining);
        PlayerPrefs.Save();

        // Remember that we need to load the appropriate end scene after a delay
        testState = win ? TestState.Won : TestState.Lost;
        endGameFadeCanvasGroup.gameObject.SetActive(true);
        if (win)
        {
            m_TimeLoadEndGameScene = Time.time + endSceneLoadDelay + delayBeforeFadeToBlack;

            // create a game message
            winDisplayMessage.delayBeforeShowing = delayBeforeWinMessage;
            winDisplayMessage.gameObject.SetActive(true);
        }
        else
        {
            m_TimeLoadEndGameScene = Time.time + endSceneLoadDelay + delayBeforeFadeToBlack;

            // create a game message
            loseDisplayMessage.delayBeforeShowing = delayBeforeWinMessage;
            loseDisplayMessage.gameObject.SetActive(true);
        }
    }

    private void OnDestroy(){
        Controller.OnGameStart -= Controller_OnGameStart;
    }
}
