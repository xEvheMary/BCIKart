using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL4Unity.OV;

namespace KartGame
{
/// <summary> Implementation for an Inlet receiving Stimulations (int) from OpenViBE. </summary>
/// <seealso cref="OVIntInlet" />
public class StimulationInlet : OVIntInlet
{
	/// <summary> Member that contains the last sample. </summary>
	/// <value> The last sample. </value>
	public int[] LastSample { get; set; }

	/// <summary> Process when samples are available. </summary>
	/// <param name="input"> The Incomming Sample. </param>
	/// <param name="time"> The current Time. </param>
	protected override void Process(int[] input, double time)
	{
		LastSample = input;
		//Debug.Log($"Got {input.Length} ints at {time}");
	}
}
}

