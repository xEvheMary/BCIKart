using System;
using UnityEngine;

namespace LSL4Unity
{
public static class LSLUtils
{
	private const int DEFAULT_PLATTFORM_SPECIFIC_FRAMERATE = -1;

	/// <summary> - EXPERIMENTAL -
	/// Use this to get the sampling rate on which most post processing procedures rely - like importing recorded date as .xdf file to EEGLAB. </summary>
	/// <param name="moment"> Enumeration value for FixedUpdate, Update, LateUpdate ... </param>
	/// <param name="setRefreshRateToDisplay"> Set this to false, if another framerate has been specified manually </param>
	/// <returns></returns>
	public static float GetSamplingRateFor(MomentForSampling moment, bool setRefreshRateToDisplay = true)
	{
		switch (moment) {
			case MomentForSampling.FixedUpdate: return 1000 / (1000 * Time.fixedDeltaTime);
			case MomentForSampling.Update:
			case MomentForSampling.LateUpdate:
			{
				if (Application.targetFrameRate == DEFAULT_PLATTFORM_SPECIFIC_FRAMERATE && !setRefreshRateToDisplay) {
					throw new InvalidOperationException("When using Update or LateUpdate as sampling moment - specify a target frameRate");
				}
				if (setRefreshRateToDisplay) {
					Debug.LogWarning("Application.targetFrameRate get set to Screen.currentResolution.refreshRate!");
					Application.targetFrameRate = Screen.currentResolution.refreshRate;
				}
				return 1.0f / Application.targetFrameRate;
			}
			default: return 0.0f;
		}
	}
}

/// <summary>
/// Using the XML classes from LSL namespace is a bit clumsy.
/// A support class to construct channel definitions automatically.
/// See Transform Outlet as an example.
/// </summary>
internal struct ChannelDefinition
{
	public string label;
	public string unit;
	public string type;
}
}
