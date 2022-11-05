using simplicius.Audio;
using simplicius.Core;
using UnityEngine;

public enum WeaponID
{
	AssaultRifle1 = 0, 
	Pistol1 = 1
}

public enum FireMode { Single, Burst, Auto }

public class Weapon : MonoBehaviour
{
	private static readonly int shoot = Animator.StringToHash("Shoot");
	private static readonly int reload = Animator.StringToHash("Reload");
	private static readonly int show = Animator.StringToHash("Show");
	private static readonly int hide = Animator.StringToHash("Hide");

	[Header("General")] 
	[SerializeField] private WeaponID id;
	[Space]
	[SerializeField] private Transform shootPoint;
	[SerializeField] private ParticleSystem muzzleFlash;
	[Space]
	[SerializeField] private Transform bulletCasingPos;
	[SerializeField] private GameObject bulletCasing;
	[SerializeField] private WeaponProperties properties;
	[Space]
	[SerializeField] private Transform rearHandRef;
	[SerializeField] private Transform frontHandRef;
	[Header("SFX")]
	[SerializeField] private AudioEvent shootSfx;

	private Animator animator;
	private int? aimTween;

	public int AmmoInClip { get; set; }
	public int AmmoReserve { get; set; }

	public WeaponID ID => id;
	public Transform ShootPoint => shootPoint;
	public Transform RearHandRef => rearHandRef;
	public Transform FrontHandRef => frontHandRef;
	public WeaponProperties Properties => properties;
	public FireMode FireMode => properties.fireMode;
	public int BurstAmount => properties.burstAmount;
	public float MinShootInterval_Sec => 60f / properties.rateOfFire;

	public void Init()
	{
		animator = GetComponent<Animator>();

		AmmoInClip = Properties.clipSize;
		AmmoReserve = Properties.maxReserveAmmo;
	}

	public void Shoot()
	{
		Debug.Log("[Weapon] Shoot()");
		animator.SetTrigger(shoot);
		AudioManager.Instance.PlayOnce(shootSfx);
		Instantiate(muzzleFlash, shootPoint).Play(true);
		Instantiate(bulletCasing, bulletCasingPos);
	}
	
	public void Reload()
	{
		animator.SetTrigger(reload);
	}

	public void Aim(bool aim)
	{
		if (aimTween.HasValue)
			LeanTween.cancel(aimTween.Value);

		aimTween = LeanTween.value(gameObject, animator.GetLayerWeight(1), aim ? 1f : 0f, aim ? properties.adsOnDur : properties.adsOffDur)
			.setOnUpdate(v => animator.SetLayerWeight(1, v))
			.setEase(LeanTweenType.easeOutExpo)
			.uniqueId;
	}

	/// <summary>
	/// Switch to this weapon
	/// </summary>
	public void TakeOut()
	{
		animator.SetTrigger(show);
		
	}

	/// <summary>
	/// Hide this weapon to switch to a new one
	/// </summary>
	public void Hide()
	{
		animator.SetTrigger(hide);
	}
}