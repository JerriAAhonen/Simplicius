using System;
using EZCameraShake;
using simplicius.Audio;
using simplicius.Util;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace simplicius.Core
{
	public class PlayerShooting : InitializedMonoBehaviour
	{
		[SerializeField] private AudioEvent hitSfx;
		[SerializeField] private AudioEvent armorBreakSfx;
		[SerializeField] private AudioEvent killSfx;
		[SerializeField] private TwoBoneIKConstraint rightHand;
		[SerializeField] private TwoBoneIKConstraint leftHand;
		
		private Recoil recoil;
		private Transform shootPoint;

		private int shotsInBurst;
		private float lastShotTime;
		private bool canShoot => Time.time - lastShotTime > weapon.MinShootInterval_Sec;
		private Weapon weapon => WeaponContainer.Weapon;

		public event Action Shot;
		public event Action<bool> StartedShooting;
		
		public bool IsShooting { get; private set; }
		public bool IsAiming { get; private set; } 

		public WeaponContainer WeaponContainer { get; private set; }

		public void Init()
		{
			WeaponContainer = GetComponentInChildren<WeaponContainer>();
			recoil = GetComponentInChildren<Recoil>();

			SwitchWeapon(WeaponID.M4);
			
			InputManager.Instance.Shoot += OnShoot;
			InputManager.Instance.Reload += OnReload;
			InputManager.Instance.Aim += OnAim;
			InputManager.Instance.SwitchWeapon += OnSwitchWeapon;
			
			OnInitialized();
		}

		private void OnDisable()
		{
			InputManager.Instance.Shoot -= OnShoot;
			InputManager.Instance.Reload -= OnReload;
			InputManager.Instance.Aim -= OnAim;
			InputManager.Instance.SwitchWeapon -= OnSwitchWeapon;
		}

		#region Shoot
		
		private void Update()
		{
			// ------------------------------
			// SINGLE
			if (weapon.FireMode == FireMode.Single) 
				return;
			
			// ------------------------------
			// BURST
			if (weapon.FireMode == FireMode.Burst && IsShooting && canShoot)
			{
				if (shotsInBurst < weapon.BurstAmount)
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
			if (weapon.FireMode == FireMode.Auto && IsShooting && canShoot)
				Shoot();
		}

		private void OnShoot(bool pressed)
		{
			// TODO: Cancel reload if possible
			
			// Weapon passive movement animation
			WeaponContainer.Shoot(pressed);

			// ------------------------------
			// SINGLE
			if (weapon.FireMode == FireMode.Single && pressed && canShoot)
			{
				Shoot();
				return;
			}

			// ------------------------------
			// BURST
			if (weapon.FireMode == FireMode.Burst)
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
			recoil.OnShoot(IsAiming ? weapon.Properties.ADSRecoil : weapon.Properties.hipRecoil);
			
			// Weapon animation
			weapon.Shoot();
			
			// Camera effects
			var magnitude = IsAiming ? weapon.Properties.ADSMagnitude : weapon.Properties.hipMagnitude;
			var roughness = IsAiming ? weapon.Properties.ADSRoughness : weapon.Properties.hipRoughness;
			var fadeOutTime = IsAiming ? weapon.Properties.ADSFadeOutTime : weapon.Properties.hipFadeOutTime;
			CameraShaker.Instance.ShakeOnce(magnitude, roughness, 0, fadeOutTime);
		}
		
		#endregion

		#region ADS

		private void OnAim(bool pressed)
		{
			WeaponContainer.Aim(pressed);	// Weapon passive movement animation
			weapon.Aim(pressed);			// Weapon active position animation
			IsAiming = pressed;
		}

		#endregion

		#region Relaod

		private void OnReload()
		{
			weapon.Reload();
		}

		#endregion

		#region Switch Weapon

		private void OnSwitchWeapon()
		{
			
		}
		
		private void SwitchWeapon(WeaponID id)
		{
			WeaponContainer.ChangeWeapon(id);
			rightHand.data.target = WeaponContainer.Weapon.RearHandRef;
			leftHand.data.target = WeaponContainer.Weapon.FrontHandRef;
		}

		#endregion
	}
}