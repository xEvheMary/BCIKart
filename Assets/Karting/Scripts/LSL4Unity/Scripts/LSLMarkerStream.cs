using System.Collections;
using UnityEngine;

namespace LSL4Unity
{
[HelpURL("https://github.com/xfleckx/LSL4Unity/wiki#using-a-marker-stream")]
public class LSLMarkerStream : MonoBehaviour
{
	public string streamName = "Unity_<Paradigma_Name_here>";
	public string streamType = "LSL_Marker_Strings";

	private const string UNIQUE_SOURCE_ID  = "D3F83BB699EB49AB94A9FA44B88882AB";
	private const int    LSL_CHANNEL_COUNT = 1;
	private const double NOMINAL_SRATE     = LSL.IRREGULAR_RATE;

	private const ChannelFormat LSL_CHANNEL_FORMAT = ChannelFormat.Str;

	private StreamInfo   info;
	private StreamOutlet outlet;

	//Assuming that markers are never send in regular intervalls
	private string[] sample;

	private void Awake()
	{
		sample = new string[LSL_CHANNEL_COUNT];
		info   = new StreamInfo(streamName, streamType, LSL_CHANNEL_COUNT, NOMINAL_SRATE, LSL_CHANNEL_FORMAT, UNIQUE_SOURCE_ID);
		outlet = new StreamOutlet(info);
	}

	public void Write(string marker, double timeStamp = 0)
	{
		sample[0] = marker;
		outlet.PushSample(sample, timeStamp);
	}

	public void WriteBeforeFrameIsDisplayed(string marker) { StartCoroutine(WriteMarkerAfterImageIsRendered(marker)); }

	private IEnumerator WriteMarkerAfterImageIsRendered(string pendingMarker)
	{
		yield return new WaitForEndOfFrame();
		Write(pendingMarker);
		yield return null;
	}
}
}
