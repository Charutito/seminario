using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName="Audio Events/Simple")]
public class SimpleAudioEvent : AudioEvent
{
	public AudioClip[] clips;

	public RangedFloat volume;

	[MinMaxRange(0, 2)]
	public RangedFloat pitch;

	public override void Play(AudioSource source)
	{
		if (clips.Length == 0) return;

		source.clip = clips[Random.Range(0, clips.Length)];
		source.volume = Random.Range(volume.minValue, volume.maxValue);
		source.pitch = Random.Range(pitch.minValue, pitch.maxValue);
		source.Play();
	}

	public override void Play(Vector3 position)
	{
		if (clips.Length == 0) return;

		var clip = clips[Random.Range(0, clips.Length)];

		var clipVolume = Random.Range(volume.minValue, volume.maxValue);
		
		AudioSource.PlayClipAtPoint(clip, position, clipVolume);
	}
}