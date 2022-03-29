using System;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponID
{
	M4 = 0, 
	Pistol1 = 1
}

public class WeaponContainer : MonoBehaviour
{
	private static readonly int idle = Animator.StringToHash("Idle");
	private static readonly int walk = Animator.StringToHash("Walk");
	private static readonly int sprint = Animator.StringToHash("Sprint");
	private static readonly int aiming = Animator.StringToHash("Aiming");
	private static readonly int shooting = Animator.StringToHash("Shooting");

	[SerializeField] private List<WeaponMap> weapons;

	private Animator animator;
	public Weapon Weapon { get; private set; }

	public event Action<Weapon> WeaponChanged;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		foreach (var map in weapons)
			map.prefab.gameObject.SetActive(false);
	}

	#region Weapon Change

	public Weapon ChangeWeapon(WeaponID id)
	{
		var prevWeapon = Weapon;
		Weapon = weapons.Find(x => x.id == id).prefab;
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

	public void Aim(bool aim)
	{
		animator.SetBool(aiming, aim);
	}

	public void Shoot(bool shoot)
	{
		animator.SetBool(shooting, shoot);
	}
	
	#endregion
	
	[Serializable]
	private struct WeaponMap
	{
		public WeaponID id;
		public Weapon prefab;
	}
}