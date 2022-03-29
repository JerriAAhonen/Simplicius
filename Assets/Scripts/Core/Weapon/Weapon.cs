using simplicius.Audio;
using simplicius.Core;
using UnityEngine;

public enum FireMode { Single, Burst, Auto }

public class Weapon : MonoBehaviour
{
	private static readonly int shoot = Animator.StringToHash("Shoot");
	private static readonly int reload = Animator.StringToHash("Reload");
	private static readonly int show = Animator.StringToHash("Show");
	private static readonly int hide = Animator.StringToHash("Hide");
	
	[Header("General")]
	[SerializeField] private Transform shootPoint;
	[SerializeField] private WeaponProperties properties;
	[SerializeField] private Transform rearHandRef;
	[SerializeField] private Transform frontHandRef;
	[Header("SFX")]
	[SerializeField] private AudioEvent shootSfx;

	private Animator animator;
	private MuzzleFlash muzzleFlash;
	private int? aimTween;

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
		muzzleFlash = GetComponentInChildren<MuzzleFlash>();
	}

	public void Shoot()
	{
		animator.SetTrigger(shoot);
		AudioManager.Instance.PlayOnce(shootSfx);
		muzzleFlash.ShowVFX();
	}
	
	public void Reload()
	{
		animator.SetTrigger(reload);
	}

	public void Aim(bool aim)
	{
		if (aimTween.HasValue)
			LeanTween.cancel(aimTween.Value);

		aimTween = LeanTween.value(gameObject, animator.GetLayerWeight(1), aim ? 1f : 0f, properties.ADSDur)
			.setOnUpdate(v => animator.SetLayerWeight(1, v)).uniqueId;
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