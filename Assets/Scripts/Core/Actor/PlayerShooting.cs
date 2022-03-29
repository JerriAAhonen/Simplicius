using System;
using EZCameraShake;
using simplicius.Audio;
using simplicius.Util;
using UnityEngine;

namespace simplicius.Core
{
	public class PlayerShooting : InitializedMonoBehaviour
	{
		[SerializeField] private AudioEvent hitSfx;
		[SerializeField] private AudioEvent armorBreakSfx;
		[SerializeField] private AudioEvent killSfx;
		
		private Weapon currentWeapon;
		private Recoil recoil;
		private Transform shootPoint;

		private int shotsInBurst;
		private float lastShotTime;
		private bool canShoot => Time.time - lastShotTime > currentWeapon.MinShootInterval_Sec;

		public event Action Shot;
		public event Action<bool> StartedShooting;
		
		public bool IsShooting { get; private set; }
		public bool IsAiming { get; private set; } 

		public WeaponContainer WeaponContainer { get; private set; }

		public void Init()
		{
			currentWeapon = GetComponentInChildren<Weapon>();
			if (!currentWeapon) return;

			currentWeapon.Init();

			shootPoint = currentWeapon.ShootPoint;
			WeaponContainer = GetComponentInChildren<WeaponContainer>();
			recoil = GetComponentInChildren<Recoil>();
			
			InputManager.Instance.Shoot += OnShoot;
			InputManager.Instance.Reload += OnReload;
			InputManager.Instance.Aim += OnAim;
			
			OnInitialized();
		}
		
		#region Shoot
		
		private void Update()
		{
			// ------------------------------
			// SINGLE
			if (currentWeapon.FireMode == FireMode.Single) 
				return;
			
			// ------------------------------
			// BURST
			if (currentWeapon.FireMode == FireMode.Burst && IsShooting && canShoot)
			{
				if (shotsInBurst < currentWeapon.BurstAmount)
				{
					shotsInBurst++;
					Shoot();
					return;
				}

				IsShooting = false;
				shotsInBurst = 0;
				return;
			}

			// ------------------------------
			// AUTO
			if (currentWeapon.FireMode == FireMode.Auto && IsShooting && canShoot)
				Shoot();
		}

		private void OnShoot(bool pressed)
		{
			// TODO: Cancel reload if possible
			
			// Weapon passive movement animation
			WeaponContainer.Shoot(pressed);

			// ------------------------------
			// SINGLE
			if (currentWeapon.FireMode == FireMode.Single && pressed && canShoot)
			{
				Shoot();
				return;
			}

			// ------------------------------
			// BURST
			if (currentWeapon.FireMode == FireMode.Burst)
			{
				IsShooting = pressed;
				return;
			}

			// ------------------------------
			// AUTO
			IsShooting = pressed;
			StartedShooting?.Invoke(pressed);
		}

		private void Shoot()
		{
			lastShotTime = Time.time;
			
			var ray = new Ray(shootPoint.position, shootPoint.forward);
			if (Physics.Raycast(ray, out var hit))
			{
				Debug.DrawRay(shootPoint.position, shootPoint.forward * hit.distance, Color.green);
				var actor = hit.collider.GetComponent<Actor>();
				if (actor)
				{
					var killed = actor.TakeDamage(10);
					Crosshair.Instance.ShowHitMarker(killed);
					AudioManager.Instance.PlayOnce(hitSfx);
					// TODO: Armor break and Kill SFX
				}
			}
			else
			{
				Debug.DrawRay(shootPoint.position, shootPoint.forward * 100f, Color.red);
			}
			
			Shot?.Invoke();
			
			// Recoil
			recoil.OnShoot(IsAiming ? currentWeapon.Properties.ADSRecoil : currentWeapon.Properties.hipRecoil);
			
			// Weapon animation
			currentWeapon.Shoot();
			
			// Camera effects
			var magnitude = IsAiming ? currentWeapon.Properties.ADSMagnitude : currentWeapon.Properties.hipMagnitude;
			var roughness = IsAiming ? currentWeapon.Properties.ADSRoughness : currentWeapon.Properties.hipRoughness;
			var fadeOutTime = IsAiming ? currentWeapon.Properties.ADSFadeOutTime : currentWeapon.Properties.hipFadeOutTime;
			CameraShaker.Instance.ShakeOnce(magnitude, roughness, 0, fadeOutTime);
		}
		
		#endregion

		#region ADS

		private void OnAim(bool pressed)
		{
			WeaponContainer.Aim(pressed);	// Weapon passive movement animation
			currentWeapon.Aim(pressed);		// Weapon active position animation
			IsAiming = pressed;
		}

		#endregion

		#region Relaod

		private void OnReload()
		{
			currentWeapon.Reload();
		}

		#endregion
	}
}