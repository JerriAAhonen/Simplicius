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
		private Player player;

		private int shotsInBurst;
		private float lastShotTime;
		private bool canShoot => Time.time - lastShotTime > weapon.MinShootInterval_Sec;
		private Weapon weapon => WeaponContainer.Weapon;
		private Transform shootPoint => weapon.ShootPoint;

		public event Action Shot;
		public event Action<bool> StartedShooting;
		
		public bool IsShooting { get; private set; }
		public bool IsAiming { get; private set; } 

		public WeaponContainer WeaponContainer { get; private set; }

		public void Init(Player player)
		{
			this.player = player;
			
			WeaponContainer = GetComponentInChildren<WeaponContainer>();
			WeaponContainer.Init(player);
			recoil = GetComponentInChildren<Recoil>();

			SwitchWeapon(WeaponID.AssaultRifle1);
			IngameHUD.Instance.AmmoDisplay.SetAmmo(weapon.AmmoInClip, weapon.AmmoReserve);
			
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
			
			Debug.Log("[PlayerShooting] Shoot pressed");
			
			// Weapon passive movement animation
			//WeaponContainer.Shoot(pressed);

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
			Debug.Log($"[PlayerShooting] Shoot(), Ammo in clip: {weapon.AmmoInClip}");
			// Update Ammo
			if (weapon.AmmoInClip <= 0)
				return;

			weapon.AmmoInClip--;
			IngameHUD.Instance.AmmoDisplay.SetAmmo(weapon.AmmoInClip, weapon.AmmoReserve);
			
			lastShotTime = Time.time;
			
			var ray = new Ray(shootPoint.position, shootPoint.forward);
			if (Physics.Raycast(ray, out var hit))
			{
				Debug.DrawRay(shootPoint.position, shootPoint.forward * hit.distance, Color.green);
				var actor = hit.collider.GetComponentInParent<Actor>();
				if (actor)
				{
					var killed = actor.TakeDamage(10);
					IngameHUD.Instance.Crosshair.ShowHitMarker(killed);
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
			IngameHUD.Instance.Crosshair.ShowCrosshair(!pressed);
			WeaponContainer.Aim(pressed);
			IsAiming = pressed;
			
			if (pressed)
				player.FPCamera.SetFov(50);
			else
				player.FPCamera.ResetFov();
		}

		#endregion

		#region Relaod

		private void OnReload()
		{
			if (weapon.AmmoReserve <= 0)
				return;

			weapon.AmmoReserve -= weapon.Properties.clipSize;
			weapon.AmmoInClip = weapon.Properties.clipSize;
			IngameHUD.Instance.AmmoDisplay.SetAmmo(weapon.AmmoInClip, weapon.AmmoReserve);
			
			weapon.Reload();
		}

		#endregion

		#region Switch Weapon

		private void OnSwitchWeapon()
		{
			if (weapon.ID == WeaponID.AssaultRifle1)
				SwitchWeapon(WeaponID.Pistol1);
			else
				SwitchWeapon(WeaponID.AssaultRifle1);
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