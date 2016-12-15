using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class PlayRandomSound : MonoBehaviour 
{
	public enum PlayOnEvent
	{
		Enable,
		Disable
	}

	[SerializeField] private PlayOnEvent _playOn = PlayOnEvent.Enable;
	[SerializeField] private AudioClip[] _clips;

	private AudioSource _audioSource;

	public void Play()
	{
		if (_clips.Length == 0)
		{
			return;
		}

		var rand = Random.Range(0, _clips.Length);
		_audioSource.clip = _clips[rand];
		_audioSource.Play();
	}

	private void OnEnable()
	{
		if (_playOn == PlayOnEvent.Enable)
		{
			Play();
		}
	}

	private void OnDisable()
	{
		if (_playOn == PlayOnEvent.Disable)
		{
			Play();
		}
	}

	private void Awake()
	{
		_audioSource = GetComponent<AudioSource>();
		_audioSource.playOnAwake = false;
	}
}
