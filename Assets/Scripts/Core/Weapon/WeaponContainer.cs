using UnityEngine;

public class WeaponContainer : MonoBehaviour
{
	private static readonly int idle = Animator.StringToHash("Idle");
	private static readonly int walk = Animator.StringToHash("Walk");
	private static readonly int sprint = Animator.StringToHash("Sprint");
	private static readonly int aiming = Animator.StringToHash("Aiming");
	private static readonly int shooting = Animator.StringToHash("Shooting");

	private Animator animator;

	private void Awake()
	{
		animator = GetComponent<Animator>();
	}

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
}