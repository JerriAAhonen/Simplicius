using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MuzzleFlash : MonoBehaviour
{
	private VisualEffect vfx;

	private void Awake()
	{
		vfx = GetComponentInChildren<VisualEffect>();
	}

	public void ShowVFX()
	{
		vfx.Play();
	}
}