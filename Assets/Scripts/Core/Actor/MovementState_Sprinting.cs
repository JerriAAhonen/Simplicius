using simplicius.Util;
using UnityEngine;

namespace simplicius.Core
{
	public class MovementState_Sprinting : MovementState
	{
		[SerializeField] private float speedMod;
		[SerializeField] private float crosshairMod;
		[SerializeField] private float fov;

		public override void Init(Player player, PlayerMovement movement, CharacterController cc)
		{
			base.Init(player, movement, cc);
			SpeedModifier = speedMod;
			CrosshairModifier = crosshairMod;
		}

		public override bool CanEnter(MovementState currentState)
		{
			if (player.Shooting.IsAiming) return false;
			if (player.Shooting.IsShooting) return false;
			return movement.Sprinting && movement.IsGrounded
			       || WasSprinting() && movement.Sprinting;
			
			bool WasSprinting() => currentState != null && currentState.PreviousWas<MovementState_Sprinting>();
		}

		protected override void OnEnter(MovementState previousState)
		{
			base.OnEnter(previousState);
			
			if (movement.PrintDebugLogs)
				Debug.Log("[MovementState_Sprinting] Enter");
			
			movement.Sprinting = true;
			player.FPCamera.SetFov(fov);
			player.Anim.Sprint();
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();

			if (InputManager.Instance.MovementInput.y <= 0 
			    || movement.HorizontalVel.ApproximatelyEquals(Vector3.zero)
			    || player.Shooting.IsAiming
			    || player.Shooting.IsShooting)
			{
				movement.Sprinting = false;
				enabled = false;
			}
		}

		protected override void OnExit()
		{
			if (movement.PrintDebugLogs)
				Debug.Log("[MovementState_Sprinting] Exit");
			
			player.FPCamera.ResetFov();
		}
	}
}