using simplicius.Util;
using UnityEngine;

namespace simplicius.Core
{
	public class MovementState_Crouching : MovementState
	{
		[SerializeField] private float crouchHeight = 1.5f;
		[SerializeField] private float crouchSpeedMod = 0.6f;
		[SerializeField] private float crosshairMod;
		
		private const float crouchTransitionDur = 0.2f;
		
		private LayerMask groundMask;
		private float defaultHeight;
		private float crouchHeightDiff;
		private Vector3 ccCenterDefaultPos;
		private Vector3 ccCenterCrouchPos;
		private float defaultCameraRootHeight;
		private float crouchCameraRootHeight;
		private Coroutine crouchRoutine;
		
		public override void Init(Player player, PlayerMovement movement, CharacterController cc)
		{
			base.Init(player, movement, cc);
			
			groundMask = PhysicsUtil.GetMask("Default");
			defaultHeight = cc.height;
			crouchHeightDiff = defaultHeight - crouchHeight;
			ccCenterDefaultPos = cc.center;
			ccCenterCrouchPos = new Vector3(0f, crouchHeight / 2, 0f);
			defaultCameraRootHeight = player.CameraRoot.localPosition.y;
			crouchCameraRootHeight = defaultCameraRootHeight - crouchHeightDiff / 2;
			
			SpeedModifier = crouchSpeedMod;
			CrosshairModifier = crosshairMod;
		}

		public override bool CanEnter(MovementState currentState)
		{
			return movement.CrouchPressed || movement.Crouching;
		}

		protected override void OnEnter(MovementState previousState)
		{
			base.OnEnter(previousState);
			
			if (movement.PrintDebugLogs)
				Debug.Log("[MovementState_Crouching] Enter");
			
			ToggleCrouch(true);
		}

		protected override void OnExit()
		{
			if (movement.PrintDebugLogs)
				Debug.Log("[MovementState_Crouching] Exit");
			
			ToggleCrouch(false);
			movement.Crouching = false;
		}

		private void ToggleCrouch(bool crouching)
		{
			if (movement.PrintDebugLogs)
				Debug.Log($"[MovementState_Crouching] Toggle({crouching})");
			
			if (crouchRoutine != null)
				StopCoroutine(crouchRoutine);
			
			// Handle cc
			var targetHeight = crouching ? crouchHeight : defaultHeight;
			var targetCenter = crouching ? ccCenterCrouchPos : ccCenterDefaultPos;
			
			// Handle camera movement
			var targetCamHeight = crouching ? crouchCameraRootHeight : defaultCameraRootHeight;
			
			crouchRoutine = StartCoroutine(movement.CrouchRoutine(targetHeight, targetCenter, targetCamHeight, crouchTransitionDur));
		}
	}
}