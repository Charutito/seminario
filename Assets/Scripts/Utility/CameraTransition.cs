using System.Collections;
using System.Collections.Generic;
using Entities;
using Managers;
using UnityEngine;
using UnityEngine.UI;

public class CameraTransition : MonoBehaviour
{
	[Header("Shader Config")]
	[Range(0, 1)]
	public float CurrentTransition;
	public Material Material;
	public Camera SecondCamera;
	
	[Header("Effect Config")]
	[Range(0, 1)]
	public float DeathTimeScale = 0.4f;
	[Range(0, 5)]
	public float DeathFadeTime = 2f;

	public void ChangeTransitionValue(float value)
	{
		CurrentTransition = value;
	}

	private void Start()
	{
		GameManager.Instance.Character.OnDeath += OnCharacterDeath;
	}

	private void OnCharacterDeath(Entity entity)
	{
		Time.timeScale = DeathTimeScale;
		
		iTween.ValueTo( gameObject, iTween.Hash(
				"from", 0f,
				"to", 1f,
				"time", DeathFadeTime,
				"onupdatetarget", gameObject,
				"onupdate", "ChangeTransitionValue",
				"easetype", iTween.EaseType.easeInCubic
			)
		);
	}

	private void Update()
	{
		SecondCamera.gameObject.SetActive(CurrentTransition > 0);
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		Material.SetFloat("_Transition", CurrentTransition);
		Graphics.Blit(src, dest, Material);
	}
}
