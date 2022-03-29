using simplicius.Util;
using UnityEngine;

namespace simplicius.Core
{
	public class MovementState_Walking : MovementState
	{
		// Animation helpers
		private bool idling;
		private bool walking;
		
		// Walking state is the default
		public override bool CanEnter(MovementState currentState)
		{
			return true;
		}

		protected override void OnEnter(MovementState previousState)
		{
			base.OnEnter(previousState);
			SpeedModifier = 1f;

			movement.Sprinting = false;
			
			if (movement.PrintDebugLogs)
				Debug.Log("[MovementState_Walking] Enter");
		}
		
		protected override void OnUpdate()
		{
			base.OnUpdate();
			if (!walking && !movement.HorizontalVel.ApproximatelyEquals(Vector3.zero))
			{
				idling = false;
				walking = true;
				player.Anim.Walk();
			}
			else if (!idling && movement.HorizontalVel.ApproximatelyEquals(Vector3.zero))
			{
				idling = true;
				walking = false;
				player.Anim.Idle();
			}
		}

		protected override void OnExit()
		{
			if (movement.PrintDebugLogs)
				Debug.Log("[MovementState_Walking] Exit");

			idling = false;
			walking = false;
		}
	}
}