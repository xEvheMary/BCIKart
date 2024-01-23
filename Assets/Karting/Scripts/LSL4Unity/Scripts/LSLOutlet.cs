using System.Diagnostics;
using UnityEngine;

namespace LSL4Unity
{
public enum MomentForSampling { Update, FixedUpdate, LateUpdate }

public class LSLOutlet : MonoBehaviour
{
	private StreamOutlet outlet;
	private StreamInfo   info;
	private float[]      sample;

	public string streamName   = "Unity.ExampleStream";
	public string streamType   = "Unity.FixedUpdateTime";
	public int    channelCount = 1;

	private Stopwatch watch;

	/// <summary> Use this for initialization. </summary>
	private void Start()
	{
		watch = new Stopwatch();
		watch.Start();

		sample = new float[channelCount];
		info   = new StreamInfo(streamName, streamType, channelCount, Time.fixedDeltaTime * 1000);
		outlet = new StreamOutlet(info);
	}

	public void FixedUpdate()
	{
		if (watch == null) { return; }

		watch.Stop();

		sample[0] = watch.ElapsedMilliseconds;

		watch.Reset();
		watch.Start();

		outlet.PushSample(sample);
	}
}
}
