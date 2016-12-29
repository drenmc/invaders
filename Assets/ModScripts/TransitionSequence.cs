using UnityEngine;
using System;
using System.Collections;

public class TransitionSequence : MonoBehaviour 
{
	public event Action Done;

	private StepSequencer[] _sequencers;

	private void Awake()
	{
		_sequencers = GetComponentsInChildren<StepSequencer>();

		foreach (var seq in _sequencers)
		{
			seq.Looped += HandleSequencerLooped;
			seq.Suspend = true;
		}
	}

	public void Play()
	{
		foreach (var seq in _sequencers)
		{
			seq.Reset();
			seq.Suspend = false;
		}
	}

	void HandleSequencerLooped(StepSequencer seq)
	{
		seq.Suspend = true;
		if (Done != null)
		{
			Done();
		}
	}
}
