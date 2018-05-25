using Managers;
using UnityEngine;

public abstract class AudioEvent : ScriptableObject
{
	public abstract void Play(AudioSource source);
	public abstract void Play(Vector3 position);

	public void Play()
	{
		SoundManager.Instance.Play(this);
	}
}