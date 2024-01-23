using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LSL4Unity.Editor
{
public class LSLShowStreamsWindow : EditorWindow
{
	//public double WaitOnResolveStreams = 2;

	private const string NO_STREAMS_FOUND = "No streams found!";
	private const string N_STREAMS_FOUND  = " Streams found";

	//private const string CLICK_LOOK_UP_FIRST = "Click lookup first";

	private readonly List<string> namesOfStreams = new List<string>();

	private Vector2 scrollVector;
	private string  streamLookUpResult;

	private ContinuousResolver resolver;
	private string             lslVersionInfos;

	public void Init()
	{
		resolver = new ContinuousResolver();

		int libVersion      = LSL.LibraryVersion();
		int protocolVersion = LSL.ProtocolVersion();

		int libMajor  = libVersion / 100;
		int libMinor  = libVersion % 100;
		int protMajor = protocolVersion / 100;
		int protMinor = protocolVersion % 100;

		lslVersionInfos = $"You are using LSL library: {libMajor}.{libMinor} implementing protocol version: {protMajor}.{protMinor}";

		titleContent = new GUIContent("LSL Utility");
	}

	private StreamInfo[] streamInfos = null;

	private void OnGUI()
	{
		if (resolver == null) { Init(); }

		UpdateStreams();

		EditorGUILayout.BeginVertical();
		EditorGUILayout.Space();
		EditorGUILayout.LabelField(lslVersionInfos, EditorStyles.miniLabel);
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(streamLookUpResult, EditorStyles.boldLabel);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space();
		EditorGUILayout.Separator();

		scrollVector = EditorGUILayout.BeginScrollView(scrollVector, GUILayout.Width(EditorGUIUtility.currentViewWidth));
		GUILayoutOption fieldWidth = GUILayout.Width(EditorGUIUtility.currentViewWidth / 4.3f);

		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Name",      EditorStyles.boldLabel, fieldWidth);
		EditorGUILayout.LabelField("Type",      EditorStyles.boldLabel, fieldWidth);
		EditorGUILayout.LabelField("HostName",  EditorStyles.boldLabel, fieldWidth);
		EditorGUILayout.LabelField("Data Rate", EditorStyles.boldLabel, fieldWidth);
		EditorGUILayout.EndHorizontal();

		//foreach (string item in namesOfStreams)
		//{
		//	string[] s = item.Split(' ');
		//
		//	EditorGUILayout.BeginHorizontal();
		//	EditorGUILayout.LabelField(new GUIContent(s[0], s[0]), fieldWidth);
		//	EditorGUILayout.LabelField(new GUIContent(s[1], s[1]), fieldWidth);
		//	EditorGUILayout.LabelField(new GUIContent(s[2], s[2]), fieldWidth);
		//	EditorGUILayout.LabelField(new GUIContent(s[3], s[3]), fieldWidth);
		//	EditorGUILayout.EndHorizontal();
		//}
		//Replace by LINQ Expression
		foreach (string[] s in namesOfStreams.Select(item => item.Split(' '))) {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(new GUIContent(s[0], s[0]), fieldWidth);
			EditorGUILayout.LabelField(new GUIContent(s[1], s[1]), fieldWidth);
			EditorGUILayout.LabelField(new GUIContent(s[2], s[2]), fieldWidth);
			EditorGUILayout.LabelField(new GUIContent(s[3], s[3]), fieldWidth);
			EditorGUILayout.EndHorizontal();
		}

		EditorGUILayout.EndScrollView();
		EditorGUILayout.EndVertical();
	}

	private void UpdateStreams()
	{
		namesOfStreams.Clear();
		streamInfos = resolver.Results();

		if (streamInfos.Length == 0) { streamLookUpResult = NO_STREAMS_FOUND; }
		else {
			foreach (StreamInfo item in streamInfos) { namesOfStreams.Add($"{item.Name()} {item.Type()} {item.Hostname()} {item.Sampling()}"); }
			streamLookUpResult = namesOfStreams.Count + N_STREAMS_FOUND;
		}
	}
}
}
