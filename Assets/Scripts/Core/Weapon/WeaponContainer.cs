using System;
using System.Collections.Generic;
using simplicius.Core;
using simplicius.Util;
using UnityEngine;

public class WeaponContainer : InitializedMonoBehaviour
{
	#region Animator Hashes
	private static readonly int idle = Animator.StringToHash("Idle");
	private static readonly int walk = Animator.StringToHash("Walk");
	private static readonly int sprint = Animator.StringToHash("Sprint");
	private static readonly int jump = Animator.StringToHash("Jump");
	private static readonly int land = Animator.StringToHash("Land");
	private static readonly int aiming = Animator.StringToHash("Aiming");
	private static readonly int shooting = Animator.StringToHash("Shooting");
	#endregion
	
	[Header("Weapon refs")]
	[SerializeField] private List<Weapon> weapons;
	[Header("Look sway")]
	[SerializeField] private float lookSwayAmount = 6f;
	[SerializeField] private float lookSwaySmoothing = 0.1f;
	[SerializeField] private float lookSwayResetSmoothing = 0.05f;
	[SerializeField] private float clampX = 4;
	[SerializeField] private float clampY = 4;
	[Header("Movement sway")] 
	[SerializeField] private Vector2 movementSwayAmount = new(10f, 2f);
	[SerializeField] private float movementSwaySmoothing = 0.1f;
	[SerializeField] private float movementSwayResetSmoothing = 0.05f;
	[Header("Sights")] 
	[SerializeField] private float sightOffset;
	[SerializeField] private float adsSmoothing;
	[SerializeField] private Transform adsObject;
	[Header("Weapon Recoil")] 
	[SerializeField] private float recoilAmount;

	private bool isInitialized;
	private Transform cameraTm;
	private Animator animator;
	private bool isAiming;
	
	#region Sway 
	
	// Look
	private Vector3 targetLookRotation;
	private Vector3 targetLookRotationVelocity;
		
	private Vector3 newLookRotation;
	private Vector3 newLookRotationVelocity;

	// Movement
	private Vector3 targetMovementRotation;
	private Vector3 targetMovementRotationVelocity;

	private Vector3 newMovementRotation;
	private Vector3 newMovementRotationVelocity;
	
	#endregion

	#region ADS

	private Vector3 targetADSPosition;
	private Vector3 targetADSPositionVelocity;
	private Vector3 newADSPosition;
	private Vector3 newADSPositionVelocity;

	#endregion

	#region Recoil

	private Vector3 targetRecoilPosition;
	private Vector3 targetRecoilPositionVelocity;
	private Vector3 newRecoilPosition;
	private Vector3 newRecoilPositionVelocity;

	#endregion
	
	public Weapon Weapon { get; private set; }
	public event Action<Weapon> WeaponChanged;

	private void Awake()
	{
		animator = GetComponentInChildren<Animator>();
		foreach (var weapon in weapons)
			weapon.gameObject.SetActive(false);
	}

	private void Update()
	{
		if (!IsReady) return;
		
		CalculateRotation();
		CalculatePosition();
	}

	public void Init(Player player)
	{
		cameraTm = player.CameraRoot;
		OnInitialized();
	}
	
	private void CalculateRotation()
	{
		// Look
		targetLookRotation.y += lookSwayAmount * InputManager.Instance.LookInput.x * Time.deltaTime;
		targetLookRotation.x += -lookSwayAmount * InputManager.Instance.LookInput.y * Time.deltaTime;
		targetLookRotation.Clamp(clampX, clampY, 0);
		targetLookRotation.z = -targetLookRotation.y;
			
		targetLookRotation = Vector3.SmoothDamp(targetLookRotation, Vector3.zero, ref targetLookRotationVelocity, movementSwayResetSmoothing);
		newLookRotation = Vector3.SmoothDamp(newLookRotation, targetLookRotation, ref newLookRotationVelocity, lookSwaySmoothing);

		// Movement
		targetMovementRotation.z = -movementSwayAmount.x * InputManager.Instance.MovementInput.x;
		targetMovementRotation.x = movementSwayAmount.y * InputManager.Instance.MovementInput.y;
			
		targetMovementRotation = Vector3.SmoothDamp(targetMovementRotation, Vector3.zero, ref targetMovementRotationVelocity, lookSwayResetSmoothing);
		newMovementRotation = Vector3.SmoothDamp(newMovementRotation, targetMovementRotation, ref newMovementRotationVelocity, movementSwaySmoothing);
			
		// Set
		transform.localRotation = Quaternion.Euler(newLookRotation + newMovementRotation);
	}

	private Vector3 adsObjectPosition;
	private Vector3 adsPositionVelocity;
	
	private bool shot;
	private void CalculatePosition()
	{
		var targetPosition = transform.position;
		if (isAiming)
			targetPosition = cameraTm.position + (adsObject.position - Weapon.ReticleTm.position);
		
		if (shot)
		{
			shot = false;
			targetPosition += -transform.forward * recoilAmount;
		}

		adsObjectPosition = adsObject.position;
		adsObjectPosition = Vector3.SmoothDamp(adsObjectPosition, targetPosition, ref adsPositionVelocity, adsSmoothing);
		adsObject.position = adsObjectPosition;
	}

	#region Weapon Change

	public Weapon ChangeWeapon(WeaponID id)
	{
		var prevWeapon = Weapon;
		Weapon = weapons.Find(x => x.ID == id);
		if (Weapon == null)
		{
			Debug.LogError($"Weapons list doesn't contain an entry for {id}");
			Weapon = prevWeapon;	// Return to previous weapon
			return null;
		}

		if (prevWeapon != null)
			prevWeapon.gameObject.SetActive(false);
		Weapon.gameObject.SetActive(true);
		Weapon.Init();
		
		// TODO Hide current weapon with animation
		
		// TODO Show new weapon with animation
		
		WeaponChanged?.Invoke(Weapon);
		return Weapon;
	}

	#endregion

	#region Animations

	public void Idle()
	{
		animator.SetBool(idle, true);
		animator.SetBool(walk, false);
		animator.SetBool(sprint, false);
	}

	public void Walk()
	{
		animator.SetBool(idle, false);
		animator.SetBool(walk, true);
		animator.SetBool(sprint, false);
	}

	public void Sprint()
	{
		animator.SetBool(idle, false);
		animator.SetBool(walk, false);
		animator.SetBool(sprint, true);
	}

	public void Jump()
	{
		animator.SetTrigger(jump);
	}

	public void Land()
	{
		animator.SetTrigger(land);
	}

	public void Aim(bool aim)
	{
		isAiming = aim;
		animator.SetBool(aiming, aim);
	}

	public void IsShooting(bool shoot)
	{
		animator.SetBool(shooting, shoot);
	}

	public void OnShoot()
	{
		shot = true;
	}
	
	#endregion
}