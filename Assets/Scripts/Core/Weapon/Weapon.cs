using simplicius.Audio;
using simplicius.Core;
using UnityEngine;

public enum FireMode { Single, Burst, Auto }

public class Weapon : MonoBehaviour
{
	private static readonly int shoot = Animator.StringToHash("Shoot");
	private static readonly int reload = Animator.StringToHash("Reload");
	
	[Header("General")]
	[SerializeField] private Transform shootPoint;
	[SerializeField] private WeaponProperties properties;
	[Header("SFX")]
	[SerializeField] private AudioEvent shootSfx;

	private Animator animator;
	private MuzzleFlash muzzleFlash;
	private int? aimTween;

	public Transform ShootPoint => shootPoint;
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
}