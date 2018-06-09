using System;
using System.Collections;
using Cinemachine;
using GameUtils;
using UnityEngine;

namespace Utility
{
	public class CinemachineShake : SingletonObject<CinemachineShake>
	{
		[Serializable]
		public class ShakeConfig
		{
			public float Duration;
			public float Amplitude;
			public float Frequency;
		}

		public float DefaultShakeAmplitude = .5f;
		public float DefaultShakeFrequency = 10f;
		public NoiseSettings DefaultProfile;
		
		private Cinemachine.CinemachineBasicMultiChannelPerlin _perlin;
		private Cinemachine.CinemachineVirtualCamera _virtualCamera;

		private Coroutine _coroutine;
		private Coroutine _resetCoroutine;

		protected virtual void Awake () 
		{
			_virtualCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();
			_perlin = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
		}
		
		public virtual void ShakeCamera (float duration)
		{
			CameraReset();
			_coroutine = StartCoroutine(Shake (duration, DefaultShakeAmplitude, DefaultShakeFrequency, DefaultProfile));
		}
		
		public virtual void ShakeCamera (float duration, float amplitude, float frequency)
		{
			CameraReset();
			_coroutine = StartCoroutine(Shake (duration, amplitude, frequency, DefaultProfile));
		}

		public virtual void ShakeCamera (float duration, float amplitude, float frequency, NoiseSettings profile)
		{
			CameraReset();
			_coroutine = StartCoroutine(Shake (duration, amplitude, frequency, profile));
		}

		protected virtual IEnumerator Shake(float duration, float amplitude, float frequency, NoiseSettings profile)
		{
			if(duration <= 0 || amplitude <= 0 || frequency <= 0) yield break;
			
			if (_resetCoroutine != null)
			{
				StopCoroutine(_resetCoroutine);
			}
			
			_perlin.m_AmplitudeGain = amplitude;
			_perlin.m_FrequencyGain = frequency;
			_perlin.m_NoiseProfile = profile;
			yield return new WaitForSeconds(duration);
			CameraReset();
		}
		
		protected virtual IEnumerator Reset()
		{
			while (_perlin.m_AmplitudeGain > 0)
			{
				_perlin.m_AmplitudeGain -= Time.deltaTime;
				yield return null;
			}

			CameraReset();
		}
		
		protected virtual void CameraReset()
		{
			if (_coroutine != null)
			{
				StopCoroutine(_coroutine);
			}

			if (_resetCoroutine != null)
			{
				StopCoroutine(_resetCoroutine);
			}

			if (_perlin.m_AmplitudeGain > 0)
			{
				_resetCoroutine = StartCoroutine(Reset());
			}
			else
			{
				_perlin.m_AmplitudeGain = 0;
				_perlin.m_FrequencyGain = 0;
			}
		}

	}
}
