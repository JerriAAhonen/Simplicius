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
		
		[Header("Look sway")]
		public float lookSwayHipAmount = 6f;
		public float lookSwayHipSmoothing = 0.1f;
		public float lookSwayHipResetSmoothing = 0.05f;
		public float lookSwayADSAmount = 2f;
		public float lookSwayADSSmoothing = 0.1f;
		public float lookSwayADSResetSmoothing = 0.05f;
		public float clampX = 4;
		public float clampY = 4;
		
		[Header("Movement sway")] 
		public Vector2 movementSwayHipAmount = new(10f, 2f);
		public Vector2 movementSwayADSAmount = new(4f, 0.4f);
		public float movementSwaySmoothing = 0.1f;
		public float movementSwayResetSmoothing = 0.05f;
		
		[Header("Camera Recoil")] 
		public Vector3 hipRecoil = new(-2f, 2f, 0.35f);
		public Vector3 ADSRecoil = new(-0.5f, 0.5f, 0.15f);

		[Header("Weapon Recoil")] 
		public float hipWeaponRecoil = 0.1f;
		public float adsWeaponRecoil = 0.03f;
		
		[Header("Animation Timings")]
		public float equipDur = 0.5f;
		public float adsOnDur = 0.1f;
		public float adsOffDur = 0.3f;
		
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