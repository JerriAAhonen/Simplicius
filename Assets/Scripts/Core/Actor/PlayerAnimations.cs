using simplicius.Util;
using UnityEngine;

namespace simplicius.Core
{
	public class PlayerAnimations : InitializedMonoBehaviour
	{
		private const float movementBlendTreeIdleSpeed = 0f;
		private const float movementBlendTreeWalkSpeed = 0.5f;
		private const float movementBlendTreeSprintSpeed = 1f;
		
		[SerializeField] private float idleSpeed = 1f;
		[SerializeField] private float walkSpeed = 1.7f;
		[SerializeField] private float sprintSpeed = 1.2f;
		[SerializeField] private float movementBlendTreeTransitionDur = 0.8f;

		private PlayerShooting shooting;
		private Animator characterAnimator;
		private float movementBlendTreeSpeed;
		private int? movementBlendTreeTweenId;
		
		private static readonly int ap_Speed = Animator.StringToHash("Speed");
		private static readonly int ap_MotionSpeed = Animator.StringToHash("MotionSpeed");
		private static readonly int ap_Grounded = Animator.StringToHash("Grounded");
		private static readonly int ap_Jump1 = Animator.StringToHash("Jump");

		public void Init(PlayerShooting shooting)
		{
			this.shooting = shooting;
			characterAnimator = GetComponentInChildren<Animator>();
			OnInitialized();
		}

		#region MovementBlendTree

		public void Idle()
		{
			OnMovementChanged(movementBlendTreeIdleSpeed, idleSpeed);
			shooting.WeaponContainer.Idle();
		}

		public void Walk()
		{
			OnMovementChanged(movementBlendTreeWalkSpeed, walkSpeed);
			shooting.WeaponContainer.Walk();
		}

		public void Sprint()
		{
			OnMovementChanged(movementBlendTreeSprintSpeed, sprintSpeed);
			shooting.WeaponContainer.Sprint();
		}

		private void OnMovementChanged(float newSpeed, float motionSpeed)
		{
			if (movementBlendTreeTweenId.HasValue)
				LeanTween.cancel(movementBlendTreeTweenId.Value);

			var previousSpeed = movementBlendTreeSpeed;
			movementBlendTreeTweenId = LeanTween.value(gameObject, previousSpeed, newSpeed, movementBlendTreeTransitionDur)
				.setOnUpdate(v =>
				{
					characterAnimator.SetFloat(ap_Speed, v);
					movementBlendTreeSpeed = v;
				})
				.setOnComplete(_ => movementBlendTreeSpeed = newSpeed)
				.uniqueId;
			
			characterAnimator.SetFloat(ap_MotionSpeed, motionSpeed);
		}

		#endregion

		public void Jump()
		{
			characterAnimator.SetBool(ap_Grounded, false);
			characterAnimator.SetBool(ap_Jump1, true);
			Debug.LogError("Jump");
			shooting.WeaponContainer.Jump();
		}

		public void Land()
		{
			characterAnimator.SetBool(ap_Jump1, false);
			characterAnimator.SetBool(ap_Grounded, true);
			Debug.LogError("Land");
			shooting.WeaponContainer.Land();
		}
	}
}