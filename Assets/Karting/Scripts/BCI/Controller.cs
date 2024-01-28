using System;
using System.Linq;
using LSL4Unity;
using LSL4Unity.OV;
using UnityEngine;

namespace KartGame.KartSystems
{
/// <summary> Controller is used to manage the cube. </summary>
/// <seealso cref="UnityEngine.MonoBehaviour" />
public class Controller : MonoBehaviour
{
	public static event EventHandler OnGameStart;
	public static event EventHandler<OnCueArgs> OnCueTrigger;
    public class OnCueArgs : EventArgs{ public int cueClass; }
	public static event EventHandler<OnSignalArgs> OnSignal;
    public class OnSignalArgs : EventArgs{ public float signalValue; }
	// Class Management
    private		 	 		int[] stimList = new int[] {(int) Stimulations.GDF_LEFT, (int) Stimulations.LABEL_00, (int) Stimulations.GDF_RIGHT, (int) Stimulations.GDF_END_OF_TRIAL};
	private					int	  endStim = (int) Stimulations.GDF_END_OF_SESSION;
	private					int	  startStim = (int) Stimulations.BASELINE_STOP;
	private					int	  baseStartStim = (int) Stimulations.GDF_START_OF_TRIAL;
    private int					  leftRight = 1;
    private float                 value = 1.0f;
	private float                 lrVal = 0.0f;
	private bool				  accel = true;
    
	public ArcadeKart playerKart;
	ArcadeKart[] karts;
	
	// Settings
	private const string PLAYER_PREFS_SIGNAL_OUTLET = "ovSignalOutlet";			// Name is outlet, but it's just the inlet from the openvibe's outlet
	private const string PLAYER_PREFS_MARKER_OUTLET = "ovMarkerOutlet";
	private const string PLAYER_PREFS_CALIBRATION_MODE = "calibrationMode";
	[SerializeField] private string           signalOutletName = "ovInSignal";
	[SerializeField] private string           markerOutletName = "ovInMarkers";
	[SerializeField] private FloatInlet       signalInlet;
	[SerializeField] private StimulationInlet stimInlet;

	// Outlet Variables
	//private Stream
	private       StreamOutlet outletSignal, outletMarker;
	private const int                 STIMULATION = (int) Stimulations.GDF_BEEP;
	private       double              startTime;
    private       bool                isConnected = false;
	private		  int 				  calibMode;

    private void Awake(){
        
    }
	
	private void Start()
	{
		// Setup parameters
		if(PlayerPrefs.HasKey(PLAYER_PREFS_SIGNAL_OUTLET)){
            signalOutletName = PlayerPrefs.GetString(PLAYER_PREFS_SIGNAL_OUTLET);
        }
		else{
			PlayerPrefs.SetString(PLAYER_PREFS_SIGNAL_OUTLET, signalOutletName);
            PlayerPrefs.Save();
		}

		if(PlayerPrefs.HasKey(PLAYER_PREFS_MARKER_OUTLET)){
            markerOutletName = PlayerPrefs.GetString(PLAYER_PREFS_MARKER_OUTLET);
        }
		else{
			PlayerPrefs.SetString(PLAYER_PREFS_MARKER_OUTLET, markerOutletName);
            PlayerPrefs.Save();
		}
		// Karts
		karts = FindObjectsOfType<ArcadeKart>();
        if (karts.Length > 0)
        {
            if (!playerKart) playerKart = karts[0];
        }
        DebugUtility.HandleErrorIfNullFindObject<ArcadeKart, GameFlowManager>(playerKart, this);
		// Create Streams
		StreamInfo info = new StreamInfo(signalOutletName, "signal", 1, LSL.IRREGULAR_RATE, LSL4Unity.ChannelFormat.Float32, "signal");
		outletSignal = new StreamOutlet(info);
		Debug.Log($"Creating Stream : Name = {info.Name()}, Type = {info.Type()}, Channel Count = {info.ChannelCount()}, Format = {info.ChannelFormat()}");
		info         = new StreamInfo(markerOutletName, "Marker", 1, LSL.IRREGULAR_RATE, LSL4Unity.ChannelFormat.Int32, "stimulation");
		outletMarker = new StreamOutlet(info);
		Debug.Log($"Creating Stream : Name = {info.Name()}, Type = {info.Type()}, Channel Count = {info.ChannelCount()}, Format = {info.ChannelFormat()}");
		startTime = LSL.LocalClock(); // Initialize Time
		// Calibration objects
		if (FindAnyObjectByType<CalibObject>() != null){
			CalibObject.OnCheckpointTrigger += CalibObject_OnCheckpoint;
			if(PlayerPrefs.HasKey(PLAYER_PREFS_CALIBRATION_MODE)){
				calibMode = PlayerPrefs.GetInt(PLAYER_PREFS_CALIBRATION_MODE);
				if(calibMode == 1){
					// If baseline, sub to baseline event
					CalibObject.OnBaselineCheckpointTrigger += CalibObject_OnBaselineCheckpoint;
					CalibObject.OnBaselineAfterTrigger += CalibObject_OnBaselineAfter;
			}}
		}
	}

    private void CalibObject_OnBaselineAfter(object sender, EventArgs e)
        {
            //delayFlag = true;
			playerKart.SetCanMove(true);
			accel = true;
        }

    private void CalibObject_OnBaselineCheckpoint(object sender, CalibObject.OnCheckpointArgs e)
    {
        switch(e.miclass){
			default:
				// Class start type
				//Debug.Log("Push Stimulation: "+baseStartStim);
				//print("Hit class");
				outletMarker.PushSample(new[] { baseStartStim }, LSL.LocalClock() - startTime); // Send baseline's trial start stim
				playerKart.SetCanMove(false);
				playerKart.FullStop();
				//if (delayFlag){}
				accel = false;
				break;
			case CalibObject.MIClass.End:
				// End of trial type
				//print("Hit end");
				playerKart.SetCanMove(false);
				accel = false;
				break;
		}
    }

    private void CalibObject_OnCheckpoint(object sender, CalibObject.OnCheckpointArgs e)
	{
		int i = 0;
		switch(e.miclass){
			case CalibObject.MIClass.Left:
				i = 0;
				break;
			case CalibObject.MIClass.Rest:
				i = 1;
				break;
			case CalibObject.MIClass.Right:
				i = 2;
				break;
			case CalibObject.MIClass.End:
				i = stimList.Length - 1;
				break;
		}
		Debug.Log("Push Stimulation: "+stimList[i]);
        outletMarker.PushSample(new[] { stimList[i] }, LSL.LocalClock() - startTime); // Send Stimulation back with unity time
    }

    private void Update()
	{
		// Check Stream
		if (!signalInlet.IsSolved()) { signalInlet.ResolveStream(); }
		if (!stimInlet.IsSolved()) { stimInlet.ResolveStream(); }

		// Stimulation value
		if (stimInlet.LastSample != null && stimInlet.LastSample.Length > 0 )  
        // If sample isn't null, length isn't 0, and the stimulation received is within list
		{
			isConnected = true;
			Debug.Log($"Stimulation received : {stimInlet.LastSample[0]}");
			if (stimList.Contains(stimInlet.LastSample[0])){
            	// Do something if receive certain stimulation
				leftRight = Array.FindIndex(stimList, x => x.Equals(stimInlet.LastSample[0]));  // Convert stim to action
				lrVal = value * (leftRight-1);
				// Event trigger based on stim --> for baseline (Unused?)
				OnCueTrigger?.Invoke(this, new OnCueArgs{
					cueClass = leftRight
				});
				outletMarker.PushSample(new[] { STIMULATION }, LSL.LocalClock() - startTime); // Send Stimulation back with unity time
			}
			else if (stimInlet.LastSample[0] == startStim){
				OnGameStart?.Invoke(this, EventArgs.Empty);
			}
			stimInlet.LastSample = stimInlet.LastSample.Skip(1).ToArray();	// The Stimulation stay in the variable if we don't remove the first
		}

		// Float / signal value
		if (signalInlet.LastSample != null && signalInlet.LastSample.Length > 0)
        // If sample isn't null && length isn't 0
		{
            isConnected = true;
            // Get the value from LSL (float)
			//value = Math.Abs(signalInlet.LastSample[0]);    // Get the value (Absolute)
			value = signalInlet.LastSample[0];    // Get the value (Absolute)
			if (stimInlet.LastSample != null && stimInlet.LastSample.Length > 0 ){
				lrVal = value * (leftRight-1);
			}
			else{
				lrVal = value;
			}
			//Debug.Log($"Steer val : {signalInlet.LastSample[0]} --> {lrVal}");
			OnSignal?.Invoke(this, new OnSignalArgs{
					signalValue = lrVal
				});	
			// LSL Out
			//outletSignal.PushSample(new[] { value }, LSL.LocalClock() - startTime); // Send samples with unity time (return back the value, but absolute)
		}
        else{
            isConnected = false;
        }

	}

	public bool GetConnectedFlag(){
        return isConnected;
    }

    public float GetSteerVal(){
		
        return lrVal;
    }

	public bool GetAccelBool(){
		return accel;
	}

	public void SendEndStim(){
        outletMarker.PushSample(new[] { endStim }, LSL.LocalClock() - startTime);
		SubscribeCleanup();
		Debug.Log("Push End Stimulation: "+endStim);
		Debug.Log("End recording");
	}

	public void SubscribeCleanup(){
		if (FindAnyObjectByType<CalibObject>() != null){
			CalibObject.OnCheckpointTrigger -= CalibObject_OnCheckpoint;
			if(PlayerPrefs.HasKey(PLAYER_PREFS_CALIBRATION_MODE)){
				calibMode = PlayerPrefs.GetInt(PLAYER_PREFS_CALIBRATION_MODE);
				if(calibMode == 1){
					CalibObject.OnBaselineCheckpointTrigger -= CalibObject_OnBaselineCheckpoint;
					CalibObject.OnBaselineAfterTrigger -= CalibObject_OnBaselineAfter;
			}}
		}
	}
}
}
