﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventSystemCustom : MonoBehaviour
{
	public UnityEvent OnScoreIncrease;
	public UnityEvent OnHeartDecrease;
	public UnityEvent OnHeartIncrease;

	void Awake()
	{
		OnScoreIncrease = new UnityEvent();
		OnHeartDecrease = new UnityEvent();
		OnHeartIncrease = new UnityEvent();
	}
}