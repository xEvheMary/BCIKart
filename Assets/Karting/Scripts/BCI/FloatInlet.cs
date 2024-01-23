using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LSL4Unity.OV;

namespace KartGame
{
/// <summary> An example of implementation for an Inlet receiving Float values for OpenViBE Link. </summary>
/// <seealso cref="OVFloatInlet" />
public class FloatInlet : OVFloatInlet
{
	/// <summary> Member that contains the last sample. </summary>
	/// <value> The last sample. </value>
	public float[] LastSample { get; private set; }

	/// <summary> Process when samples are available. </summary>
	/// <param name="input"> The Incomming Sample. </param>
	/// <param name="time"> The current Time. </param>
	protected override void Process(float[] input, double time) { 
		LastSample = input; 
		//Debug.Log($"Got {input.Length} thing at {time}");
	}
}
}
