﻿using System.Collections;
using KartGame.Track;
using UnityEngine;

public class ObjectiveCalibrate : Objective
{
    
    [Tooltip("How many laps should the player complete before the game is over?")]
    public int lapsToComplete;

    private int notificationLapsRemainingThreshold = 1;
    
    public int currentLap { get; private set; }

    private const string PLAYER_PREFS_CALIBRATION_LAP = "calibrationLap";
    private const string PLAYER_PREFS_CALIBRATION_MODE = "calibrationMode";

    void Awake()
    {
        currentLap = 0;
        if (PlayerPrefs.HasKey(PLAYER_PREFS_CALIBRATION_LAP)){
            lapsToComplete = PlayerPrefs.GetInt(PLAYER_PREFS_CALIBRATION_LAP, 2);
        }
        // set a title and description specific for this type of objective, if it hasn't given one (HUD related)
        if (string.IsNullOrEmpty(title))
            title = $"Recording signal for {lapsToComplete} {targetName}s";
        
    }

    IEnumerator Start()
    {
        TimeManager.OnSetTime(totalTimeInSecs, isTimed, gameMode);
        yield return new WaitForEndOfFrame();
        Register();
    }

    protected override void ReachCheckpoint(int remaining)
    {
        if (isCompleted)
            return;

        currentLap++;

        int targetRemaining = lapsToComplete - currentLap;

        // update the objective text according to how many enemies remain to kill
        if (targetRemaining == 0)
        {
            CompleteObjective(string.Empty, GetUpdatedCounterAmount(),
                "Objective complete: " + title);
        }
        else if (targetRemaining >= 1)
        {
            // create a notification text if needed, if it stays empty, the notification will not be created
            string notificationText = notificationLapsRemainingThreshold >= targetRemaining
                ? targetRemaining + " " + targetName + "s to collect left"
                : string.Empty;

            UpdateObjective(string.Empty, GetUpdatedCounterAmount(), notificationText);
        }

    }
    
    public override string GetUpdatedCounterAmount()
    {
        return currentLap + " / " + lapsToComplete;
    }
  
   
  
  

}
