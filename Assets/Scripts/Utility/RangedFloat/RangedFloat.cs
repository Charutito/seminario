﻿using System;

[Serializable]
public struct RangedFloat
{
	public float minValue;
	public float maxValue;
	public float GetRandom
	{
		get { return UnityEngine.Random.Range(minValue, maxValue); }
	}
}