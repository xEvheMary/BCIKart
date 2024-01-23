using System;
using System.Collections.Generic;
using KartGame.Track;
using UnityEngine;
using UnityEngine.Events;

public enum GameMode
{
    TimeLimit, Crash, Laps, Calibrate, Test, Baseline
}

public abstract class Objective : MonoBehaviour
{
    [Tooltip("Which game mode are you playing?")]
    public GameMode gameMode;

    protected int m_PickupTotal;

    [Tooltip("Name of the target object the player will collect/crash/complete for this objective")]
    public string targetName;

    [Tooltip("Short text explaining the objective that will be shown on screen")]
    public string title;

    [Tooltip("Short text explaining the objective that will be shown on screen")]
    public string description;

    [Tooltip("Whether the objective is required to win or not")]
    public bool isOptional;

    [Tooltip("Delay before the objective becomes visible")]
    public float delayVisible;

    [Header("Requirements")] [Tooltip("Does the objective have a time limit?")]
    public bool isTimed;

    [Tooltip("If there is a time limit, how long in secs?")]
    public int totalTimeInSecs;
    public bool isCompleted { get; protected set; }
    public bool isBlocking() => !(isOptional || isCompleted);

    public UnityAction<UnityActionUpdateObjective> onUpdateObjective;

    protected NotificationHUDManager m_NotificationHUDManager;
    protected ObjectiveHUDManger m_ObjectiveHUDManger;
    
    public static Action<TargetObject> OnRegisterPickup;
    public static Action<TargetObject> OnUnregisterPickup;
    
    public DisplayMessage displayMessage;

    private List<TargetObject> pickups = new List<TargetObject>();

    public List<TargetObject> Pickups => pickups;
    public int NumberOfPickupsTotal { get; private set; }
    public int NumberOfPickupsRemaining => Pickups.Count;
    
    public int NumberOfActivePickupsRemaining()
    {
        int total = 0;
        for (int i = 0; i < Pickups.Count; i++)
        {
            if (Pickups[i].active) total++;
        }

        return total;
    }

    protected abstract void ReachCheckpoint(int remaining);
    
    void OnEnable()
    {
        OnRegisterPickup += RegisterPickup;
        OnUnregisterPickup += UnregisterPickup;
    }

    protected void Register()
    {
        // add this objective to the list contained in the objective manager
        ObjectiveManager.RegisterObjective(this);

        if (new List<GameMode>{GameMode.Crash, GameMode.Laps, GameMode.TimeLimit}.Contains(gameMode)){
            // register this objective in the ObjectiveHUDManger
            m_ObjectiveHUDManger = FindObjectOfType<ObjectiveHUDManger>();
            DebugUtility.HandleErrorIfNullFindObject<ObjectiveHUDManger, Objective>(m_ObjectiveHUDManger, this);
            m_ObjectiveHUDManger.RegisterObjective(this);
        }
        if (new List<GameMode>{GameMode.Crash, GameMode.Laps, GameMode.TimeLimit}.Contains(gameMode)){
        // register this objective in the NotificationHUDManager
            m_NotificationHUDManager = FindObjectOfType<NotificationHUDManager>();
            DebugUtility.HandleErrorIfNullFindObject<NotificationHUDManager, Objective>(m_NotificationHUDManager, this);
            m_NotificationHUDManager.RegisterObjective(this);
        }
    }

    public void UpdateObjective(string descriptionText, string counterText, string notificationText)
    {
        // HUD Related
        onUpdateObjective?.Invoke(new UnityActionUpdateObjective(this, descriptionText, counterText, false,
            notificationText));
    }

    public void CompleteObjective(string descriptionText, string counterText, string notificationText)
    {
        isCompleted = true;     // Flag
        UpdateObjective(descriptionText, counterText, notificationText); // Also HUD related

        // unregister this objective form both HUD managers
        if (m_ObjectiveHUDManger != null) {m_ObjectiveHUDManger.UnregisterObjective(this);}
        if (m_NotificationHUDManager != null) {m_NotificationHUDManager.UnregisterObjective(this);}
    }

    public virtual string GetUpdatedCounterAmount()
    {
        return "";
    }
    
    public void RegisterPickup(TargetObject pickup)
    {
        if (pickup.gameMode != gameMode) return;    // Ignore if objective is of different type

        Pickups.Add(pickup);

        NumberOfPickupsTotal++;
    }

    public void UnregisterPickup(TargetObject pickupCollected)
    {
        if (pickupCollected.gameMode != gameMode) return;   // Ignore if objective hit is of different type

        // removes the pickup from the list, so that we can keep track of how many are left on the map
        if (pickupCollected.gameMode == GameMode.Laps)
        {
            pickupCollected.active = false;

            LapObject lapObject = (LapObject) pickupCollected;

            if (!lapObject.finishLap) return;   // if this lap object is not the last lap, the return

            if (!lapObject.lapOverNextPass)
            {
                TimeDisplay.OnUpdateLap();
                lapObject.lapOverNextPass = true;
                return;
            }

            if (NumberOfActivePickupsRemaining() != 0) return;

            ReachCheckpoint(0);
            ResetPickups();
            TimeDisplay.OnUpdateLap();      // HUD

        }
        else if (pickupCollected.gameMode == GameMode.Calibrate){
            pickupCollected.active = false;

            if (pickupCollected is CalibObject){ 
                CalibObject calibObject = (CalibObject) pickupCollected;
                if (!calibObject.finishLap) return;   // if this lap object is not the last lap, the return

                if (!calibObject.lapOverNextPass)
                {
                    calibObject.lapOverNextPass = true;
                    return;
                }

                if (NumberOfActivePickupsRemaining() != 0) return;

                ReachCheckpoint(0);
                ResetPickups();
            }
        }
        else if (pickupCollected.gameMode == GameMode.Test){
            pickupCollected.active = false;

            LapObject lapObject = (LapObject) pickupCollected;

            if (!lapObject.finishLap) return;   // if this lap object is not the last lap, the return

            if (!lapObject.lapOverNextPass)
            {
                //TimeDisplay.OnUpdateLap();
                lapObject.lapOverNextPass = true;
                return;
            }

            if (NumberOfActivePickupsRemaining() != 0) return;

            ReachCheckpoint(0);
            ResetPickups();
            //TimeDisplay.OnUpdateLap();      // HUD

        }
        else
        {
            ReachCheckpoint(NumberOfPickupsRemaining - 1);
            Pickups.Remove(pickupCollected);
            if (gameMode == GameMode.Laps)
                KartGame.Track.TimeDisplay.OnUpdateLap();
        }
    }

    public void ResetPickups()
    {
        for (int i = 0; i < Pickups.Count; i++)
        {
            Pickups[i].active = true;
        }
    }
    
    void OnDisable()
    {
        OnRegisterPickup -= RegisterPickup;
        OnUnregisterPickup -= UnregisterPickup;
    }

}

public class UnityActionUpdateObjective
{
    public Objective objective;
    public string descriptionText;
    public string counterText;
    public bool isComplete;
    public string notificationText;

    public UnityActionUpdateObjective(Objective objective, string descriptionText, string counterText, bool isComplete, string notificationText)
    {
        this.objective = objective;
        this.descriptionText = descriptionText;
        this.counterText = counterText;
        this.isComplete = isComplete;
        this.notificationText = notificationText;
    }
}
