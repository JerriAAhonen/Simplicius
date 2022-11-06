using System;
using System.Collections;
using System.Collections.Generic;
using simplicius.Audio;
using simplicius.Util;
using UnityEngine;

namespace simplicius.Core
{
	public class PlayerMovement : InitializedMonoBehaviour
	{
		[SerializeField] private bool printDebugLogs;
		[SerializeField] private float speed = 5;
		[SerializeField] private float gravity = -30;
		[SerializeField] private float isGroundedRadius;

		private Player player;
		private CharacterController cc;
		private InputManager inputManager;

		private Transform tm;
		private LayerMask groundMask;
		private Vector3 groundNormal;
		private float distanceToGround;

		private List<MovementState> movementStates;
		private MovementState previousMovementState;
		
		public bool PrintDebugLogs => printDebugLogs;
		public bool IsGrounded { get; private set; }
		public bool IsFalling { get; private set; }
		public bool WasGrounded { get; private set; }
		public float LastTimeGrounded { get; set; }
		public Vector3 HorizontalVel { get; private set; }
		[HideInInspector] public Vector3 VerticalVel;
		public float Gravity => gravity;

		public event Action<MovementState> MovementStateChanged;
		public MovementState ActiveMovementState { get; private set; }

		//------------------------------
		// Input

		public bool JumpPressed { get; private set; }
		public bool CrouchPressed { get; private set; }
		public bool SprintPressed { get; private set; }
		public bool Sprinting { get; set; }
		public bool Crouching { get; set; }

		#region MonoBehaviour

		private void Awake()
		{
			cc = GetComponent<CharacterController>();
			tm = transform;
		}

		public void Init(Player player)
		{
			this.player = player;
			inputManager = InputManager.Instance;
			groundMask = PhysicsUtil.GetMask("Default");

			//------------------------------
			// Movement States

			PoolUtil.Get(ref movementStates);
			GetComponents(movementStates);
			foreach (var state in movementStates)
			{
				state.enabled = false;
				state.Init(player, this, cc);
			}

			UpdateMovementState();
			ActiveMovementState = movementStates.Find(x => x.Is<MovementState_Walking>());

			//------------------------------
			// Input

			inputManager.Jump += OnJump;
			inputManager.Crouch += OnCrouch;
			inputManager.Sprint += OnSprint;
			
			OnInitialized();
		}

		private void Update()
		{
			if (!IsReady) return;
			if (!player.IsAlive) return;
			
			RefreshIsGrounded();
			RefreshGroundInfo();
			RefreshIsFalling();

			//------------------------------
			// HORIZONTAL movement

			var horMovement = inputManager.MovementInput;
			HorizontalVel = tm.right * horMovement.x + tm.forward * horMovement.y;

			// Adjust movement direction to be vertically in the same direction as the ground 
			if (IsGrounded
			    && HorizontalVel.sqrMagnitude > Mathf.Epsilon
			    && Vector3.Dot(tm.forward, groundNormal) > 0)
			{
				Quaternion rotation = Quaternion.FromToRotation(Vector3.up, groundNormal);
				HorizontalVel = rotation * HorizontalVel;
			}

			var finalSpeed = speed * ActiveMovementState.SpeedModifier;
			cc.Move(HorizontalVel * (finalSpeed * Time.deltaTime));
			
			//------------------------------
			// Audio
			if (HorizontalVel.sqrMagnitude > 0 && IsGrounded)
			{
				if (Crouching)
					AudioManager.Instance.PlayOnce(player.CrouchingFootstepSfx);
				else if (Sprinting)
					AudioManager.Instance.PlayOnce(player.SprintingFootstepSfx);
				else
					AudioManager.Instance.PlayOnce(player.WalkingFootstepSfx);
			}

			//------------------------------
			// VERTICAL movement

			if (!IsGrounded)
				VerticalVel.y += gravity * Time.deltaTime;
			cc.Move(VerticalVel * Time.deltaTime);

			//------------------------------
			if (ActiveMovementState != null && ActiveMovementState.enabled == false)
			{
				previousMovementState = ActiveMovementState;
				ActiveMovementState = null;
				UpdateMovementState();
			}
		}

		private void OnDisable()
		{
			PoolUtil.Release(ref movementStates);
			
			inputManager.Jump -= OnJump;
			inputManager.Crouch -= OnCrouch;
			inputManager.Sprint -= OnSprint;
		}

		#endregion

		public void Rotate(float yaw)
		{
			tm.eulerAngles += new Vector3(0, yaw, 0);
		}

		#region Input

		private void OnJump()
		{
			JumpPressed = true;
			UpdateMovementState();
			JumpPressed = false;
		}

		private void OnCrouch(bool pressed, bool isToggle)
		{
			if (!isToggle)
				Crouching = pressed;		// If isn't a toggle, set crouch to input (pressed / press stopped) 
			else if (pressed)
				Crouching = !Crouching;		// If is toggle, and input was pressed, toggle Crouching

			if (!isToggle || pressed)		// If isn't toggle, update on every input, or if crouch is toggle and input was pressed update state
			{
				CrouchPressed = true;
				UpdateMovementState();
				CrouchPressed = false;
			}
		}

		private void OnSprint(bool pressed, bool isToggle)
		{
			if (!isToggle)
				Sprinting = pressed;
			else if (pressed)
				Sprinting = !Sprinting;

			if (!isToggle || pressed)
			{
				SprintPressed = true;
				UpdateMovementState();
				SprintPressed = false;
			}
		}

		#endregion

		#region MovementState_Crouching

		/// <summary>
		/// Only MovementState_Crouch should use this!
		/// </summary>
		public IEnumerator CrouchRoutine(float targetHeight, Vector3 targetCenter, float targetCamHeight, float dur)
		{
			// Handle camera movement
			var camPos = player.CameraRoot.localPosition;
			var targetCamPos = new Vector3(camPos.x, targetCamHeight, camPos.z);
			
			var elapsed = 0f;
			while (elapsed < dur)
			{
				// Handle cc
				cc.height = Mathf.Lerp(cc.height, targetHeight, elapsed / dur);
				cc.center = Vector3.Lerp(cc.center, targetCenter, elapsed / dur);
				
				// Handle camera movement
				var curCamPos = player.CameraRoot.localPosition;
				player.CameraRoot.localPosition =
					Vector3.Lerp(curCamPos, targetCamPos, elapsed / dur);

				elapsed += Time.deltaTime;
				yield return null;
			}

			cc.height = targetHeight;
			cc.center = targetCenter;
			player.CameraRoot.localPosition = targetCamPos;
		}

		#endregion

		private void UpdateMovementState()
		{
			foreach (var state in movementStates)
			{
				// TODO: Exceptions to re-enter same state?	
				if (state != ActiveMovementState && state.CanEnter(ActiveMovementState))
				{
					if (ActiveMovementState != null)
					{
						previousMovementState = ActiveMovementState;
						ActiveMovementState.enabled = false;
					}

					state.Enable(previousMovementState);
					ActiveMovementState = state;
					MovementStateChanged?.Invoke(ActiveMovementState);
					return;
				}
			}
		}

		private void RefreshIsGrounded()
		{
			WasGrounded = IsGrounded;
			IsGrounded = Physics.CheckSphere(transform.position, isGroundedRadius, groundMask);
		}

		private void RefreshIsFalling()
		{
			IsFalling = !IsGrounded && VerticalVel.y < 0f;
		}
		
		private void RefreshGroundInfo()
		{
			var hits = Physics.SphereCastNonAlloc(
				transform.position + Vector3.up,
				cc.radius,
				Vector3.down,
				PhysicsUtil.Hits,
				1000f,
				groundMask);
			if (hits > 0)
			{
				var closest = PhysicsUtil.GetClosestHit(hits);
				groundNormal = PhysicsUtil.Hits[closest].normal;
				distanceToGround = PhysicsUtil.Hits[closest].distance - 1;
				var dot = Vector3.Dot(Vector3.up, groundNormal);
				if (dot <= 0)
				{
					groundNormal = Vector3.up;
				}
				else if (dot < 0.75f)
				{
					float adjust = 1 - dot / 0.75f;
					groundNormal = Vector3.Lerp(groundNormal, Vector3.up, adjust);
				}
			}
			else
			{
				groundNormal = Vector3.up;
				distanceToGround = 0f;
			}
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.DrawWireSphere(transform.position, isGroundedRadius);
		}
	}
}