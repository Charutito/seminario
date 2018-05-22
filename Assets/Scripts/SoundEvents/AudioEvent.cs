using Managers;
using UnityEngine;

public abstract class AudioEvent : ScriptableObject
{
	public abstract void Play(AudioSource source);

	public void Play()
	{
		SoundManager.Instance.Play(this);
	}
}