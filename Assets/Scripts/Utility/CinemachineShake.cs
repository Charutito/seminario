using System.Collections;
using Cinemachine;
using GameUtils;
using UnityEngine;

namespace Utility
{
	public class CinemachineShake : SingletonObject<CinemachineShake>
	{
		public float DefaultShakeAmplitude = .5f;
		public float DefaultShakeFrequency = 10f;
		public NoiseSettings DefaultProfile;
		
		private Cinemachine.CinemachineBasicMultiChannelPerlin _perlin;
		private Cinemachine.CinemachineVirtualCamera _virtualCamera;

		private Coroutine _coroutine;

		protected virtual void Awake () 
		{
			_virtualCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
			_perlin = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
		}
		
		public virtual void ShakeCamera (float duration)
		{
			CameraReset();
			_coroutine = StartCoroutine(ShakeCameraCo (duration, DefaultShakeAmplitude, DefaultShakeFrequency, DefaultProfile));
		}
		
		public virtual void ShakeCamera (float duration, float amplitude, float frequency)
		{
			CameraReset();
			_coroutine = StartCoroutine(ShakeCameraCo (duration, amplitude, frequency, DefaultProfile));
		}

		public virtual void ShakeCamera (float duration, float amplitude, float frequency, NoiseSettings profile)
		{
			CameraReset();
			_coroutine = StartCoroutine(ShakeCameraCo (duration, amplitude, frequency, profile));
		}

		protected virtual IEnumerator ShakeCameraCo(float duration, float amplitude, float frequency, NoiseSettings profile)
		{
			_perlin.m_AmplitudeGain = amplitude;
			_perlin.m_FrequencyGain = frequency;
			_perlin.m_NoiseProfile = profile;
			_perlin.enabled = true;
			yield return new WaitForSeconds(duration);
			CameraReset();
		}
		
		protected virtual void CameraReset()
		{
			if (_coroutine != null)
			{
				StopCoroutine(_coroutine);
			}

			_perlin.enabled = false;
		}

	}
}
