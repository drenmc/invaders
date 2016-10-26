using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PlayRandomSound : MonoBehaviour 
{
	[SerializeField] private AudioClip[] _clips;

	private AudioSource _audioSource;

	public void Play()
	{
		var rand = Random.Range(0, _clips.Length);
		_audioSource.clip = _clips[rand];
		_audioSource.Play();
	}

	private void Awake()
	{
		_audioSource = GetComponent<AudioSource>();

		if (_audioSource.playOnAwake)
		{
			Play();
		}
	}
}
