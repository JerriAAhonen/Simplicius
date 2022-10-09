using System;
using UnityEngine;

namespace simplicius.Core
{
	[Serializable]
	public class WeaponProperties
	{
		[Header("General")]
		public int damage = 10;
		public int clipSize = 30;
		public int maxReserveAmmo = 300;
		public float reloadTime = 1f;
		
		[Header("Fire Mode")]
		public FireMode fireMode = FireMode.Auto;
		public  int burstAmount = 3;
		public  int rateOfFire = 850;
		
		[Header("Recoil")] 
		public Vector3 hipRecoil = new(-2f, 2f, 0.35f);
		public Vector3 ADSRecoil = new(-0.5f, 0.5f, 0.15f);
		
		[Header("Animation Timings")]
		public float equipDur = 0.5f;
		public float ADSDur = 0.2f;
		
		[Header("Camera Shake")] 
		public float hipMagnitude = 0.5f;
		public float hipRoughness = 10f;
		public float hipFadeOutTime = 0.5f;
		
		[Space]
		public float ADSMagnitude = 0.1f;
		public float ADSRoughness = 5f;
		public float ADSFadeOutTime = 0.1f;
	}
}