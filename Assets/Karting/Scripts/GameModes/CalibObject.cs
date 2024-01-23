using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// This class inherits from TargetObject and represents a LapObject.
/// </summary>
public class CalibObject : TargetObject
{
    public enum MIClass{
        Left, Right, Rest, End
    }
    public static event EventHandler<OnCheckpointArgs> OnCheckpointTrigger;
    public static event EventHandler<OnCheckpointArgs> OnBaselineCheckpointTrigger;
    public static event EventHandler OnBaselineAfterTrigger;
    public class OnCheckpointArgs : EventArgs{ public MIClass miclass; }

    [Header("LapObject")]
    [Tooltip("Is this the first/last lap object?")]
    public bool finishLap;

    [HideInInspector]
    public bool lapOverNextPass;

    [SerializeField] private MIClass target;
    private int calibMode;

    private const string PLAYER_PREFS_CALIBRATION_MODE = "calibrationMode";

    private const string PLAYER_PREFS_BASELINE_DELAY = "baselineDelay";

    float delayVal = 4f;

    void Start() {
        if(PlayerPrefs.HasKey(PLAYER_PREFS_CALIBRATION_MODE)){
            calibMode = PlayerPrefs.GetInt(PLAYER_PREFS_CALIBRATION_MODE);
        }
        if(PlayerPrefs.HasKey(PLAYER_PREFS_BASELINE_DELAY)){
            delayVal = (float)PlayerPrefs.GetInt(PLAYER_PREFS_BASELINE_DELAY);
        }
        Register();
    }
    
    void OnEnable()
    {
        lapOverNextPass = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!((layerMask.value & 1 << other.gameObject.layer) > 0 && other.CompareTag("Player")))
            // Anything other than the kart
            return;
        if (calibMode == 1){
            // 1 is baseline, 0 doesn't need timing modifications
            OnBaselineCheckpointTrigger?.Invoke(this, new OnCheckpointArgs{
                miclass = target
            });
            if (target != MIClass.End){
                StartCoroutine(DelayedAction(delayVal)); // Stop at this calib point, then wait for 3 seconds
            }  
            else{
                OnBaselineAfterTrigger?.Invoke(this, EventArgs.Empty);
                ActionAfter();
            }
        }
        else{
            ActionAfter();
        }
    }

    private void ActionAfter(){
        OnCheckpointTrigger?.Invoke(this, new OnCheckpointArgs{
            miclass = target
        });
        Objective.OnUnregisterPickup?.Invoke(this);
    }

    private IEnumerator DelayedAction(float dur)
    {
        yield return new WaitForSeconds(dur); // Wait for 3 seconds
        OnBaselineAfterTrigger?.Invoke(this, EventArgs.Empty);
        ActionAfter();
    }
}
