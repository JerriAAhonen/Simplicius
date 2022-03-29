using simplicius.Audio;
using UnityEngine;

namespace simplicius.Core
{
	public class MovementState_Jumping : MovementState
	{
		[SerializeField] private float jumpHeight;
		[SerializeField] private float jumpTimingForgiveness;
		[SerializeField] private float crosshairMod;
		[Header("SFX")] 
		[SerializeField] private AudioEvent jumpSfx;
		[SerializeField] private AudioEvent landSfx;

		private float groundCheckDelay;
		private float jumpStartY;

		public override void Init(Player player, PlayerMovement movement, CharacterController cc)
		{
			base.Init(player, movement, cc);

			CrosshairModifier = crosshairMod;
		}

		public override bool CanEnter(MovementState currentState)
		{
			return movement.JumpPressed && (movement.IsGrounded || TimingForgiven());

			bool TimingForgiven() => Time.time - movement.LastTimeGrounded < jumpTimingForgiveness;
		}

		protected override void OnEnter(MovementState previousState)
		{
			base.OnEnter(previousState);
			
			if (movement.PrintDebugLogs)
				Debug.Log("[MovementState_Jumping] Enter");
			
			movement.VerticalVel.y = Mathf.Sqrt(-2f * jumpHeight * movement.Gravity);
			groundCheckDelay = 0.1f;
			jumpStartY = player.transform.position.y;

			SpeedModifier = previousState switch
			{
				MovementState_Walking => previousState.SpeedModifier,
				MovementState_Sprinting => previousState.SpeedModifier,
				_ => 1f
			};
			
			player.Anim.Jump();
			AudioManager.Instance.PlayOnce(jumpSfx);
		}

		protected override void OnUpdate()
		{
			groundCheckDelay -= Time.deltaTime;
			if (groundCheckDelay <= 0)
			{
				base.OnUpdate();

				if (movement.IsGrounded)
				{
					var hardLanding = jumpStartY - player.transform.position.y >= 2f;
					enabled = false;

					player.Anim.Land();
					AudioManager.Instance.PlayOnce(landSfx);
					
					if (movement.PrintDebugLogs)
						Debug.Log($"[MovementState_Jumping] Exit(hardLanding: {hardLanding})");
				}
			}
		}
	}
}